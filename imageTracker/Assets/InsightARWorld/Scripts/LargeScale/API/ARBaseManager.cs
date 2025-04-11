using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace InsightAR.Internal
{
    public class ARBaseManager : MonoBehaviour
    {
        #region params
        private const string TAG = "ARBaseManager";
        [SerializeField]
        protected Camera m_ARCamera;

        protected InsightARInterface m_ARInterface;
        protected InsightARCloudLocation cloudLocation;

        protected bool _isRungning = false;

        private InsightARSettings _ARSetting;

        /// <summary>
        /// AR运行状态
        /// </summary>
        public bool isRunning
        {
            get {
                return _isRungning;
            }
        }

        public InsightARInterface GetARInterface()
        {
            return m_ARInterface;
        }
        public InsightARCloudLocation GetARCloudLocation()
        {
            return cloudLocation;
        }

        public string GetResultString(int idx)
        {
            return m_ARInterface.GetResultString(idx);
        }


        /// <summary>
        /// 暴露当前算法状态
        /// </summary>
        public InsightARState ARState
        {
            get
            {
                return m_ARInterface.ARState;
            }
        }

        public int ARReason
        {
            get
            {
                return m_ARInterface.ARReason;
            }
        }

        public int ARScreenWidth
        {
            get
            {
                return m_ARInterface.InsightARSceenWidth;
            }
        }

        public int ARScreenHeight
        {
            get
            {
                return m_ARInterface.InsightARSceenHeight;
            }
        }

        public InsightARResult GetTrackingResult
        {
            get
            {
                return m_ARInterface.GetTrackingResult;
            }
        }

        //cloud status
        public int CloudLocStatus
        {
            get {
                return cloudLocation.CloudLocAlgCode;
            }
        }

        public string CloudLocReason
        {
            get
            {
                return cloudLocation.CloudLocAlgReason;
            }
        }

        public long CloudLocSuccessCount
        {
            get
            {
                return cloudLocation.CloudSuccessCount;
            }
        }

        public long CloudLocTotalCount
        {
            get
            {
                return cloudLocation.CloudLocTotalCount;
            }
        }

        /// <summary>
        ///  返回markers
        /// </summary>
        /// <returns></returns>
        public List<InsightARMarkerAnchor> GetInsightARMarkers()
        {
            return m_ARInterface.GetMarkerList();
        }

        public Camera GetCamera()
        {
            return m_ARCamera;
        }

        #endregion

        #region unity_functions

        public void Update()
        {
            if (!_isRungning)
                return;
            checkScreenChange();

            m_ARInterface.Update();

        }

        public void LateUpdate() {
            if (!_isRungning)
                return;

            m_ARInterface.LateUpdate();
        }

        
        #endregion

        #region custom_functions
        /// <summary>
        /// 清理缓存数据
        /// </summary>
        public void ClearData()
        {
            m_ARInterface.ClearData();
            cloudLocation.ResetCloudLocation();
        }

        public virtual void InitAR(string configDir = "")
        {
            DoStartAR();
        }


        public virtual void StopAR()
        {
            if (!_isRungning)
            {
                return;
            }
            _isRungning = false;

            //cloud location close
            cloudLocation.RemoverListener();

            m_ARInterface.StopAR();

            ClearData();
        }

        public virtual void ResetAR()
        {
            m_ARInterface.ResetAR(_ARSetting.configPath);
        }

        public virtual void ResetAR(string path)
        {
            _ARSetting.configPath = path;
            ResetAR();
        }

        public void SetConfig(string _configPath, string _appKey, string _appSecret)
        {
            _ARSetting = new InsightARSettings()
            {
                configPath = _configPath,
                appKey = _appKey,
                appSecret = _appSecret
            };
        }
        public void SetConfig(string _configPath)
        {
            _ARSetting.configPath = _configPath;
        }

        public void SetUpCamera(InsightARCamera arCamera)
        {
            m_ARCamera = arCamera.ARCamera;
            if(m_ARInterface == null)
                m_ARInterface = new InsightARInterface();
            m_ARInterface.SetupCamera(m_ARCamera);
        }

        protected void DoStartAR()
        {
            if (m_ARInterface == null)
                m_ARInterface = new InsightARInterface();
            m_ARInterface.StartAR(_ARSetting);
            m_ARInterface.SetupCamera(m_ARCamera);

            //云端定位
            cloudLocation = new InsightARCloudLocation();
            cloudLocation.AddListener();

            _isRungning = true;
        }

        private ScreenOrientation lastScreenOrientation = ScreenOrientation.Unknown;
        private  Resolution lastScreenSize = new Resolution();
        private void checkScreenChange() {

            if (!isRunning)
            {
                return;
            }
            if (Screen.orientation != lastScreenOrientation) {
                InsightARUIOrientation iori = InsightARUIOrientation.Portrait;
                if (Screen.orientation == ScreenOrientation.LandscapeLeft)
                {
                    iori = InsightARUIOrientation.LandscapeLeft;
                }
                else if (Screen.orientation == ScreenOrientation.Portrait) {
                    iori = InsightARUIOrientation.Portrait;
                } else if (Screen.orientation == ScreenOrientation.LandscapeRight)
                {
                    iori = InsightARUIOrientation.LandscapeRight;
                }
                else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    iori = InsightARUIOrientation.PortraitUpsideDown;
                }
                m_ARInterface.AdaptUIOrientation(iori);
                lastScreenOrientation = Screen.orientation;
            }
            if (Screen.currentResolution.width != lastScreenSize.width || Screen.currentResolution.height != lastScreenSize.height) {
                m_ARInterface.AdaptViewChange(Screen.currentResolution.width, Screen.currentResolution.height);
            }
        }

        #endregion
    }
}
