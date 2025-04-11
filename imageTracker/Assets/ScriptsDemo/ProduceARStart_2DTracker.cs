using InsightAR.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceARStart_2DTracker : MonoBehaviour
{
    public GameObject target = null;
    public GameObject succeed = null;

    void Start()
    {
        ProduceStart();

        //启动AR
        //1.设置算法文件的路径
        //2.启动AR场景

        string configpath = Application.streamingAssetsPath + "/ezxr/config_2d_i";
#if !UNITY_EDITOR
#if UNITY_ANDROID
        configpath = InsightARUtility.CopyAssetsToApplicationDataDir("ezxr") + "/config_2d_a";
#endif
#endif
        LSGameManager.Instance.InitConfigPath(configpath);
        LSGameManager.Instance.InitARScene();
    }

    private void ProduceStart()
    {
        target.SetActive(false);
        succeed.SetActive(false);
    }

    private void CloseSucceedUI()
    {
        succeed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Uninitialized = 0,
        //Initing = 1,
        //Init_OK = 2,
        //Init_Fail = 3,
        //Detecting = 4,
        //Detect_OK = 5,
        //Detect_Fail = 6,
        //Tracking = 7,
        //Track_Limited = 8,
        //Track_Lost = 9,
        //Track_Fail = 10,
        //Track_Stop = 11,
        if (InsightTracking.status == 7)
        {
            //isValid： 1跟踪中，0可能丢失，使用时需要判定该参数
            //identifier： merker ID
            var markers = InsightARManager.Instance.GetARInterface().GetMarkerList();
            if (markers.Count > 0)
            {
                foreach (var m in markers)
                {
                    Debug.Log("ezxr markers result is: " + m.isValid + ", " + m.identifier);
                }
            }
            if (target.activeSelf == false)
            {
                target.SetActive(true);
                succeed.SetActive(true);
                Invoke(nameof(CloseSucceedUI), 3);
            }
        }
    }
}
