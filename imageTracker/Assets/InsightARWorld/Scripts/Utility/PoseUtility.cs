using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PoseUtility
{
    /// <summary>
    /// 左上角pose 转换为screen position,针对人体算法
    /// 转换到左下角
    /// 如果是横竖屏 左上角(x,y) 左下角为(x,h-y)
    /// </summary>
    /// <param name="pose"></param>
    /// <param name="imageHeight"></param>
    /// <param name="imageWidth"></param>
    /// <param name="screenHeight"></param>
    /// <param name="screenWidth"></param>
    /// <returns></returns>
    public static Vector3 PosePointToScreent(Vector2 pose, int imageHeight, int imageWidth, float distance = 1.0f)
    {
        float screenHeight = (float)Screen.height;
        float screenWidth = (float)Screen.width;

        bool isPortraited = screenWidth < screenHeight;
        //算法做横竖屏转换，算法给的图片宽高，是横屏数据，因此需要调整顺序
        if (isPortraited)
        {
            //宽高调换
            int temp = imageWidth;
            imageWidth = imageHeight;
            imageHeight = temp;
        }

        float ratio = (float)imageWidth * (float)screenHeight / ((float)imageHeight * (float)screenWidth);
        float x = ((pose.x / (float)imageWidth - 0.5f) * ratio + 0.5f) * (float)screenWidth;
        float y = pose.y / (float)imageHeight * (float)screenHeight;
        // 算法已处理横竖屏，不需要再次处理
        return new Vector3(x, screenHeight - y, distance);
    }
}

