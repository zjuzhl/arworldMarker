using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// string utility
/// </summary>
public class StringUtility
{
    /// <summary>
    /// parse int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ParseInt(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        int result;
        if (int.TryParse(str, out result))
        {
            return result;
        }
        return 0;
    }

    /// <summary>
    /// parse float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ParseFloat(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0.0f;
        float result;
        if (float.TryParse(str, out result))
        {
            return result;
        }
        return 0;
    }

    /// <summary>
    /// parse double
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static double ParseDouble(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        double result;
        if (double.TryParse(str, out result))
        {
            return result;
        }
        return 0;
    }

    /// <summary>
    /// parse long
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static long ParseLong(string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        long result;
        if (long.TryParse(str, out result))
        {
            return result;
        }
        return 0;
    }

    /// <summary>
    /// vector3 from vector3.tostring 
    /// </summary>
    /// <returns>The to vec.</returns>
    /// <param name="s">S.</param>
    public static Vector3 ParseVector3(string s)
    {
        string[] temp = s.Substring(1, s.Length - 2).Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }

    /// <summary>
    /// vector3 from vector3.tostring 
    /// </summary>
    /// <returns>The to vec.</returns>
    /// <param name="s">S.</param>
    public static Quaternion ParseQuaternion(string s)
    {
        string[] temp = s.Substring(1, s.Length - 2).Split(',');
        return new Quaternion(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3]));
    }

}
