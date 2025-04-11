using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{
    /// <summary>
    /// // 设置UI方向，输出的结果适配该方向。
    // 设置完成之后，下一帧开始的结果输出采用设置的方向； 若设置为InsightARUIOrientationUnknown, 则输出无改变。
    // 当前只适配【相机的位置】输出。
    /// </summary>
    public enum InsightARUIOrientation
    {
        Unknown,
        Portrait,
        PortraitUpsideDown,
        LandscapeLeft,
        LandscapeRight,
    }
}
