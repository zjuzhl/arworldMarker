using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InsightAR.Internal;

/// <summary>
/// ar camera 组件
/// </summary>
public class InsightARCamera : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    public Camera ARCamera
    {
        get {
            if (cam == null) {
                cam = gameObject.GetComponent<Camera>();
            }
            return cam;
        }
    }
}
