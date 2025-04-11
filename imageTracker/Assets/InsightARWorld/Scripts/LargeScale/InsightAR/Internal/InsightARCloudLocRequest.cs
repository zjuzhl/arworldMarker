using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace InsightAR.Internal
{
    public struct InsightARCloudLocRequestMeta
    {
        public double timestamp;           // the query jpg's timestamp

        public InsightARCloudLocRequestMeta(double time)
        {
            this.timestamp = time;
        }
    }

    public struct InsightARCloudLocRequest
    {
        public InsightARCloudLocRequestMeta meta;  // query struct

        public IntPtr jpgPtr;         // jpg ptr
        public int byteLength;             // jpg ptr byte length

        public IntPtr reqestInfoPtr;         // proto ptr 字节流
        public int reqestInfoLength;             // proto byte length
    }

    public struct InsightARCloudLocRequestImpl
    {
        public InsightARCloudLocRequestMeta meta;  // query struct

        public string jpgStr;         // jpg string
        public string requestInfoStr;

        public InsightARCloudLocRequestImpl(InsightARCloudLocRequest requestData)
        {
            if (requestData.byteLength == 0 || requestData.jpgPtr == IntPtr.Zero)
            {
                this.jpgStr = "";
            }
            else
            {
                byte[] imageBytes = new byte[requestData.byteLength];
                Marshal.Copy(requestData.jpgPtr, imageBytes, 0, requestData.byteLength);
                this.jpgStr = Convert.ToBase64String(imageBytes);
              //  Marshal.FreeHGlobal(requestData.jpgPtr); native自己会释放
            }

            if (requestData.reqestInfoLength == 0 || requestData.reqestInfoPtr == IntPtr.Zero)
            {
                this.requestInfoStr = "";
            }
            else
            {
                byte[] protoBytes = new byte[requestData.reqestInfoLength];
                Marshal.Copy(requestData.reqestInfoPtr, protoBytes, 0, requestData.reqestInfoLength);
                this.requestInfoStr = Convert.ToBase64String(protoBytes);
                //   Marshal.FreeHGlobal(requestData.reqestInfoPtr);// native自己会释放
            }
            this.meta = requestData.meta;
        }
    }



    public struct InsightARCloudLocResultMeta
    {
        public int status;
        public double timestamp;           // the query jpg's timestamp
    }

    public struct InsightARCloudLocResult
    {
        public InsightARCloudLocResultMeta meta; //result stuct

        public IntPtr resultInfoPtr;  //proto ptr;
        public int resultLength;      //proto byte length;
    }

    /// <summary>
    /// 云端重定位返回的数据状态
    /// </summary>
    public enum LOCSTATUS
    {
        SUCCESS = 0x01,   ///定位成功，返回pose等信息
        FAIL_UNKNOWN = 0x10,   ///定位失败，走完了定位算法流程，但是图像无法定位到给定地图中
        FAIL_MATCH = 0x11,   ///定位失败，具体原因1
        FAIL_INLIER = 0x12,   ///定位失败，具体原因2
        INVALID_DEVICEINFO = 0x20,  ///数据不合法，传入的protobuf.deviceInfo不符合规范
        INVALID_LOCALIZER = 0x21,   ///数据不合法，部署阶段的localizer未成功初始化
        INVALID_IMAGE = 0x22,       ///数据不合法，传入的图像或protobuf.deviceInfo中出现不被接受的图像格式（仅接收通道数为1或3,且类型为CV_8U或CV_8UC3的图像）
        INVALID_IMAGE_PROTO_MATCH = 0x23    ///数据不合法，传入的图像文件长度，与protobuf.deviceInfo中记录的图像字节数不匹配
    }

    public struct InsightCloudRequestResult 
    {
        public int algCode;
        public string algResult;
    }
    [System.Serializable]
    public struct InsightCloudRequestResponse
    {
        public string code;
        public string msg;
        public InsightCloudResultResponse result;
    }
    [System.Serializable]
    public struct InsightCloudResultResponse
    {
        public int algEncodingType;
        public string algEncodingData;
        public int algCode;
        public string protobufEncodingData;
    }
}
