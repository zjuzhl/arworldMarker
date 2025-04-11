using InsightAR.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceARStart : MonoBehaviour
{
    public GameObject target = null;
    public GameObject succeed = null;

    [SerializeField]
    public string cloudLocUrl = string.Empty;
    void Start()
    {
        ProduceStart();


        //启动AR
        //1.设置定位算法url
        //2.设置算法文件的路径
        //3.启动AR场景

        if (string.IsNullOrEmpty(cloudLocUrl)) 
        {
            Debug.Log("cloudLocUrl is empty or null!");
            return;
        }

        LSCloudLocationRequet.cloudUrl = cloudLocUrl;

        string configpath = Application.streamingAssetsPath + "/ezxr/config";
#if !UNITY_EDITOR
#if UNITY_ANDROID
        configpath = InsightARUtility.CopyAssetsToApplicationDataDir("ezxr") + "/config";
#endif
#endif
        LSGameManager.Instance.InitConfigPath(configpath);
        LSGameManager.Instance.InitARScene();
    }

    private void ProduceStart()
    {
        target.SetActive(false);
        succeed.SetActive(false);

        // clooud location success
        LSCloudLocationRequet.lsCloudLocSuccess = null;
        LSCloudLocationRequet.lsCloudLocSuccess += (res) =>
        {
            if (InsightTracking.cloudLocationSuccessCount == 1)
            {
                target.SetActive(true);
                succeed.SetActive(true);
                Invoke(nameof(CloseSucceedUI), 3);
            }
        };
        // clooud location fail
        LSCloudLocationRequet.lsCloudLocFail = null;
        LSCloudLocationRequet.lsCloudLocFail += (res) =>
        {
            Debug.Log("cloud location error, code: " + res.code + ", msg: " + res.msg);
        };
        // clooud location error
        LSCloudLocationRequet.lsCloudLocError = null;
        LSCloudLocationRequet.lsCloudLocError += (res) =>
        {
            Debug.Log("cloud location error, code: " + res.code + ", serverCode: " + res.serverCode + ", msg: " + res.msg);
        };
    }

    private void CloseSucceedUI()
    {
        succeed.SetActive(false);
    }
}
