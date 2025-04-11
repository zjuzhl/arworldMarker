using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InsightAR.Internal
{
    public enum InsightARTextureType
    {
        InsightAR_METAL = 0,
        InsightAR_OPENGL = 1,
        InsightAR_RAWDATA = 2
    }

    public struct InsightARTextureHandles
    {
        public IntPtr textureY;
        public IntPtr textureCbCr;

        public double timestamp;

#if UNITY_ANDROID
        public int textureTarget;
#endif
    }
}
