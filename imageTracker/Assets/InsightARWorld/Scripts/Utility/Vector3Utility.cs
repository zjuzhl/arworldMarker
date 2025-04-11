using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Utility
{
    /// <summary>
    /// 计算两个向量的夹角
    /// 返回度数
    /// </summary>
    /// <param name="startVect"></param>
    /// <param name="endVect"></param>
    /// <returns></returns>
    public static float GetAngle360(Vector3 from, Vector3 to)
    {
        Vector3 normal = Vector3.Cross(from, to);
        if(normal.z > 0)
        {
            return Vector3.Angle(from, to);
        }
        else
        {
            return 360 - Vector3.Angle(from, to);
        }
    }

    /// <summary>
    /// 返回x z 距离
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static float GetXZPlaneDistance(Vector3 start, Vector3 end)
    {
        Vector2 startVect = new Vector2(start.x, start.z);
        Vector2 endVect = new Vector2(end.x, end.z);
        return Vector2.Distance(startVect, endVect);
    }

    /// <summary>
    /// 判断一个是否在两个点之间 
    /// </summary>
    /// <returns><c>true</c> if is point in two points line the specified point startPoint endPoint; otherwise, <c>false</c>.</returns>
    /// <param name="point">Point.</param>
    /// <param name="startPoint">Start point.</param>
    /// <param name="endPoint">End point.</param>
    public static bool IsPointInTwoPointsLine(Vector3 point, Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 pToStart = startPoint - point;
        Vector3 pToEnd = endPoint - point;
        return Vector3.Dot(pToStart, pToEnd) < 0;
    }
}
