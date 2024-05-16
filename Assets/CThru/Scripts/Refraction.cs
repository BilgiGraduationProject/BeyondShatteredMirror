using Autodesk.Fbx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

namespace GapperGames
{

    [ExecuteInEditMode]
    public class Refraction : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            public string TextureName = "_Refraction";
            public LayerMask LayerMask;
        }

        class GrabPass : ScriptableRenderPass
        {
            RTHandle tempColorTarget;
            RTHandle camTarget;
            Settings settings;

            RenderTargetIdentifier cameraTarget;

            public GrabPass(Settings s)
            {
                settings = s;
                renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
                //tempColorTarget.Init(settings.TextureName);
                tempColorTarget = RTHandles.Alloc(settings.TextureName, name: settings.TextureName);
                camTarget = RTHandles.Alloc("ctr", name: "ctr");
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                cmd.GetTemporaryRT(Shader.PropertyToID(tempColorTarget.name), cameraTextureDescriptor);
                cmd.GetTemporaryRT(Shader.PropertyToID(tempColorTarget.name), cameraTextureDescriptor);
                cmd.SetGlobalTexture(settings.TextureName, Shader.PropertyToID(tempColorTarget.name));
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                camTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
                cmd.GetTemporaryRT(Shader.PropertyToID(camTarget.name), renderingData.cameraData.cameraTargetDescriptor, FilterMode.Bilinear);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.isPreviewCamera)
                {
                    return;
                }

                CommandBuffer cmd = CommandBufferPool.Get();

                Blit(cmd, camTarget, tempColorTarget);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(Shader.PropertyToID(tempColorTarget.name));
                cmd.ReleaseTemporaryRT(Shader.PropertyToID(camTarget.name));
            }
        }

        class RenderPass : ScriptableRenderPass
        {
            Settings settings;
            List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

            FilteringSettings m_FilteringSettings;
            RenderStateBlock m_RenderStateBlock;

            public RenderPass(Settings settings)
            {
                this.settings = settings;
                renderPassEvent = RenderPassEvent.AfterRenderingTransparents + 1;

                m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
                m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
                m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));

                m_FilteringSettings = new FilteringSettings(RenderQueueRange.all, settings.LayerMask);
                m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                CommandBuffer cmd = CommandBufferPool.Get();

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                DrawingSettings drawSettings;
                drawSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, SortingCriteria.CommonTransparent);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        GrabPass grabPass;
        RenderPass renderPass;
        [SerializeField] Settings settings;

        public override void Create()
        {
            grabPass = new GrabPass(settings);
            renderPass = new RenderPass(settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(grabPass);
            renderer.EnqueuePass(renderPass);
        }
    }
}
