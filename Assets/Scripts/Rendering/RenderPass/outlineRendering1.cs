using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

class outlineRendering1 : CustomPass
{
    public LayerMask outlineLayer = 0;
    [ColorUsage(false, true)]
    public Color outlineColor = Color.black;

    public Color fadeCheck = Color.white;
    [Range(0, 10)] public float threshold = 1;
    public float range = 10;
    public float nearPlane = 1;
    public Material replacementMaterial;
    public Material visualMaterial;
    public Shader outlineShader;
    
    [SerializeField, HideInInspector]
    Material fullscreenOutline;
    
    RTHandle outlineBuffer;
    RTHandle finalOutlines;
    private LayerMask mask;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        fullscreenOutline = CoreUtils.CreateEngineMaterial(outlineShader);
        
        // Outline buffer we use to render each object into separately
        outlineBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,

            useDynamicScale: true, name: "Outline Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        var x = Object.FindObjectOfType<TogglexRay>(false);
        if (x != null) range = x.GetRange();
        // Sets properties for outline
        ctx.propertyBlock.SetColor("_OutlineColor", outlineColor);
        ctx.propertyBlock.SetFloat("_Threshold", threshold);
        replacementMaterial.SetFloat("_CheckDistance", range);
        visualMaterial.SetFloat("_CheckDistance", range);
        ctx.propertyBlock.SetColor("_fadeColor", fadeCheck);
        
        ShaderTagId[] shaderTags = new ShaderTagId[4]
        {
            new ShaderTagId("Forward"),
            new ShaderTagId("ForwardOnly"),
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("FirstPass"),
        };
        
        // Messy but easy way of doing this. Modifies camera settings for culling data. We set these back to normal below
        // TODO Edit cullingParameters instead of camera
        mask = Camera.main.cullingMask;
        float distance = Camera.main.farClipPlane;
        float nearDistance = Camera.main.nearClipPlane;
        Camera.main.cullingMask |= outlineLayer;
        
        // makes sure we dont fuck up the camera settings
        if (range < 1) range = 1;
        if (nearPlane <= 0) nearPlane = 0.1f;
        Camera.main.farClipPlane = range + 100;
        Camera.main.nearClipPlane = nearPlane;
        
        // Get culling data
        if (!Camera.main.TryGetCullingParameters(out var cullingParameters)) Debug.Log("err");
        CullingResults cullingResults = ctx.renderContext.Cull(ref cullingParameters);
        
        // Render settings
        var result = new UnityEngine.Rendering.RendererUtils.RendererListDesc(shaderTags, cullingResults, Camera.main)
        {
            rendererConfiguration = PerObjectData.None,
            renderQueueRange = RenderQueueRange.all,
            sortingCriteria = SortingCriteria.BackToFront,
            excludeObjectMotionVectors = false,
            overrideMaterial = visualMaterial,
            overrideMaterialPassIndex = 0,
            layerMask = outlineLayer,
            stateBlock = new RenderStateBlock(RenderStateMask.Depth){ depthState = new DepthState(true, CompareFunction.Always)},
        };
        // If visual material override is set render a material effect on the objects before outlines
        if (visualMaterial != null)
        {
            CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
            CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(result));
        }

        // Render objects to a separate buffer for outline detection. replacement material helps with outline detection
        result.overrideMaterial = replacementMaterial;
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);
        CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(result));
        
        // Draw outlines using the buffer rendered above
        ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
        
        // set camera settings back to normal 
        Camera.main.cullingMask = mask;
        Camera.main.farClipPlane = distance;
        Camera.main.nearClipPlane = nearDistance;
    }

    protected override void Cleanup()
    {
        // release unnecessary resources
        CoreUtils.Destroy(fullscreenOutline);
        outlineBuffer.Release();
    }
}
