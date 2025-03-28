using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class ScannerPass : CustomPass
{
    [SerializeField]private List<ScanSettings> scanSettingsList = new List<ScanSettings>();
    [SerializeField] private TogglexRay scanValues;
    
    [Range(0, 10)] [SerializeField] private float threshold = 1;
    [SerializeField] private Material replacementMaterial;
    [SerializeField] private Shader outlineShader;

    [SerializeField] private LayerMask lineRenderers;

    [SerializeField] private List<Material> flowMaterials = new List<Material>();
    
    public float range;
    
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
        ShaderTagId[] shaderTags = new ShaderTagId[4]
        {
            new ShaderTagId("Forward"),
            new ShaderTagId("ForwardOnly"),
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("FirstPass"),
        };
        range = scanValues.GetRange();
        // Messy but easy way of doing this. Modifies camera settings for culling data. We set these back to normal below
        // TODO Edit cullingParameters instead of camera
        var main = Camera.main;
        mask = main.cullingMask;
        float distance = main.farClipPlane;
        main.cullingMask |= Physics.AllLayers;
        
        // makes sure we dont fuck up the camera settings
        //if (range < 1) range = 1;
        main.farClipPlane = range + 5;
        
        // Get culling data
        if (!main.TryGetCullingParameters(out var cullingParameters)) Debug.Log("err");
        cullingParameters.cullingOptions = CullingOptions.None;
        CullingResults cullingResults = ctx.renderContext.Cull(ref cullingParameters);
        
        // Render settings
        var result = new UnityEngine.Rendering.RendererUtils.RendererListDesc(shaderTags, cullingResults, main)
        {
            rendererConfiguration = PerObjectData.None,
            renderQueueRange = RenderQueueRange.all,
            sortingCriteria = SortingCriteria.BackToFront,
            excludeObjectMotionVectors = false,
            //overrideMaterial = visualMaterial,
            overrideMaterialPassIndex = 0,
            //layerMask = outlineLayer,
            stateBlock = new RenderStateBlock(RenderStateMask.Depth){ depthState = new DepthState(true, CompareFunction.Always)},
        };
        
        // Sets properties for outline
        //ctx.propertyBlock.SetFloat("_Threshold", threshold);
        replacementMaterial.SetFloat("_CheckDistance", range);
        
        foreach (var scanSetting in scanSettingsList)
        {
            ctx.propertyBlock.SetFloat("_Threshold", scanSetting.overrideThreshold ? scanSetting.threshold : threshold);
            // set scanSetting specific settings
            ctx.propertyBlock.SetColor("_OutlineColor", scanSetting.color);
            result.layerMask = scanSetting.layerMask;
            
            // Render visual override on materials if it exists in scanSettings
            if (scanSetting.overrideVisual != null)
            {
                scanSetting.overrideVisual.SetFloat("_CheckDistance", range);
                result.overrideMaterial = scanSetting.overrideVisual;
                // Set camera as render target and renders override visual material
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
        }
        //CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        //CustomPassUtils.DrawRenderers(ctx, lineRenderers);

        foreach (var mat in flowMaterials)
        {
            mat.SetFloat("_CheckDistance", range);
        }
        result.overrideMaterial = null;
        result.layerMask = lineRenderers;
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(result));
        
        // set camera settings back to normal 
        main.cullingMask = mask;
        main.farClipPlane = distance;
    }

    protected override void Cleanup()
    {
        // release unnecessary resources
        CoreUtils.Destroy(fullscreenOutline);
        outlineBuffer.Release();
    }
}