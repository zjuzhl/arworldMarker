using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static  Color Vector4ToColor(UnityEngine.Vector4 vect)
    {
        return new Color(vect.x, vect.y, vect.z, vect.w);
    }
}
