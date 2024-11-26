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
    [SerializeField] private Interact interact;
    
    [SerializeField, HideInInspector]
    Material fullscreenOutline;
    
    RTHandle outlineBuffer;
    private RTHandle cameraBuffer;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        fullscreenOutline = CoreUtils.CreateEngineMaterial(outlineShader);
        
        // Outline buffer we use to render each object into separately
        outlineBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,

            useDynamicScale: true, name: "Outline Buffer"
        );
        
        cameraBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,

            useDynamicScale: true, name: "Camera Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        CustomPassUtils.Copy(ctx, ctx.cameraColorBuffer, cameraBuffer);
        // Sets properties for outline
        ctx.propertyBlock.SetColor("_OutlineColor", outlineColor);
        ctx.propertyBlock.SetFloat("_Threshold", threshold);

        if (interact.currentlyHighlighted == null) return;
        var renderers = interact.currentlyHighlighted.GetComponentsInChildren<Renderer>();
        // set outlineBuffer as render target
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);
        foreach (var obj in renderers)
        {
            // add draw renderer command to command buffer
            ctx.cmd.DrawRenderer(obj, replacementMaterial);
        }
        // set texture that shader samples as outline buffer
        ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
        ctx.propertyBlock.SetTexture("_CameraBuffer", cameraBuffer);
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        // release unnecessary resources
        CoreUtils.Destroy(fullscreenOutline);
        cameraBuffer.Release();
        outlineBuffer.Release();
    }
}
