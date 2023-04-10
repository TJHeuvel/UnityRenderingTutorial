using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RenderSystem : MonoBehaviour
{
    void LateUpdate()
    {
        MaterialPropertyBlock matProps = new MaterialPropertyBlock();
        matProps.SetMatrix("custom_ObjectToWorld", transform.localToWorldMatrix);
        GetComponent<Renderer>().SetPropertyBlock(matProps);
    }
}
