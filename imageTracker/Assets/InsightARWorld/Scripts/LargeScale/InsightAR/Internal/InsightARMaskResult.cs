using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InsightAR.Internal
{
    public struct InsightARMaskResult
    {
        public IntPtr maskPtr;                  // 纹理指针 / rawdata数据指针
        public int width, height;               // 长度和宽度
        public uint pixelFormat;             // 目前都是单通道R8输出，暂时可以无视这个选项		
        public InsightARTextureType maskType;	// InsightAR_METAL / InsightAR_OPENGL / InsightAR_RAWDATA 三种，与iaslsInit设置的一致

    }
}
