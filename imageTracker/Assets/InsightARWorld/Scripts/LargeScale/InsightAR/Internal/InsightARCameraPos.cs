using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace InsightAR.Internal
{
    public struct InsightARCameraPose
    {
        //OpenGL坐标系
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] quaternion_opengl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] center_opengl;

        //U3D坐标系

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] quaternion_u3d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] center_u3d;
    }
}
