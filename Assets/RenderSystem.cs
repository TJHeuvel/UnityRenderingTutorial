using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
class RenderSystem : MonoBehaviour
{
    void LateUpdate()
    {
        MaterialPropertyBlock matProps = new MaterialPropertyBlock();
        matProps.SetMatrix("custom_ObjectToWorld", transform.localToWorldMatrix);
        GetComponent<Renderer>().SetPropertyBlock(matProps);

        Shader.SetGlobalMatrix("custom_ViewMatrix", Camera.main.worldToCameraMatrix);
        Shader.SetGlobalMatrix("custom_ProjectionMatrix", GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix, true));
    }
}
