using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
class RenderSystem : MonoBehaviour
{
    struct DrawCall
    {
        public Mesh Mesh;
        public Material Material;
        public float4x4 ObjectToWorld;
    }

    private List<DrawCall> drawCalls;
    void OnEnable()
    {
        drawCalls = new List<DrawCall>();
        foreach (var meshRenderer in FindObjectsOfType<MeshRenderer>())
        {
            drawCalls.Add(new DrawCall()
            {
                Mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh,
                Material = meshRenderer.sharedMaterial,

                ObjectToWorld = meshRenderer.transform.localToWorldMatrix
            });
            meshRenderer.forceRenderingOff = true;//We'll do our own!
        }

        RenderPipelineManager.beginCameraRendering += onPreRender;
    }

    void OnDisable()
    {
        foreach (var meshRenderer in FindObjectsOfType<MeshRenderer>())
            meshRenderer.forceRenderingOff = false;

        
        RenderPipelineManager.beginCameraRendering -= onPreRender;
    }

    private void onPreRender(ScriptableRenderContext arg1, Camera cam)
    {
        Shader.SetGlobalMatrix("custom_ViewMatrix", cam.worldToCameraMatrix);
        Shader.SetGlobalMatrix("custom_ProjectionMatrix", GL.GetGPUProjectionMatrix(cam.projectionMatrix, true));

        foreach (var draw in drawCalls)
        {
            MaterialPropertyBlock matProps = new MaterialPropertyBlock();
            matProps.SetMatrix("custom_ObjectToWorld", draw.ObjectToWorld);

            Graphics.RenderMeshPrimitives(new RenderParams(draw.Material)
            {
                matProps = matProps
            }, draw.Mesh, 0);
        }

    }
}
