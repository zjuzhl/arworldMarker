using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class  BezierCurveUtility
{
    /// <summary>
    ///   根据距离预判控制点，如果数据超出可以认为从0开始
    /// </summary>
    /// <param name="lst">Lst.</param>
    /// <param name="startIndex">Start index.</param>
    /// <param name="endIndex">End index.</param>
    public static float  GetBezierPointRatio(Vector3[]lst, float dist, ref int start, int num = 20)
    {
        if (lst.Length == 0)
        {
            return 0.0f;
        }
        
        float result = 0;
        int startIndex = 0;
        int endIndex = 0; 
        int count = 0;
        float ratio = 0.0f;

        for (int i = start; i < start + num; i++,count++)
        {
            startIndex = i;
            endIndex = i + 1;

            if (startIndex >= lst.Length)
            {
                startIndex = startIndex - lst.Length;
            }

            if (endIndex >= lst.Length)
            {
                endIndex = endIndex - lst.Length;
            }
//            float lengthPerBezierCurve = BezierLength(lst[startIndex], lst[endIndex], 50);
            float lengthPerBezierCurve = Vector3.Distance(lst[startIndex], lst[endIndex]);

            if (result + lengthPerBezierCurve >= dist)
            { 
                ratio = (dist - result) / lengthPerBezierCurve;
                break;
            }
            else
            {
                result += lengthPerBezierCurve;
            }

            // 如果到达最后一个点仍然没有结果,返回最后一个点
            if (count == num && result < dist)
            {
                ratio = 0.0f;
                return endIndex;
            }
        }
        start = startIndex;
        return  ratio;
    }

    /// <summary>
    /// Beziers the point.
    /// </summary>
    /// <returns>The point.</returns>
    /// <param name="t">T.</param>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="order">Order.</param>
    public static Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, int order = 3)
    {
        if (order == 2)
        {
            Vector3 m0 = 0.5f * (p0 + p1);
            return BezierPoint(t, p0, m0, p1);
        }
        else if (order == 3)
        {
            float interval = 1.0f / 3.0f;
            Vector3 m0 = interval * (p0 + p1);
            Vector3 m1 = (1 - interval) * (p0 + p1);
            return BezierPoint(t, p0, m0, m1, p1);
        } 
        return t * (p0 + p1);
    }


    /// <summary>
    /// 二阶贝塞尔 
    /// </summary>
    /// <returns>The cubic bezier point.</returns>
    /// <param name="t">T.</param>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="p2">P2.</param>
    public static  Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
          
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
          
        return p;
    }

    /// <summary>
    /// 三阶段贝塞尔曲线 
    /// </summary>
    /// <returns>The point.</returns>
    /// <param name="t">T.</param>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="p2">P2.</param>
    /// <param name="p3">P3.</param>
    public static Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    /// <summary>
    /// Beziers the length.
    /// </summary>
    /// <returns>The length.</returns>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="pointCount">Point count.</param>
    public static float BezierLength(Vector3 p0, Vector3 p1, int pointCount = 30)
    {
        float interval = 1.0f / 3.0f;
        Vector3 m0 = interval * (p0 + p1);
        Vector3 m1 = (1 - interval) * (p0 + p1);
        return BezierLength(p0, m0, m1, p1);
    }

    /// <summary>
    /// 三阶段贝塞尔曲线近似长度 
    /// </summary>
    /// <returns>The length.</returns>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="p2">P2.</param>
    /// <param name="p3">P3.</param>
    /// <param name="pointCount">Point count.</param>
    public static float BezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int pointCount = 30)
    {
        if (pointCount < 2)
        {
            return 0;
        }

        //取点 默认 30个
        float length = 0.0f;
        Vector3 lastPoint = BezierPoint(0.0f / (float)pointCount, p0, p1, p2, p3);
        for (int i = 1; i <= pointCount; i++)
        {
            Vector3 point = BezierPoint((float)i / (float)pointCount, p0, p1, p2, p3);
            length += Vector3.Distance(point, lastPoint);
            lastPoint = point;
        }
        return length;
    }

    /// <summary>
    /// 返回切线 
    /// </summary>
    /// <returns>The tangent.</returns>
    /// <param name="t">T.</param>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    public static Vector3 BezierTangent(float t, Vector3 p0, Vector3 p1)
    {
        float interval = 1.0f / 3.0f;
        Vector3 m0 = interval * (p0 + p1);
        Vector3 m1 = (1 - interval) * (p0 + p1);
        return BezierTangent(t, p0, m0, m1, p1);
    }

    /// <summary>
    /// 返回bezier切线 
    /// </summary>
    /// <returns>The tangent.</returns>
    /// <param name="t">T.</param>
    /// <param name="p0">P0.</param>
    /// <param name="p1">P1.</param>
    /// <param name="p2">P2.</param>
    /// <param name="p3">P3.</param>
    public static Vector3 BezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float uu = u * u;
        float tu = t * u;
        float tt = t * t;

        Vector3 P = p0 * 3 * uu * (-1.0f);
        P += p1 * 3 * (uu - 2 * tu);
        P += p2 * 3 * (2 * tu - tt);
        P += p3 * 3 * tt;        

        //返回单位向量
        return P.normalized;
    }
}
