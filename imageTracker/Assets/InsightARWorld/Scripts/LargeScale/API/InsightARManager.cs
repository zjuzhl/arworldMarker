using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

namespace InsightAR.Internal
{
    public class InsightARManager : ARBaseManager
    {
        public const string TAG = "InsightARManager";
        private static InsightARManager _instance;
        public static InsightARManager Instance
        {
            get
            {
                _instance = _instance == null? new GameObject("ARManager").AddComponent<InsightARManager>(): _instance;
                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = gameObject.GetComponent<InsightARManager>();
            }
            else
            {
                Destroy(gameObject);
            }
            //
            // NOTE:暂时改为卸载时销毁
            // 
            //DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="algorithPath"></param>
        /// <param name="arCamera"></param>
        public void Init(string algorithPath, InsightARCamera arCamera)
        {
            m_ARCamera = arCamera.ARCamera;
            InitAR(algorithPath);
        }

        public void ResetAR(string algorithPath, InsightARCamera arCamera)
        {
            m_ARCamera = arCamera.ARCamera;
            ResetAR(algorithPath);
        }



        public void Start()
        {

            //string algorithPath = GameSceneData.Instance.GetCurrentAlgPath();

            // InitAR(algorithPath);

            //InsightARInterface.recognizeAction += RecognitionBodyCallback;

        }

        public override void InitAR(string configDir = "")
        {
            SetConfig(configDir, InsightConst.APPKEY, InsightConst.APPSECRET);
            base.InitAR(configDir);
        }


        public override void ResetAR()
        {
            base.ResetAR();
        }

        public override void StopAR()
        {
            base.StopAR();
            InsightDebug.Log(TAG, " StopAR");
            DestroyImmediate(gameObject);
        }
    }
}