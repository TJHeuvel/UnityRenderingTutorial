using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

class DrawClipSpace : MonoBehaviour
{
    void OnDrawGizmos()
    {
        var camera = Camera.main;
        CameraEditorUtils.DrawFrustumGizmo(camera);


        var worldPos = transform.localToWorldMatrix * new float4(float3.zero, 1);  //Also known as:  transform.localToWorldMatrix;

        var viewPos = camera.worldToCameraMatrix * worldPos;
        var clipPos = camera.projectionMatrix * viewPos;

        var centerOfFrustumAtObjectDepth = camera.transform.position +
                            camera.transform.forward * Vector3.Dot((Vector3)worldPos - camera.transform.position, camera.transform.forward);

        clipPos.w = clipPos.w * 1.15f; //Otherwise it all *just* doesnt line up, i think its because i use object space and not each individual vertex?

        Gizmos.color = clipPos.x > -clipPos.w &&
                        clipPos.x < clipPos.w &&

                        clipPos.y > -clipPos.w &&
                        clipPos.y < clipPos.w &&

                        clipPos.z > -clipPos.w &&
                        clipPos.z < clipPos.w ? Color.green : Color.red;


        Gizmos.DrawWireCube(centerOfFrustumAtObjectDepth, new float3(clipPos.w, clipPos.w, 0));

    }
}
