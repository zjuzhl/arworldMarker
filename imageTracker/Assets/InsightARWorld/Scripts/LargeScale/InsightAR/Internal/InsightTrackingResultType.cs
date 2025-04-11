using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{
    public enum TrackingResultType
    {
        TRACKING_RESULT_STRING_FACE_JSON = 0,
        TRACKING_RESULT_STRING_GESTURE_JSON = 1,
        TRACKING_RESULT_STRING_OBJECT_JSON = 2, // object detection
        TRACKING_RESULT_STRING_QRCODE_JSON = 3,
        TRACKING_RESULT_STRING_ARCODE_JSON = 4,
        TRACKING_RESULT_STRING_2DIMAGE_JSON = 5,
        TRACKING_RESULT_STRING_BODY_JSON = 6,
        TRACKING_RESULT_STRING_COUNT = 7,
    }

}
