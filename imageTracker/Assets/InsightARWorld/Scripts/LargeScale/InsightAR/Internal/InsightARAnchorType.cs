using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{
    public enum InsightARPlaneAnchorAlignment
    {
        HorizontalUp,
        HorizontalDown,
        Vertical,
        Unknown
    }

    public enum InsightARAnchorType
    {
        Plane,
        UserHitTest,
        Marker_2D,
        Object_3D,
        Face,
        EnvrProbe,
        QRCode,
        ARCode,
    }

    public enum AREngines_Type
    {
        NONE_SUPPORTED = 0,
        INSIGHT_AR = 1,
        ARCORE = 2,
        ARKIT = 4,
        HUAWEI_AR = 8,
    }

    public enum AREngines_Availability
    {
        Available = 0,
        Unknown_Error = 1,
        JVM_Error = 2,
        Device_NO_IMU = 3,
        Device_OS_NotSupport = 4,
        Device_Arch_NotSupport = 5,
        Check_Timeout = 6,
        AREngine_NotInstalled = 7,
        AREngine_Too_Old = 8,
        Device_Camera_Disable = 9,
    }

    public struct InsightARSettings
    {
        public string configPath;
        public string appKey;
        public string appSecret;
    }
}

