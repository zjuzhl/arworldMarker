using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{
    public class InsightARStart : Singleton<InsightARStart>
    {
        private const string ezxrCamName = "EZXRCamera";

        /// <summary>
        /// 开启ar
        /// </summary>
        public void StartARManager(string algPath, bool resetAR = false)
        {
            GameObject camGO = GameObject.Find(ezxrCamName);
            if (camGO == null)
            {
                camGO = new GameObject(ezxrCamName);
                camGO.AddComponent<Camera>();
                camGO.AddComponent<InsightARCamera>();
            }
            InsightARCamera arCamera = camGO.GetComponent<InsightARCamera>();
            InsightARManager arManager = InsightARManager.Instance;
            arManager.SetUpCamera(arCamera);
            if (resetAR)
            {
                arManager.ResetAR(algPath, arCamera);
            }
            else
            {
                arManager.Init(algPath, arCamera);
            }
        }

        /// <summary>
        /// 更新主相机
        /// 在切换主场景（且load完成）的时候，需要重置主相机
        /// </summary>
        public void RestartARCamera()
        {
            GameObject camGO = GameObject.Find("EZXRCamera");
            if (camGO == null)
            {
                camGO = new GameObject("EZXRCamera");
                camGO.AddComponent<Camera>();
                camGO.AddComponent<InsightARCamera>();
            }
            InsightARCamera arCamera = camGO.GetComponent<InsightARCamera>();
            InsightARManager.Instance.SetUpCamera(arCamera);
        }

    }
}

