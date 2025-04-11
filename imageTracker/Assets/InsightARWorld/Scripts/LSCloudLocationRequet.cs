using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InsightAR.Internal;
using UnityEngine.Networking;
using System;

public class LSCloudLocationRequet : MonoBehaviour
{
    public class CloudLocResult {
        public int code;
        public string serverCode;
        public string otherCode;
        public string msg;
    }
    public static Action<CloudLocResult> lsCloudLocSuccess;
    public static Action<CloudLocResult> lsCloudLocFail;
    public static Action<CloudLocResult> lsCloudLocError;
    public static string cloudUrl = "https://yx-tc1-parkc.easexr.com/api/alg/cloud/aw/reloc?map=c6";

    /// <summary>
    /// 退出场景时触发,释放所有委托
    /// </summary>
    private void OnDisable()
    {
        lsCloudLocSuccess = null;
        lsCloudLocFail = null;
        lsCloudLocError = null;
    }

    public void StartCloudLocation(long timestamp, string imageBase64, string protoBase64) 
    {
        Debug.Log("cloud url: " + cloudUrl.Replace("https://", ""));
        StartCoroutine(cloudLocationRequest(timestamp, imageBase64, protoBase64, cloudUrl));
    }

    private IEnumerator cloudLocationRequest(long timestamp, string imageBase64, string protoBase64, string cloudUrl) {

        InsightCloudRequestData data = new InsightCloudRequestData();
        data.alg.imageEncodingData = imageBase64;
        data.alg.protobufEncodingData = protoBase64;

        using (UnityWebRequest request = new UnityWebRequest(cloudUrl, "POST")) {
            byte[] postBytes = System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            //Debug.Log("Status Code: " + request.responseCode);
            if (request.isNetworkError || request.isHttpError)
            {
                lsCloudLocError?.Invoke(new CloudLocResult
                {
                    code = -100,
                    serverCode = "-100",
                    msg = request.error,
                });
                Debug.Log(request.error);
            }
            else
            {
                if (request.isDone && request.responseCode == 200) {
                    var result = request.downloadHandler.text;
                    InsightCloudRequestResponse requestResponse = JsonUtility.FromJson<InsightCloudRequestResponse>(result);
                    if (requestResponse.code == "00000000")
                    {
                        //Debug.Log("algCode: " + requestResponse.msg);
                        //Debug.Log("algCode: " + requestResponse.result);
                        //Debug.Log("algCode: " + requestResponse.result.algCode);
                        //Debug.Log("algCode: " + requestResponse.result.algEncodingData);
                        //Debug.Log("algCode: " + requestResponse.result.algEncodingType);
                        //Debug.Log("algCode: " + requestResponse.result.protobufEncodingData); 
                        InsightCloudRequestResult requestResult = new InsightCloudRequestResult
                        {
                            algCode = requestResponse.result.algCode,
                            algResult = requestResponse.result.protobufEncodingData
                        };
                        InsightARManager.Instance.GetARCloudLocation().OnCloudLocNativeCallbackHandler(requestResult);
                        CloudLocResult locResult = new CloudLocResult
                        {
                            code = requestResult.algCode,
                            msg = requestResponse.msg
                        };
                        if (requestResult.algCode == 1)
                        {
                            lsCloudLocSuccess?.Invoke(locResult);
                        }
                        else
                        {
                            lsCloudLocFail?.Invoke(locResult);
                        }
                    }
                    else {
                        lsCloudLocError?.Invoke(new CloudLocResult
                        {
                            serverCode = requestResponse.code,
                            msg = requestResponse.msg
                        });
                        Debug.Log("cloud server error: " + result);
                    }
                }
                
            }
        }
        yield return null;
    }
}
