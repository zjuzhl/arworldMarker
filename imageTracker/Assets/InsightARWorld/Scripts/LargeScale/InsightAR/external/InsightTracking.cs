using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{

    public class InsightTracking
    {
        public static int quadCount
        {
            get
            {
                return 0;
            }
        }

        public static int reason
        {
            get
            {
                return (int)InsightARManager.Instance.ARReason;
            }
        }

        public static int status
        {
            get
            {
                if (InsightARManager.Instance.GetARInterface() != null)
                {
                    return (int)InsightARManager.Instance.ARState;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public static int type
        {
            get
            {
                return 0;
            }
        }

        //   type    算法类型，type=0表示在线光照恢复算法，type=1表示离线光照恢复算法
        public static bool EstimateIllumination(int type)
        {
            return false;
        }

        public static UnityEngine.Vector3 QuadGetCenter(string name)
        {
            List<InsightARMarkerAnchor> anchors = InsightARManager.Instance.GetInsightARMarkers();
            if (anchors == null || anchors.Count == 0) return UnityEngine.Vector3.zero;
            for (int i = 0; i < anchors.Count; i++)
            {
                if (name == anchors[i].identifier)
                {
                    return anchors[i].center;
                }
            }
            return UnityEngine.Vector3.zero;
        }

        /// <summary>
        /// 获得名称 
        /// </summary>
        /// <returns>The get name.</returns>
        /// <param name="index">Index.</param>
        public static string QuadGetName(int index)
        {
            List<InsightARMarkerAnchor> anchors = InsightARManager.Instance.GetInsightARMarkers();
            if (anchors == null || anchors.Count == 0) return string.Empty;
            for (int i = 0; i < anchors.Count; i++)
            {
                if (index == i)
                {
                    return anchors[i].identifier;
                }
            }
            return string.Empty;
        }

        public static UnityEngine.Quaternion QuadGetRotation(string name)
        {
            List<InsightARMarkerAnchor> anchors = InsightARManager.Instance.GetInsightARMarkers();
            if (anchors == null || anchors.Count == 0) return UnityEngine.Quaternion.identity;
            for (int i = 0; i < anchors.Count; i++)
            {
                if (name == anchors[i].identifier)
                {
                    return anchors[i].rotation;
                }
            }
            return UnityEngine.Quaternion.identity;
        }

        public static UnityEngine.Vector3 QuadGetScale(string name)
        {
            List<InsightARMarkerAnchor> anchors = InsightARManager.Instance.GetInsightARMarkers();
            if (anchors == null || anchors.Count == 0) return UnityEngine.Vector3.zero;
            for (int i = 0; i < anchors.Count; i++)
            {
                if (name == anchors[i].identifier)
                {
                    return anchors[i].extent;
                }
            }
            return UnityEngine.Vector3.zero;
        }

        public static bool QuadGetValid(string name)
        {
            List<InsightARMarkerAnchor> anchors = InsightARManager.Instance.GetInsightARMarkers();
            if (anchors == null || anchors.Count == 0) return false;
            for (int i = 0; i < anchors.Count; i++)
            {
                if (name == anchors[i].identifier)
                {
                    return anchors[i].isValid == 1;
                }
            }
            return false;
        }


        //返回body mask 纹理
        public static Texture2D bodyMaskTexture
        {
            get
            {
                return null;
            }
        }
        public static bool onceActiveRequestCloudLocationChecking
        {
            get
            {
#if UNITY_EDITOR
                return false;
#else
                return InsightARManager.Instance.GetARCloudLocation().onceActiveRequestCloudLocationChecking;
#endif
            }
        }

        /// <summary>
        /// 返回识别结果
        ///  TRACKING_RESULT_STRING_FACE_JSON = 0,
        ///  TRACKING_RESULT_STRING_GESTURE_JSON = 1,
        ///  TRACKING_RESULT_STRING_OBJECT_JSON = 2, // object detection
        ///  TRACKING_RESULT_STRING_QRCODE_JSON = 3,
        ///  TRACKING_RESULT_STRING_ARCODE_JSON = 4,
        ///  TRACKING_RESULT_STRING_2DIMAGE_JSON = 5,
        ///  TRACKING_RESULT_STRING_BODY_JSON = 6,
        ///  TRACKING_RESULT_STRING_COUNT = 7,
        /// </summary>
        /// <param name="trackIndex"></param>
        /// <returns></returns>
        public static string GetResultString(int trackIndex)
        {
            return InsightARManager.Instance.GetResultString(trackIndex);
        }

        //cloud location
        public static int cloudLocationStatus
        {
            get
            {
                if (InsightARManager.Instance.GetARCloudLocation() != null)
                {
                    return (int)InsightARManager.Instance.CloudLocStatus;
                }
                else
                {
                    return 0;
                }
            }

        }
        public static string cloudLocationReason
        {

            get
            {
                return InsightARManager.Instance.CloudLocReason;
            }
        }

        public static long cloudLocationSuccessCount
        {

            get
            {
                return InsightARManager.Instance.CloudLocSuccessCount;
            }
        }

        public static long cloudLocationTotalCount
        {

            get
            {
                return InsightARManager.Instance.CloudLocTotalCount;
            }
        }

        public static void RequestOnceLocNative()
        {
#if UNITY_EDITOR
            return;
#else
            InsightARManager.Instance.GetARCloudLocation().onceActiveRequestCloudLocationChecking = true;
            InsightARNative.iarRequestOnceCloudLocNative();
            InsightDebug.Log("Tracking", "RequestOnceCloudLocNative");
#endif
        }

        public static void ResetCloudLocation()
        {
            InsightARManager.Instance.GetARCloudLocation().ResetCloudLocation();
        }

        public static void SetCloudLocationType(CloudRequestType type)
        {
            InsightARManager.Instance.GetARCloudLocation().requestType = type;
        }
        public static bool SetCloudLocationRequestURL(string cloudUrl)
        {
            if (cloudUrl == null || cloudUrl == "")
            {
                return false;
            }
            //ContentResPaths.Instance.CloudURL = cloudUrl;
            return true;
        }

        public static double LightEstimate 
        {
            get
            {
                return InsightARManager.Instance.GetARInterface().LightEstimate;
            }
        }

        /* public enum AREngines_Type
         {
             NONE_SUPPORTED = 0,
             INSIGHT_AR = 1,
             ARCORE = 2,
             ARKIT = 4,
             HUAWEI_AR = 8,
         }*/
        public static int GetCurrentAREngine()
        {
#if UNITY_ANDROID
            int result = (int)InsightARNative.iarlsGetCurrentAREngine();
            return result;
#elif UNITY_IOS
            int result = (int)AREngines_Type.ARKIT;
            return result;
#else
            return 0;
#endif
        }

        public static AREngines_Type GetCurrentAREngineType()
        {
#if UNITY_ANDROID
            var result = InsightARNative.iarlsGetCurrentAREngine();
            return result;
#elif UNITY_IOS
            var result = AREngines_Type.ARKIT;
            return result;
#else
            return AREngines_Type.NONE_SUPPORTED;
#endif
        }
    }


}


