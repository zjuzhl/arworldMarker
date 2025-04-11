using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal {

    public enum InsightARError
    {
        InsightAR_ERROR_NONE = 0,
        InsightAR_ERROR_CAMERA_DEVICE = 1,
        InsightAR_ERROR_CAMERA_PERMISSION = 2,
        InsightAR_ERROR_CAMERA_TIMEOUT = 3,
        InsightAR_ERROR_CAMERA_DISABLE = 4,
        InsightAR_ERROR_CAMERA_UNKNOWN = 5,
        InsightAR_WARNING_IMU_ACCESS = 6,
        InsightAR_WARNING_ACCE_ACCESS = 7,
        InsightAR_WARNING_GYRO_ACCESS = 8,
        InsightAR_WARNING_GRAV_ACCESS = 9,
        InsightAR_ERROR_NO_IMU_DATA = 10,
        InsightAR_ERROR_ConfigFile_Not_Found = 11,
        InsightAR_ERROR_ConfigFile_Error = 12,
        InsightAR_ERROR_AppKey_Secret_Error = 13,
        InsightAR_WARNING_AR_RUNGING = 14,
        InsightAR_ERROR_Device_Unsupported = 15,
        InsightAR_WARNING_InsufficientFeatures = 16,
        InsightAR_WARNING_Track_Bad = 17,
        InsightAR_WARNING_LowLight = 18,
        InsightAR_WARNING_ExcessiveMotion = 19,
        InsightAR_ERROR_IMU_ACCESS = 20,
        InsightAR_ERROR_API_LEVEL = 21,
        InsightAR_ERROR_ARCORE_INIT_FAIL = 22,
        InsightAR_ERROR_ARCORE_RESUME_FAIL = 23,
    }
}

