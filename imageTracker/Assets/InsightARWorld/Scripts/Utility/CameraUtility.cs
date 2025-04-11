using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtility
{
    /// <summary>
    /// 判断是否在视野范围内
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static bool IsWorldPositionInView(Camera camera, Vector3 worldPosition)
    {
        Vector3 viewPort = camera.WorldToViewportPoint(worldPosition);
        Vector3 normDirection = (worldPosition - camera.transform.position).normalized;
        float dot = Vector3.Dot(camera.transform.forward, normDirection);  //判断是否在相机前面
        return dot > 0 && viewPort.x >= 0 && viewPort.x <= 1 && viewPort.y >= 0 && viewPort.y <= 1;
    }

    /// <summary>
    /// 根据真实屏幕宽高重新计算fov
    /// </summary>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public static float CalculateFov(int imageWidth, int imageHeight,
        int screenWidth, int screenHeight, float[] fieldOfView)
    {
        float[] screenFov = new float[2] { 0, 0 };
        int oriScreenWidth = screenWidth;
        int oriScreenHeight = screenHeight;

        // 图像是横屏，屏幕先按照横屏处理
        if (oriScreenWidth < oriScreenHeight)
        {
            screenWidth = oriScreenHeight;
            screenHeight = oriScreenWidth;
        }

        float screenRatio = (float)screenWidth / (float)screenHeight;
        float imageRatio = (float)imageWidth / (float)imageHeight;
        float ratio = imageRatio / screenRatio;

        if (ratio < 1)
        {
            screenFov[0] = fieldOfView[0];
            screenFov[1] = 2.0f * Mathf.Atan(ratio * Mathf.Tan(0.5f * fieldOfView[1] * Mathf.PI / 180.0f)) * 180.0f / Mathf.PI;
        }
        else
        {
            screenFov[0] = 2.0f * Mathf.Atan(1.0f / ratio * Mathf.Tan(0.5f * fieldOfView[0] * Mathf.PI / 180.0f)) * 180.0f / Mathf.PI;
            screenFov[1] = fieldOfView[1];
        }
        return oriScreenWidth > oriScreenHeight ? screenFov[1] : screenFov[0];
    }
}
