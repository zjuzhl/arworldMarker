using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using AOT;
using UnityEngine.XR;
using System.Runtime.InteropServices;

namespace InsightAR.Internal
{
    public class InsightARInterface
    {
        #region params
        public const string TAG = "InsightARInterface";
        private static string[] trackResultList = new string[(int)TrackingResultType.TRACKING_RESULT_STRING_COUNT] { "", "", "", "", "", "", "" };
#if UNITY_IOS
        private static bool sInited = false;
#endif
        private InsightARState arState;
        private int arReason;

        private InsightARBackground ARBackground;
        #endregion
        /// <summary>
        /// 数据情况
        /// </summary>
        public void ClearData()
        {
            for(int i = 0; i < (int)TrackingResultType.TRACKING_RESULT_STRING_COUNT; i++)
            {
                trackResultList[i] = "";
            }
        }

        public List<InsightARMarkerAnchor> GetMarkerList()
        {
            return listMarkerAnchors;
        }

        public Action<InsightARMarkerAnchor> markerAddedAction;
        public Action<InsightARMarkerAnchor> markerUpdatedAction;
        public Action<InsightARMarkerAnchor> markerRemovedAction;

        public Action<InsightARPlaneAnchor> planeAddedAction;
        public Action<InsightARPlaneAnchor> planeUpdatedAction;
        public Action<InsightARPlaneAnchor> planeRemovedAction;
        public Action<InsightARRecognizedResult> recognizeAction;
        public Action<InsightARCloudLocRequestImpl> cloudLocAction;
        public Action<InsightARMaskResult> maskResultAction;
        private static InsightARResult trackResult = new InsightARResult();
        private static bool algInitedWithTrackResult = false;
        private static List<InsightARResult> listARResult;

        private static List<InsightARPlaneAnchor> listAnchorsToUpdate;
        private static List<InsightARPlaneAnchor> listAnchorsToAdd;
        private static List<InsightARPlaneAnchor> listAnchorsToRemove;

        private static List<InsightARMarkerAnchor> listMarkerAnchorsToAdd;
        private static List<InsightARMarkerAnchor> listMarkerAnchorsToUpdate;
        private static List<InsightARMarkerAnchor> listMarkerAnchorsToRemove;

        private static List<InsightARRecognizedResult> listRecognizedResults;
        //存储云端定位请求
        private static List<InsightARCloudLocRequestImpl> listCloudLocRequest;

        //记录所有的anchors
        private static List<InsightARMarkerAnchor> listMarkerAnchors;

#region custom functions
        [MonoPInvokeCallback(typeof(InsightARNative.Internal_FrameUpdate))]
        private static void onFrameUpdate(InsightARResult insightResult, IntPtr pHandler)
        {
            listARResult.Add(insightResult);
        }

        [MonoPInvokeCallback(typeof(InsightARNative.Internal_AnchorAdded))]
        private static void onAnchorAdded(InsightARAnchorData anchor, IntPtr pHandler)
        {
            if (anchor.type == InsightARAnchorType.Plane)
            {
                InsightARPlaneAnchor arPlaneAnchor = InsightARUtility.GetPlaneAnchorFromAnchorData(anchor);
                listAnchorsToAdd.Add(arPlaneAnchor);
            }
            else if (anchor.type == InsightARAnchorType.Marker_2D)
            {
                InsightARMarkerAnchor arMarkerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
                arMarkerAnchor.isValid = 1;
                listMarkerAnchorsToAdd.Add(arMarkerAnchor);
            }
            //记录所有的anchor
            InsightARMarkerAnchor markerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
            markerAnchor.isValid = 1;
            bool markerFound = false;
            for(int i = 0; i < listMarkerAnchors.Count; i++)
            {
                if (listMarkerAnchors[i].identifier == markerAnchor.identifier)
                {
                    markerFound = true;
                    break;
                }
            }
            if (!markerFound)
            {
                listMarkerAnchors.Add(markerAnchor);
            }
        }

        [MonoPInvokeCallback(typeof(InsightARNative.Internal_AnchorUpdated))]
        private static void onAnchorUpdated(InsightARAnchorData anchor, IntPtr pHandler)
        {
            if (anchor.type == InsightARAnchorType.Plane)
            {
                InsightARPlaneAnchor arPlaneAnchor = InsightARUtility.GetPlaneAnchorFromAnchorData(anchor);
                listAnchorsToUpdate.Add(arPlaneAnchor);
            }
            else if (anchor.type == InsightARAnchorType.Marker_2D)
            {
                InsightARMarkerAnchor arMarkerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
                listMarkerAnchorsToUpdate.Add(arMarkerAnchor);
            }

            //update
            InsightARMarkerAnchor markerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
            for (int i = 0; i < listMarkerAnchors.Count; i++)
            {
                if (listMarkerAnchors[i].identifier == markerAnchor.identifier)
                {
                    listMarkerAnchors[i] = markerAnchor;
                }
            }
        }

        [MonoPInvokeCallback(typeof(InsightARNative.Internal_AnchorRemoved))]
        private static void onAnchorRemoved(InsightARAnchorData anchor, IntPtr pHandler)
        {
            if (anchor.type == InsightARAnchorType.Plane)
            {
                InsightARPlaneAnchor arPlaneAnchor = InsightARUtility.GetPlaneAnchorFromAnchorData(anchor);
                listAnchorsToRemove.Add(arPlaneAnchor);
            }
            else if (anchor.type == InsightARAnchorType.Marker_2D)
            {
                InsightARMarkerAnchor arMarkerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
                listMarkerAnchorsToRemove.Add(arMarkerAnchor);
            }

            //remove  不移除，只按disable处理
            InsightARMarkerAnchor markerAnchor = InsightARUtility.GetMarkerAnchorFromAnchorData(anchor);
            markerAnchor.isValid = 0;
            for (int i = 0; i < listMarkerAnchors.Count; i++)
            {
                if (listMarkerAnchors[i].identifier == markerAnchor.identifier)
                {
                    listMarkerAnchors[i] = markerAnchor;
                }
            }
        }

        [MonoPInvokeCallback(typeof(InsightARNative.Internal_RecognizedUpdate))]
        public static void onRecognizedUpdate(InsightARRecognizedResultNative recogRes, IntPtr pHandler)
        {
            //Marshal.FreeHGlobal(jsonRes);
            InsightARRecognizedResult tmp = InsightARUtility.GetARRecognizedResult(recogRes);
            listRecognizedResults.Add(tmp);
            // update tracking result
            Debug.Log("ezxr tmp recognized type: " + tmp.type + "result: " + tmp.recognizedResult);

            TrackingResultType trackType = TrackingResultType.TRACKING_RESULT_STRING_2DIMAGE_JSON;
            if (tmp.type == InsightARClassifiedType.InsightARClassifiedType2dImage)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_2DIMAGE_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeARCode)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_ARCODE_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeBody)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_BODY_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeFace)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_FACE_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeGesture)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_GESTURE_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeObject)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_OBJECT_JSON;
            }
            else if (tmp.type == InsightARClassifiedType.InsightARClassifiedTypeQRCode)
            {
                trackType = TrackingResultType.TRACKING_RESULT_STRING_QRCODE_JSON;
            }
            trackResultList[(int)trackType] = tmp.recognizedResult;
        }

        /// <summary>
        /// 请求小地图
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="pHandler"></param>
        [MonoPInvokeCallback(typeof(InsightARNative.Internal_Request_LocMap))]
        private static void onRequestLocMap(IntPtr mapId, IntPtr pHandler) { }

        /// <summary>
        /// 请求云端重定位
        /// </summary>
        /// <param name="cloudLocRequestData"></param>
        /// <param name="pHandler"></param>
        [MonoPInvokeCallback(typeof(InsightARNative.Internal_Request_CloudLoc))]
        public static void onRequestCloudLoc(InsightARCloudLocRequest cloudLocRequestData, IntPtr pHandler)
        {
            InsightARCloudLocRequestImpl requestImpl = new InsightARCloudLocRequestImpl(cloudLocRequestData);
            listCloudLocRequest.Add(requestImpl);
        }

        /// <summary>
        /// 人体分割/天空分割
        /// </summary>
        /// <param name="cloudLocRequestData"></param>
        /// <param name="pHandler"></param>
        [MonoPInvokeCallback(typeof(InsightARNative.Internal_MaskResult))]
        public static void onMaskResult(InsightARMaskResult result, IntPtr pHandler)
        {

        }

#endregion

#region PUBLIC_API
     
        public void StartAR(InsightARSettings settings)
        {
            checkARSupport(settings);
            registerInsightAR(settings);
            startInsightAR(settings);      
        }

        public void StopAR()
        {
            arState = InsightARState.Uninitialized;
#if UNITY_ANDROID || UNITY_IOS
            ResetConfigARCamera();
#if UNITY_IOS
            sInited = false;
#endif
            algInitedWithTrackResult = false;

            listARResult.Clear();
            listAnchorsToAdd.Clear();
            listAnchorsToUpdate.Clear();
            listAnchorsToRemove.Clear();
            listMarkerAnchorsToAdd.Clear();
            listMarkerAnchorsToUpdate.Clear();
            listMarkerAnchorsToRemove.Clear();
            listCloudLocRequest.Clear();
            listMarkerAnchors.Clear();
#if UNITY_IOS
            InsightARNative.iarlsStopAsync();
#elif UNITY_ANDROID
            InsightARNative.iarlsStop();
#endif
            InsightDebug.Log(TAG, "-ar- StopAR");
            // InsightARNative.iarlsRelease();
#endif
        }

        public void ResetAR(string path)
        {
#if UNITY_ANDROID || UNITY_IOS
#if UNITY_IOS
            sInited = false;
#endif
            InsightARNative.iarlsReload(path);
            InsightDebug.Log(TAG, "-ar- ResetAR over" + path);
#endif
        }

        public void SetupCamera(Camera camera)
        {
            ARBackground = new InsightARBackground();
            ARBackground.SetARCamera(camera);
            //reset
            ResetConfigARCamera();
        }

        /// <summary>
        /// 重置绘制相关数据
        /// </summary>
        public void ResetConfigARCamera()
        {
            if (ARBackground != null) {
                ARBackground.ResetConfigARCamera();
            }
        }

        public void AdaptUIOrientation(InsightARUIOrientation orien)
        {

#if UNITY_ANDROID
            InsightARNative.iarlsAdaptUIOri(orien);
#endif
        }

        public void AdaptViewChange(int viewWidth,int viewHeight)
        {

#if UNITY_ANDROID
            InsightARNative.iarOnViewSizeChangeNative(viewWidth,viewHeight);
#endif
        }

        private void registerInsightAR(InsightARSettings settings)
        {
#if UNITY_ANDROID || UNITY_IOS
            int res = InsightARNative.iarlsRegisterAppKey(settings.appKey, settings.appSecret);
#endif
        }

        private void startInsightAR(InsightARSettings settings)
        {
            trackResult = new InsightARResult();
            string configPath = settings.configPath;
            string assetPath = string.IsNullOrEmpty(configPath) ? "" : configPath + "/assets";
#if UNITY_ANDROID || UNITY_IOS
            listARResult = new List<InsightARResult>();
            listAnchorsToUpdate = new List<InsightARPlaneAnchor>();
            listAnchorsToAdd = new List<InsightARPlaneAnchor>();
            listAnchorsToRemove = new List<InsightARPlaneAnchor>();
            listMarkerAnchorsToAdd = new List<InsightARMarkerAnchor>();
            listMarkerAnchorsToUpdate = new List<InsightARMarkerAnchor>();
            listMarkerAnchorsToRemove = new List<InsightARMarkerAnchor>();
            listRecognizedResults = new List<InsightARRecognizedResult>();
            listMarkerAnchors = new List<InsightARMarkerAnchor>();
            listCloudLocRequest = new List<InsightARCloudLocRequestImpl>();
            InsightARNative.iarlsInit(configPath,
                                      assetPath,
#if UNITY_ANDROID
                                      InsightARTextureType.InsightAR_OPENGL,
#elif UNITY_IOS
                                      InsightARTextureType.InsightAR_METAL,
#endif
                                      onFrameUpdate,
                                      onAnchorAdded, onAnchorUpdated, onAnchorRemoved, onRecognizedUpdate,
                                      onMaskResult,
                                      onRequestLocMap, onRequestCloudLoc,
                                      IntPtr.Zero);
#endif
#if UNITY_IOS
            InsightARNative.iaslsSetMaskOutputTextureType(InsightARTextureType.InsightAR_OPENGL);
#endif
            InsightDebug.Log(TAG, "-ar- Init AR " + settings.configPath + "\n" + assetPath);
        }
        public void Update()
        {
//#if UNITY_ANDROID
//            // Android V1.7.0
//            // 2022.11.04版本
//            // Unity对应修改，ARBaseManager.cs中InsightARInterface.UpdateARBackground()的调用位置需要尽量前移，如Mono.Update()开始时（单线程渲染模式下）。
//            UpdateARBackground();
//#endif
            updateInsightARPlanes();
            updateInsightRecogeResults();
            updateInsightARMarkerAnchors();
            updateInsightARCloudLocRequest();
        }

        public void LateUpdate() 
        {
//#if UNITY_IOS
            UpdateARBackground();
//#endif

#if UNITY_ANDROID
            UpdataInsightARResult(InsightARNative.iarlsGetARResultNative());
#elif UNITY_IOS
            updateInsightARResult();
#endif
            UpdateInsightAR();
        }
        private void UpdateARBackground()
        {
            updateInsightARBackground();
        }
#endregion

#region private_API
        private void checkARSupport(InsightARSettings settings)
        {
#if !UNITY_EDITOR
            InsightDebug.Log(TAG, "-ar- checkARSupport " + settings.appKey + "  " + settings.appSecret);
#endif
#if UNITY_ANDROID
            //InsightARNative.RequestInstallARServiceNative(AREngines_Type.ARCore|AREngines_Type.HUAWEI);
            //check InsightAR support
            AREngines_Type res = InsightARNative.iarlsSupport();
#endif
        }

        private void updateInsightARBackground()
        {
#if !UNITY_EDITOR
            if (!algInitedWithTrackResult) {
                //算法未完全inited
                InsightDebug.Log(TAG, "alg has not total inited with green screen!");
                return;
            }
#endif
            if (ARBackground != null) {
                ARBackground.UpdateARBackground(trackResult, arState);
            }
        }

        private void UpdataInsightARResult(InsightARResult insightResult)
        {
#if UNITY_EDITOR
            insightResult = InsightARUtility.CreateARResult();
#endif

            InsightARState state = (InsightARState)insightResult.state;
            //解决iOS resetar后还会发送tracking状态问题
#if UNITY_IOS
            if (!sInited)
            {
                if (state == InsightARState.Detecting)
                {
                    sInited = true;
                }
                else if (state == InsightARState.Tracking) //解决一些resetAR误判问题
                {
                    trackResult.state = (int)InsightARState.Initing;
                    return;
                }
            }
#endif
            algInitedWithTrackResult = true;//算法成功运行并输出结果

            if (state == InsightARState.Initing || state == InsightARState.Init_Fail
                || state == InsightARState.Detect_Fail || state == InsightARState.Track_Fail
                || state == InsightARState.Track_Stop || state == InsightARState.Uninitialized
#if UNITY_IOS
                || state == InsightARState.CameraStartOK
#endif
                )
            {
                trackResult.state = insightResult.state;
            }
            else
            {
                trackResult = insightResult;
            }
        }

        private void updataInsightARMaskResult(InsightARMaskResult maskResult)
        {
            if (ARBackground != null) {
                ARBackground.updataInsightARMaskResult(maskResult);
            }
        }
        private void UpdateInsightAR()
        {
            arState = (InsightARState)trackResult.state;
            arReason = trackResult.reason;
            if (arState == InsightARState.Init_Fail || 
                arState == InsightARState.Track_Fail || 
                arState == InsightARState.Detect_Fail || 
                arState == InsightARState.Initing || 
                arState == InsightARState.Track_Stop || 
                arState == InsightARState.Uninitialized)
            {
                return;
            }

            if (ARBackground != null) {
                ARBackground.UpdateARCameraPose(trackResult, arState);
            }
        }
#if UNITY_IOS
        private void updateInsightARResult() {
#if UNITY_EDITOR
            UpdataInsightARResult(InsightARUtility.CreateARResult());
#endif
            var length = listARResult.Count;
            for (int i = 0; i < length; i++) {
                InsightARResult arResult = listARResult[i];
                UpdataInsightARResult(arResult);
            }
            listARResult.Clear();
        }
#endif
        private void updateInsightARPlanes()
        {
            if (listAnchorsToAdd == null || listAnchorsToUpdate == null || listAnchorsToRemove == null) {
                return;
            }
            InsightARPlaneAnchor tPlane;
            int length = listAnchorsToAdd.Count;
            for (int i = 0; i < length; i++)
            {
                tPlane = listAnchorsToAdd[i];
                if (planeAddedAction != null)
                    planeAddedAction(tPlane);
            }
            listAnchorsToAdd.Clear();

            length = listAnchorsToUpdate.Count;
            for (int i = 0; i < length; i++)
            {
                tPlane = listAnchorsToUpdate[i];
                if (planeUpdatedAction != null)
                    planeUpdatedAction(tPlane);
            }
            listAnchorsToUpdate.Clear();

            length = listAnchorsToRemove.Count;
            for (int i = 0; i < length; i++)
            {
                tPlane = listAnchorsToRemove[i];
                if (planeRemovedAction != null)
                    planeRemovedAction(tPlane);
            }
            listAnchorsToRemove.Clear();
        }

        // xnh : remove放置在最前和现在的多marker的渲染方式有关， 之后会去掉这个约束。
        private void updateInsightARMarkerAnchors()
        {
            if (listMarkerAnchorsToRemove == null ||
                listMarkerAnchorsToAdd == null ||
                listMarkerAnchorsToUpdate == null)
            {
                return;
            }
            InsightARMarkerAnchor tMarker;
            int length = listMarkerAnchorsToRemove.Count;
            for (int i = 0; i < length; i++)
            {
                tMarker = listMarkerAnchorsToRemove[i];
                if (markerRemovedAction != null)
                    markerRemovedAction(tMarker);
            }
            listMarkerAnchorsToRemove.Clear();

            length = listMarkerAnchorsToAdd.Count;
            for (int i = 0; i < length; i++)
            {
                tMarker = listMarkerAnchorsToAdd[i];
                if (markerAddedAction != null)
                    markerAddedAction(tMarker);
            }
            listMarkerAnchorsToAdd.Clear();

            length = listMarkerAnchorsToUpdate.Count;
            for (int i = 0; i < length; i++)
            {
                tMarker = listMarkerAnchorsToUpdate[i];
                if (markerUpdatedAction != null)
                    markerUpdatedAction(tMarker);
            }
            listMarkerAnchorsToUpdate.Clear();
        }

        private void updateInsightRecogeResults()
        {
            if (listRecognizedResults == null )
            {
                return;
            }
            int length = listRecognizedResults.Count;
            for (int i = 0; i < length; i++)
            {
                InsightARRecognizedResult res = listRecognizedResults[i];
                if (recognizeAction != null)
                    recognizeAction(res);
            }
            listRecognizedResults.Clear();
        }

        /// <summary>
        /// 更新cloud loc 请求
        /// </summary>
        private void updateInsightARCloudLocRequest()
        {
#if UNITY_EDITOR
            //if (cloudLocAction != null) {
            //    var res = InsightARUtility.CreateCloudLocationData();
            //    if (!string.IsNullOrEmpty(res.jpgStr) && !string.IsNullOrEmpty(res.requestInfoStr)) {
            //        cloudLocAction(res);
            //    }
            //}
#else
            if (listCloudLocRequest == null)
            {
                return;
            }
            int length =  listCloudLocRequest.Count;
            for (int i = 0; i < length; i++)
            {
                var res = listCloudLocRequest[i];
                if (cloudLocAction != null)
                    cloudLocAction(res);
            }
            listCloudLocRequest.Clear();
#endif
        }
#endregion

#region public attribute
        public int InsightARSceenWidth
        {
            get
            {
                return trackResult.param.width;
            }
        }

        public int InsightARSceenHeight
        {
            get
            {
                return trackResult.param.height;
            }
        }

        public InsightARState ARState
        {
            get
            {
                return arState;
            }
        }

        public int ARReason
        {
            get
            {
                return arReason;
            }
        }

        public InsightARResult GetTrackingResult
        {
            get
            {
                return trackResult;
            }
        }

        public string GetResultString(int idx)
        {
            if (idx < trackResultList.Length)
            {
                return trackResultList[idx];
            }
            return string.Empty;
        }

        public double LightEstimate
        {
            get {
                bool isRes = InsightARNative.isLightEstimateEnabled();
                if (isRes)
                {
                    InsightARNative.InsightARLightEstimateResult res;
                    res = InsightARNative.iarGetLightEstimate();
                    Debug.LogFormat("IARAPI-Light intensity: {0}  tempa: {1} tempacolor[3]: {2} {3} {4} \n", res.illuminIntensity, res.illuminTemperature, res.illuminColor[0], res.illuminColor[1], res.illuminColor[2]);
                    return res.illuminIntensity;
                }
                return 0;
            }
        }
#endregion
    }
}
