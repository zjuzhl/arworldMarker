using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InsightAR.Internal {
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDebugFrame
    {
        /*******************************************************************************
                                       IMAGE
         *******************************************************************************/
        [MarshalAs(UnmanagedType.U1)]
        public bool imageValid;             // 是否需要保存
        public InsightARDebugFrameImage image;
        /*******************************************************************************
                                       LOC_TRAJ
         *******************************************************************************/
        [MarshalAs(UnmanagedType.U1)]
        public bool locValid;            // 是否需要保存, 如果云定位失败，则Valid = false。
        public InsightARDebugFrameCamInfo loc_traj_info;
        /*******************************************************************************
                                       VIO_TRAJ
         *******************************************************************************/
        [MarshalAs(UnmanagedType.U1)]
        public bool vioValid;            // 是否需要保存
        public InsightARDebugFrameCamInfo vioInfo;
        /*******************************************************************************
                                       LOC_VIO_TRAJ
         *******************************************************************************/
        [MarshalAs(UnmanagedType.U1)]
        public bool locVioValid;         // 是否需要保存
        public InsightARDebugFrameCamInfo loc_vio_info;
        /*******************************************************************************
                                    InsightARDeviceInfo
       *******************************************************************************/
        public InsightARDeviceInfo deviceInfo;
        /*******************************************************************************
                                      InsightARDeviceVIOInfo
         *******************************************************************************/
        public InsightARDeviceVIOInfo deviceVIOInfo;
        /*******************************************************************************
                                       InsightAREZXRVioInfo
         *******************************************************************************/
        public InsightAREZXRVioInfo ezxrVIOInfo;            // iOS暂时不需要处理这个


        /*******************************************************************************
                                       TO BE CONTINUED
         *******************************************************************************/
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDebugFrameImage
    {
        public IntPtr imagePtr;             //jpeg stream buffer
        public int imageByteLength;   //jpeg stream buffer length
        public InsightARDebugFrameImageMeta meta;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDebugFrameImageMeta
    {
        public double imageTimeStamp;      // timestamp
        public float fx;                  // fx
        public float fy;                  // fy
        public float cx;                  // cx
        public float cy;                  // cy
        public int imageWidth;          // width
        public int imageHeight;         // height
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDebugFrameCamInfo
    {
        public int state;            // 2: detecting; 3: tracking; 4: trackingLimited
        public double timeStamp;        // timestamp
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] position;      // x, y, z
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] rotation;      // qx, qy, qz, qw
        public double stateTimeStamp; //根据data求解的位姿的时间戳
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDeviceInfo
    {
        public int device_type;             // 0: iPhone;             1: Android;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string model;          // iPhone: "iPhone X";    Android: android.os.Build.MODEL
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string manufacturer;   // iPhone: empty;         Android: android.os.Build.MANUFACTURER
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string brand;          // iPhone: empty;         Android: android.os.Build.BRAND
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string device;         // iPhone: empty;         Android: android.os.Build.DEVICE
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string product;        // iPhone: empty;         Android: android.os.Build.PRODUCT
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightARDeviceVIOInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string vio_name;              // "ARKIT",  "ARCORE",  "HWAREngine",  "ISNIGHTMSCKF"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string modelName;             // iPhone: "iPhone X";    Android: android.os.Build.MODEL
        public int width;                      // SDK传递给VIO算法的图像的宽
        public int height;                     // SDK传递给VIO算法的图像的高
        public float fx;
        public float fy;
        public float cx;
        public float cy;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct InsightAREZXRVioInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string calib_version;           // 标定文件的版本号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string alg_version;             // 所运行的VIO算法的版本号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string request_device_name;     // SDK传递给VIO算法的model name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string alg_parsed_device_name;  // VIO算法实际执行的参数model name
        public int width;                        // AREngine传递给VIO算法的图像的宽
        public int height;                       // AREngine传递给VIO算法的图像的高
    }

    public class InsightARDebugFrameLoc
    {
        public bool imageValid;             // 是否需要保存
        public InsightARDebugFrameImageLoc image;
        public bool locValid;            // 是否需要保存, 如果云定位失败，则Valid = false。
        public InsightARDebugFrameCamInfoLoc loc_traj_info;
        public bool vioValid;            // 是否需要保存
        public InsightARDebugFrameCamInfoLoc vioInfo;
        public bool locVioValid;         // 是否需要保存
        public InsightARDebugFrameCamInfoLoc loc_vio_info;
        public InsightARDeviceInfoLoc deviceInfo;
        public InsightARDeviceVIOInfoLoc deviceVIOInfo;
        public InsightAREZXRVioInfoLoc ezxrVIOInfo;

        public InsightARDebugFrameLoc(InsightARDebugFrame insightARDebugFrame, bool isFirstRecord = false)
        {
            this.imageValid = insightARDebugFrame.imageValid;
            this.locValid = insightARDebugFrame.locValid;
            this.vioValid = insightARDebugFrame.vioValid;
            this.locVioValid = insightARDebugFrame.locVioValid;


            if (this.imageValid)
            {
                this.image = new InsightARDebugFrameImageLoc(insightARDebugFrame.image);
            }
            if (this.locValid)
            {
                this.loc_traj_info = new InsightARDebugFrameCamInfoLoc(insightARDebugFrame.loc_traj_info);
            }
            if (this.vioValid)
            {
                this.vioInfo = new InsightARDebugFrameCamInfoLoc(insightARDebugFrame.vioInfo);
            }
            if (this.locVioValid)
            {
                this.loc_vio_info = new InsightARDebugFrameCamInfoLoc(insightARDebugFrame.loc_vio_info);
            }
            if (isFirstRecord)
            {
                this.deviceInfo = new InsightARDeviceInfoLoc(insightARDebugFrame.deviceInfo);
                this.deviceVIOInfo = new InsightARDeviceVIOInfoLoc(insightARDebugFrame.deviceVIOInfo);
                this.ezxrVIOInfo = new InsightAREZXRVioInfoLoc(insightARDebugFrame.ezxrVIOInfo);
            }
        }
    }
    public class InsightARDebugFrameImageLoc
    {
        public byte[] imagePtr;             //jpeg stream buffer
        public int imageByteLength;   //jpeg stream buffer length
        public InsightARDebugFrameImageMetaLoc meta;
        public InsightARDebugFrameImageLoc(InsightARDebugFrameImage insightARDebugFrame)
        {
            this.meta = new InsightARDebugFrameImageMetaLoc(insightARDebugFrame.meta);
            this.imageByteLength = insightARDebugFrame.imageByteLength;
            this.imagePtr = new byte[this.imageByteLength];
            Marshal.Copy(insightARDebugFrame.imagePtr, this.imagePtr, 0, this.imageByteLength);
        }
    }
    public class InsightARDebugFrameImageMetaLoc
    {
        //截取小数点后三位，不进行四舍五入
        public string imageTimeStamp;      // timestamp

        public float fx;                  // fx
        public float fy;                  // fy
        public float cx;                  // cx
        public float cy;                  // cy
        public int imageWidth;          // width
        public int imageHeight;         // height
        public InsightARDebugFrameImageMetaLoc(InsightARDebugFrameImageMeta meta)
        {
            var ts = Math.Truncate(meta.imageTimeStamp * 1000) / 1000;
            this.imageTimeStamp = ts.ToString("f3");
            this.fx = meta.fx;
            this.fy = meta.fy;
            this.cx = meta.cx;
            this.cy = meta.cy;
            this.imageWidth = meta.imageWidth;
            this.imageHeight = meta.imageHeight;
        }
    }
    public class InsightARDebugFrameCamInfoLoc
    {
        public int state;            // 2: detecting; 3: tracking; 4: trackingLimited

        //截取小数点后三位，不进行四舍五入
        public string timeStamp;        // timestamp
        public float[] position;      // x, y, z
        public float[] rotation;      // qx, qy, qz, qw
        public string stateTimeStamp; // 根据data求解的位姿的时间戳
        public InsightARDebugFrameCamInfoLoc(InsightARDebugFrameCamInfo info)
        {
            this.state = info.state;
            var ts = Math.Truncate(info.timeStamp * 1000) / 1000;
            this.timeStamp = ts.ToString("f3");



            var stateTs = Math.Truncate(info.timeStamp * 1000) / 1000;
            this.stateTimeStamp = stateTs.ToString("f3");

            this.position = new float[] { info.position[0], info.position[1], info.position[2] };
            this.rotation = new float[] { info.rotation[0], info.rotation[1], info.rotation[2], info.rotation[3] };
        }
    }

    public class DebugFrameCommonData
    {
        public string name;
        public string iosPath;//ios端 document以后目录 如：xx\document\xxx\xxx.txt => xxx\xxx.txt
        public string path;//完整地址
        public string uid;//cid+time
        public long startTime;
        public long endTime;
    }
    public class CommonCallbackData
    {
        public int type;
        public CommonResultData result;
    }
    public class CommonResultData
    {
        public int state;
        public string msg;
        public double progress;
    }

    public class InsightARDeviceVIOInfoLoc
    {
        public string vio_name;              // "ARKIT",  "ARCORE",  "HWAREngine",  "ISNIGHTMSCKF"
        public string modelName;             // iPhone: "iPhone X";    Android: android.os.Build.MODEL
        public int width;                      // SDK传递给VIO算法的图像的宽
        public int height;                     // SDK传递给VIO算法的图像的高
        public float fx;
        public float fy;
        public float cx;
        public float cy;
        public InsightARDeviceVIOInfoLoc(InsightARDeviceVIOInfo info)
        {
            this.vio_name = info.vio_name;
            this.modelName = info.modelName;
            this.width = info.width;
            this.height = info.height;
            this.fx = info.fx;
            this.fy = info.fy;
            this.cx = info.cx;
            this.cy = info.cy;
        }
    }
    public class InsightARDeviceInfoLoc
    {
        public int device_type;             // 0: iPhone;             1: Android;
        public string model;          // iPhone: "iPhone X";    Android: android.os.Build.MODEL
        public string manufacturer;   // iPhone: empty;         Android: android.os.Build.MANUFACTURER
        public string brand;          // iPhone: empty;         Android: android.os.Build.BRAND
        public string device;         // iPhone: empty;         Android: android.os.Build.DEVICE
        public string product;        // iPhone: empty;         Android: android.os.Build.PRODUCT
        public InsightARDeviceInfoLoc(InsightARDeviceInfo info)
        {
            this.device_type = info.device_type;
            this.model = info.model;
            this.manufacturer = info.manufacturer;
            this.brand = info.brand;
            this.device = info.device;
            this.product = info.product;
        }
    }
    public class InsightAREZXRVioInfoLoc
    {
        public string calib_version;           // 标定文件的版本号
        public string alg_version;             // 所运行的VIO算法的版本号
        public string request_device_name;     // SDK传递给VIO算法的model name
        public string alg_parsed_device_name;  // VIO算法实际执行的参数model name
        public int width;                        // AREngine传递给VIO算法的图像的宽
        public int height;                       // AREngine传递给VIO算法的图像的高
        public InsightAREZXRVioInfoLoc(InsightAREZXRVioInfo info)
        {
            this.calib_version = info.calib_version;
            this.alg_version = info.alg_version;
            this.request_device_name = info.request_device_name;
            this.alg_parsed_device_name = info.alg_parsed_device_name;
            this.width = info.width;
            this.height = info.height;
        }
    }
}
