using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InsightAR.Internal
{
    public static class InsightARNative
    {
        public delegate void Internal_FrameUpdate(InsightARResult result, IntPtr pHandler);

        public delegate void Internal_AnchorAdded(InsightARAnchorData anchorData, IntPtr pHandler);

        public delegate void Internal_AnchorUpdated(InsightARAnchorData anchorData, IntPtr pHandler);

        public delegate void Internal_AnchorRemoved(InsightARAnchorData anchorData, IntPtr pHandler);

        public delegate void Internal_RecognizedUpdate(InsightARRecognizedResultNative jsonRes, IntPtr pHandler);

        public delegate void Internal_MaskUpdate(IntPtr maskPointer, int width, int height, IntPtr pHandle);

        public delegate void Internal_Request_LocMap(IntPtr mapid, IntPtr pHandle);

        public delegate void Internal_Request_CloudLoc(InsightARCloudLocRequest requestData, IntPtr pHandle);

        public delegate void Internal_MaskResult(InsightARMaskResult result, IntPtr pHandle);

        public struct InsightARLightEstimateResult
        {
            public int flag;                            // scene claasification result, for indoor: 1; for outdoor: -1
            public double illuminIntensity;                // illumin intensity
            public double illuminTemperature;              // illumin temperature
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public double[] illuminColor;                 // illumin color

            public int primaryLightAvailable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] primaryLightDirection;
            public double primaryLightIntensity;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public double[] primaryLightColor;
        }

        public delegate void Internal_DebugFrameCallback(InsightARDebugFrame frameInfo, IntPtr pHandle);
#if UNITY_EDITOR
        public  static AREngines_Type iarlsSupport() { return AREngines_Type.INSIGHT_AR; }
        public static AREngines_Availability iarlsCheckAvailability(AREngines_Type type) { return AREngines_Availability.Available; }
        public static  int iarlsGetVersion() { return 0; }
        public static AREngines_Type iarlsGetCurrentAREngine() { return AREngines_Type.INSIGHT_AR; }
        public static  int iarlsRegisterAppKey(string key, string secret) { return 0; }
        public static  void iarlsInit(
            string configPath,
            string mapFileFolderPath,
            InsightARTextureType textureType,
            Internal_FrameUpdate frameUpdate,
            Internal_AnchorAdded anchorAdded,
            Internal_AnchorUpdated anchorUpdated,
            Internal_AnchorRemoved anchorRemoved,
            Internal_RecognizedUpdate recognizedUpdate,
            Internal_MaskResult maskCallback,								// 【这一项是新增的】
            Internal_Request_LocMap requestLocMap,
            Internal_Request_CloudLoc requestCloudLoc,
            IntPtr pHandler){ }

        public static void iarlsStartDebugFrame() { }
        public static void iarlsStopDebugFrame() { }

        public static void iarlsSetDebugFrameInfoCallback(Internal_DebugFrameCallback callback) { }
        public static  void iarlsReload(string configPath = "", string mapFileFolderPath = "") { }

        public static  void iarlsStop() { }

        public static void iarlsStopAsync() { }

        public static  void iarlsAdaptUIOri(InsightARUIOrientation ori) { }

        public static  void iarlsUpdateOnRenderThread() { }

        public static  InsightARTextureHandles iarlsGetVideoTextureHandles() { return new InsightARTextureHandles(); }

        /**
         * Attach a NEW ARScene to the MAIN ARScene. 当前main-AR-Scene只指代大场景定位导航跟踪场景。
         * @param configFolderPath config文件夹路径，不包括'/'
         * @param assetsFolderPath assets文件夹路径，不包括'/'
         * @return type: 3: attach-AR-Scene开始运行，挂起了main-AR-Scene。          所有输出被attach-AR-scene代替。
         *               2: attach-AR-scene成功叠加在了 main-AR-scene上面。         attach部分的输出只体现在anchor-callback、recoginized-callback
         *               1: attach-AR-scene与main-AR-scene一致，无需变化。          没有额外输出。
         *               0: attach-AR-scene不支持以上逻辑，或者是接口调运时机出错（main-AR-scene没有启动/或者是已有attachsScene在运行了等）等等。基本以维持现状为主。
         */
        public static  int iarlsStartAttachedAR(string configPath = "", string mapFileFolderPath = "") { return 3; }

        /**
       *终止附加算法
       **/
        public static void iarlsStopAttachedAR() { }

        /// 设置大场景跟踪定位的方法
        /// @param method   0:不定位；   1:本地定位；    2:云定位；     3:本地定位+云定位。
       // public static void iarlsSetLocMethod(int method) { }

        /// <summary>
        /// 云端定位结果回调给算法
        /// </summary>
        /// <param name="result"></param>
        public static void iarlsOnCloudLocalizedNative(InsightARCloudLocResult result) { }

        /// <summary>
        /// 检查arcore或者华为ar engine是否安装
        /// return 
        /// 0: not installed
        /// 1:INSTALL_REQUESTED
        /// 2:INSTALLED
        /// </summary>
        /// <param name="engineTypes"></param>
        /// <returns></returns>
        public static int RequestInstallARServiceNative(int engineTypes) { return 0; }

        /// <summary>
        /// 安卓：主动获取AR结果
        /// </summary>
        /// <returns></returns>
        public static InsightARResult iarlsGetARResultNative()
        {
            return new InsightARResult();
        }

        /// <summary>
        /// 提供内容或者SDK在需要的时候调用
        /// </summary>
        public static void iarRequestOnceCloudLocNative() { }


        public static void iarOnViewSizeChangeNative(int width, int height) { }

        public static InsightARMaskResult iarlsGetLastMaskResult() { return new InsightARMaskResult();}

        public static void iaslsSetMaskOutputTextureType(InsightARTextureType insightARTextureType) { }



        #region 测试光照估计
        public static bool isLightEstimateEnabled() { return false; }
        public static InsightARLightEstimateResult iarGetLightEstimate()
        {
            return new InsightARLightEstimateResult();
        }
        #endregion


#elif UNITY_ANDROID || UNITY_IOS

#if UNITY_ANDROID
        private const string Dll_Name = "AREngine";
        private const string Dll_Name_2 = "AREngine";    
#elif UNITY_IOS
        private const string Dll_Name = "__Internal";
        private const string Dll_Name_2 = "__Internal";
#endif




#if UNITY_ANDROID
        /**
         * 检查是否支持AR功能，返回支持的AR算法
         */
        [DllImport(Dll_Name)]
        public static extern AREngines_Type iarlsSupport();
        /**
         * 检查特定AR算法的支持情况
         */
        [DllImport(Dll_Name)]
        public static extern AREngines_Availability iarlsCheckAvailability(AREngines_Type type);
        /**
         * 获得当前AREngine的版本
         */
        [DllImport(Dll_Name)]
        public static extern int iarlsGetVersion();

        /**
         * 获得当前运行的AR算法类型
         */
        [DllImport(Dll_Name)]
        public static extern AREngines_Type iarlsGetCurrentAREngine();

          /**
         * 检查ar服务是否安装
         */
        [DllImport(Dll_Name)]
        public static extern int RequestInstallARServiceNative(int engineTypes);

        [DllImport(Dll_Name)]
        public static extern InsightARResult iarlsGetARResultNative();

        /**
         * 在Render线程Update AR任务
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsUpdateOnRenderThread();

        [DllImport(Dll_Name)]
        public static extern void iarOnViewSizeChangeNative(int width, int height);
#endif
        /**
         * 向算法端注册端云回放数据回调接口（接受端云回放数据）
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsSetDebugFrameInfoCallback(Internal_DebugFrameCallback callback);
        [DllImport(Dll_Name)]
        public static extern void iarlsStartDebugFrame();
        [DllImport(Dll_Name)]
        public static extern void iarlsStopDebugFrame();
        /**
         * 注册使用AR功能
         */
        [DllImport(Dll_Name)]
        public static extern int iarlsRegisterAppKey(string key, string secret);

        /**
         * 初始化AR功能
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsInit(
            string configPath,
            string mapFileFolderPath,
            InsightARTextureType textureType,
            Internal_FrameUpdate frameUpdate,
            Internal_AnchorAdded anchorAdded,
            Internal_AnchorUpdated anchorUpdated,
            Internal_AnchorRemoved anchorRemoved,
            Internal_RecognizedUpdate recognizedUpdate,
            Internal_MaskResult maskCallback,	
            Internal_Request_LocMap requestLocMap,
            Internal_Request_CloudLoc requestCloudLoc,
            IntPtr pHandler);
        /**
        * 开始AR，iarlsInit之后启动AR
        */
        // [DllImport(Dll_Name)]
        // public static extern void iarlsStart();
        /**
         * 重置AR，可以重启当前算法，也可以切换算法
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsReload(string configPath = "", string mapFileFolderPath = "");
        /**
         * 停止AR算法
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsStop();
        /**
         * 异步停止AR算法
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsStopAsync();
        /**
        * 释放AR算法
        */
        // [DllImport(Dll_Name)]
        // public static extern void iarlsRelease();

         [DllImport(Dll_Name)]
         public static extern void iarlsOnCloudLocalizedNative(InsightARCloudLocResult result);

        /**
        *加载POI场景附加算法，与漫游基础算法同时运行
        **/
        [DllImport(Dll_Name)]
        public static extern int iarlsStartAttachedAR(string configPath = "", string mapFileFolderPath = "");

         /**
        *终止附加算法
        **/
        [DllImport(Dll_Name)]
        public static extern void iarlsStopAttachedAR();

        /**
         * 获取当前视屏流的渲染句柄
         */
        [DllImport(Dll_Name)]
        public static extern InsightARTextureHandles iarlsGetVideoTextureHandles();

        /**
         * 开启深度学习模块， 返回1 表示成功， 返回0 表示失败。
         * 必须在运行时调用。
         */
       // [DllImport(Dll_Name)]
       // public static extern int iarlsEnableDLModule();

        /**
         * 关闭深度学习模块， 返回1 表示成功， 返回0 表示失败。
         * 必须在运行时调用。
         */
      //  [DllImport(Dll_Name)]
      //  public static extern int iarlsDisableDLModule();

      
        /**
         * 设置当前UI朝向，可同步修改输出的Camera Pose
         */
        [DllImport(Dll_Name)]
        public static extern void iarlsAdaptUIOri(InsightARUIOrientation orie);


        [DllImport(Dll_Name)]
        public static extern void iarRequestOnceCloudLocNative();

        //获取相机mask
        [DllImport(Dll_Name)]
        public static extern InsightARMaskResult iarlsGetLastMaskResult();

        //设置输出Mask 图片渲染类型
        [DllImport(Dll_Name)]
        public static extern void iaslsSetMaskOutputTextureType(InsightARTextureType insightARTextureType);


        #region 测试光照估计
        [DllImport(Dll_Name)]
        public static extern bool isLightEstimateEnabled();
          [DllImport(Dll_Name)]
        public static extern InsightARLightEstimateResult iarGetLightEstimate();
        #endregion

        // ***** 尚未实现 
        // /**
        //  * 加载地图资源
        //  */
        // [DllImport(Dll_Name_2)]
        // public static extern void loadMapAssetsNative(string mapPath);
        // /**
        //  * 卸载地图资源
        //  */
        // [DllImport(Dll_Name_2)]
        // public static extern void unloadMapAssetsNative();
        // /**
        //  * 获得地图的导航信息
        //  */
        // [DllImport(Dll_Name_2)]
        // public static extern void queryNavigationPathNative(
        //     MapPoint beginPos, MapPoint endPos,
        //     out IntPtr ptr); 
        // /**
        //  * 获取地图所有已标注物的信息
        //  */
        // [DllImport(Dll_Name_2)]
        // public static extern void queryAllMapAttachmentsInfoNative(out IntPtr ptr);
        // /**
        //  * 转换用户位置的3D坐标为Mapbox地图的2D坐标
        //  */
        // [DllImport(Dll_Name_2)]
        // public static extern void convert3DPoseTo2DMapPose(MapPoint input,out MapPoint output);
        // 尚未实现 

#endif //UNITY_ANDROID || UNITY_IOS



    }
}

