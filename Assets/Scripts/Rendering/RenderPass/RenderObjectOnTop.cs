using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

class RenderObjectOnTop : CustomPass
{
    public LayerMask PlayerLayer = 0;
    [SerializeField, HideInInspector]
    
    RTHandle outlineBuffer;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        
        // Outline buffer we use to render each object into separately
        outlineBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,

            useDynamicScale: true, name: "Outline Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        ShaderTagId[] shaderTags = new ShaderTagId[4]
        {
            new ShaderTagId("Forward"),
            new ShaderTagId("ForwardOnly"),
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("FirstPass"),
        };
        
        if (!Camera.main.TryGetCullingParameters(out var cullingParameters)) Debug.Log("err");
        CullingResults cullingResults = ctx.renderContext.Cull(ref cullingParameters);
        
        // Render settings
        var result = new UnityEngine.Rendering.RendererUtils.RendererListDesc(shaderTags, cullingResults, Camera.main)
        {
            rendererConfiguration = PerObjectData.None,
            renderQueueRange = RenderQueueRange.all,
            sortingCriteria = SortingCriteria.CommonOpaque,
            excludeObjectMotionVectors = false,
            layerMask = PlayerLayer,
            stateBlock = new RenderStateBlock(RenderStateMask.Depth){ depthState = new DepthState(true, CompareFunction.Less)},
        };
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.All);
        CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(result));
        Blitter.BlitCameraTexture(ctx.cmd, outlineBuffer, ctx.cameraColorBuffer);
    }

    protected override void Cleanup()
    {
        // release unnecessary resources
        outlineBuffer.Release();
    }
}
