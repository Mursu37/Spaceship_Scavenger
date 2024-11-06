using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

class outlineRendering : CustomPass
{
    public LayerMask outlineLayer = 0;
    [ColorUsage(false, true)]
    public Color outlineColor = Color.black;
    [Range(0, 10)] public float threshold = 1;
    public Material replacementMaterial;
    public Shader outlineShader;
    
    [SerializeField, HideInInspector]
    Material fullscreenOutline;
    
    RTHandle outlineBuffer;
    RTHandle finalOutlines;

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
        
        // get renderers
        var renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        
        // Sets properties for outline
        ctx.propertyBlock.SetColor("_OutlineColor", outlineColor);
        ctx.propertyBlock.SetFloat("_Threshold", threshold);

        /*
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);
        
        CustomPassUtils.DrawRenderers(ctx, outlineLayer, RenderQueueType.All, replacementMaterial, 0, 
            default(RenderStateBlock), SortingCriteria.CommonTransparent);
        
        ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
        
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
        */
        
        // Render each objects outline in outlineLayer (layermask) separately to get the object outlines through walls 
        foreach (var obj in renderers)
        {
            // checks if layer is in layermask
            if ((outlineLayer & (1 << obj.gameObject.layer)) != 0)
            {
                // set outlineBuffer as render target
                CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);
                
                // add draw renderer command to command buffer
                ctx.cmd.DrawRenderer(obj, replacementMaterial);
                
                // set texture that shader samples as outline buffer
                ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
                
                // renders objects outline to camera
                CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
                CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
            }
        
        }

        // should be redundant now. need to test
        /*
        ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
        */
    }

    protected override void Cleanup()
    {
        // release unnecessary resources
        CoreUtils.Destroy(fullscreenOutline);
        outlineBuffer.Release();
    }
}
