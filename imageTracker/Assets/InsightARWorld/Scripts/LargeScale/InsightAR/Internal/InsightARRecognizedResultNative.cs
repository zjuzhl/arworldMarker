
using System;

namespace InsightAR.Internal {
    public enum InsightARClassifiedType
    {
        InsightARClassifiedTypeQRCode,
        InsightARClassifiedTypeARCode,  //102
        InsightARClassifiedType2dImage, //301,仅识别图片
        InsightARClassifiedTypeObject,  //401，物体检测
        InsightARClassifiedTypeFace,    //801，
        InsightARClassifiedTypeGesture, //901
        InsightARClassifiedTypeBody,    //1001
        InsightARClassifiedTypeFaceUnity,//806
    };

    public struct InsightARRecognizedResultNative
    {
        public int isCloudMode;
        public InsightARClassifiedType type;
        public IntPtr recognizedResultPtr;
    }
}