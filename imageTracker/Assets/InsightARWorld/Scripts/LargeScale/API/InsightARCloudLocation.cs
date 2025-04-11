using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InsightAR.Internal;
using System;
using System.Runtime.InteropServices;
public enum CloudRequestType
{
    Default = 0,
    Once = 1,
    Always = 2,
}

/// <summary>
/// 云端重定位算法
/// </summary>
public class InsightARCloudLocation
{
    #region
    private const string TAG = "InsightARCloudLocation";
    public int CloudLocAlgCode = 16;//初始值未定位失败，1表示成功
    public string CloudLocAlgReason = "";
    public long CloudLocTotalCount = 0;
    public long CloudSuccessCount = 0;
    public CloudRequestType requestType = CloudRequestType.Always;

    private LSCloudLocationRequet locationRequet = null;
    //主动请求定位
    public bool onceActiveRequestCloudLocationChecking = false;
    #endregion

    #region custon functions

    /// <summary>
    /// 添加重定位监听
    /// </summary>
    public void AddListener()
    {
        InsightARManager.Instance.GetARInterface().cloudLocAction += OnCloudLocNativeRequestHandler;

        CloudLocTotalCount = 0;
        CloudLocAlgReason = "";
        CloudLocAlgCode = 0;
        CloudSuccessCount = 0;

        locationRequet = GameObject.Find("LSGameManager").GetComponent<LSCloudLocationRequet>();
    }

    /// <summary>
    /// 移除重定位监听
    /// </summary>
    public void RemoverListener()
    {
        InsightARManager.Instance.GetARInterface().cloudLocAction -= OnCloudLocNativeRequestHandler;

        CloudLocTotalCount = 0;
        CloudLocAlgReason = "";
        CloudLocAlgCode = 0;
        CloudSuccessCount = 0;
    }

    /// <summary>
    /// 发起云定位请求
    /// </summary>
    /// <param name="requestData"></param>
    private void OnCloudLocNativeRequestHandler(InsightARCloudLocRequestImpl requestData)
    {
        if (requestType == CloudRequestType.Once && CloudLocTotalCount >= 1)
        {
            return;
        }
        CloudLocTotalCount++;
        long timestamp = TimeUtility.GetTimeStampMilli();
        string imageBase64 = requestData.jpgStr;
        string protoBase64 = requestData.requestInfoStr;
        //InsightAPPNative.CloudLocationRequest(timestamp, imageBase64, protoBase64, ContentResPaths.Instance.CloudURL);
        locationRequet?.StartCloudLocation(timestamp, imageBase64, protoBase64);
    }

    /// <summary>
    /// 处理云定位回调
    /// </summary>
    /// <param name="resultData"></param>
    public void OnCloudLocNativeCallbackHandler(InsightCloudRequestResult resultData)
    {
        if (onceActiveRequestCloudLocationChecking)
        {
            onceActiveRequestCloudLocationChecking = false;
        }
        InsightDebug.Log(TAG, resultData.algCode.ToString());
        if (!string.IsNullOrEmpty(resultData.algResult)) {
            CloudLocAlgReason = "";
            CloudLocResult(resultData.algResult);
            CloudLocAlgCode = resultData.algCode;
            //云定位成功计数
            if (CloudLocAlgCode == 1)
                CloudSuccessCount++;
        }
    }

    /// <summary>
    /// 处理云端重定位服务器返回的结果
    /// </summary>
    /// <param name="requestData"></param>
    /// <param name="protoData"></param>
    private void CloudLocResult(string protoData)
    {
        InsightARCloudLocResult insightARCloudLocResult = new InsightARCloudLocResult();
        InsightARCloudLocResultMeta insightARCloudLocResultMeta = new InsightARCloudLocResultMeta();
        insightARCloudLocResultMeta.timestamp = 0.0;
        insightARCloudLocResultMeta.status = 0;
        insightARCloudLocResult.meta = insightARCloudLocResultMeta;
        try {
            //base64 解码
            byte[] buffer = Convert.FromBase64String(protoData);
            int length = buffer.Length;
            IntPtr resultPtr = Marshal.AllocHGlobal(length);
            Marshal.Copy(buffer, 0, resultPtr, length);
            insightARCloudLocResult.resultInfoPtr = resultPtr;
            insightARCloudLocResult.resultLength = length;

            //同步调用
            InsightARNative.iarlsOnCloudLocalizedNative(insightARCloudLocResult);

            //供底层算法调用之后，释放指针
            Marshal.FreeHGlobal(resultPtr);
        }
        catch (FormatException exp) {
            InsightDebug.Log(TAG, "Format Error: " + exp);
        }
       
    }

    public void ResetCloudLocation()
    {
        CloudLocTotalCount = 0;
        CloudLocAlgReason = "";
        CloudLocAlgCode = 0;
        CloudSuccessCount = 0;
    }

    #endregion
}
