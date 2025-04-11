using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InsightAR.Internal;
using UnityEngine.Android;

public class LSGameManager : MonoBehaviour
{
    #region params
    private const string TAG = "LSGameManager";
    private static bool sGameInited = false;
    private static LSGameManager _instance;
    public static LSGameManager Instance{
        get{return _instance;}
    }
    #endregion

    #region unity functions
    private void Awake()
    {
        if (_instance == null){
            _instance = this;
        }else{
            GameObject.Destroy(gameObject);
        }
        InsightDebug.Log(TAG, " On Awake");
    }

    private void OnEnable()
    {
        InsightDebug.Log(TAG, "OnEnable");
#if UNITY_ANDROID
        bool cameraPermissionGranted = Permission.HasUserAuthorizedPermission(Permission.Camera);
        if (!cameraPermissionGranted)
        {
            Permission.RequestUserPermission(Permission.Camera);
            return;
        }
        bool storagePermissionGranted = Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
        if (!storagePermissionGranted)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            return;
        }
#endif
        //InitARScene();
    }

    private void OnDisable() {
        InsightDebug.Log(TAG, " On Disable");
        ExitARScene();
    }

    private void Update()
    {
        //InsightDebug.Log(TAG, Tracking.status.ToString()) ;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause) {
#if UNITY_ANDROID
            bool cameraPermissionGranted = Permission.HasUserAuthorizedPermission(Permission.Camera);
            bool storagePermissionGranted = Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
            if (cameraPermissionGranted & storagePermissionGranted) {
                //InitARScene();
            } else if (!storagePermissionGranted) {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            } else if (!cameraPermissionGranted) {
                Permission.RequestUserPermission(Permission.Camera);
            }
#endif
        }
    }
    #endregion

    #region custom functions
    /// <summary>
    /// 注册算法key&secret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="secret"></param>
    public void InitARKeySecret(string key, string secret) {
        InsightConst.APPKEY = key;
        InsightConst.APPSECRET = secret;
    }

    public void InitConfigPath(string configpath) {
        InsightConst.ConfigPath = configpath;
    }
    /// <summary>
    /// 进入游戏
    /// </summary>
    public void InitARScene()
    {
        if (sGameInited) return;
        sGameInited = true;
        InsightDebug.Log(TAG, " InitARScene configpath = " + InsightConst.ConfigPath);
        if(!string.IsNullOrEmpty(InsightConst.ConfigPath))
            InsightARStart.Instance.StartARManager(InsightConst.ConfigPath);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitARScene()
    {
        if (!sGameInited) return;
        sGameInited = false;
        InsightDebug.Log(TAG, " ExitARScene");
        //停止AR
        InsightARManager.Instance.StopAR();

    }
#endregion
}
