using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InsightConst
{
#if UNITY_IOS
    public static string APPKEY = "AR2-f9411a536aeb7486eca274cc57b5e3bd";
    public static string APPSECRET = "92d1e63fe5c3c2e7c6cf3b97181b25f0";
#else
    public static string APPKEY = "AR2-44e07e3d0c3930a8102cc0dc5d6a7481";
    public static string APPSECRET = "0d33175962b7e6a41d9938742d6b0421";
#endif

    public static string ConfigPath = "";
}
