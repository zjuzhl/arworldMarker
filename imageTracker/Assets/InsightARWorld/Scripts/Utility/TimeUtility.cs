using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static  class TimeUtility
{
    /// <summary>
    /// 返回时间字符串
    /// </summary>
    /// <returns></returns>
    public static string GetTimeString()
    {
        return System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") ;
    }

    /// <summary>
    /// 返回当前时刻字符串 
    /// </summary>
    /// <returns>The time string.</returns>
    /// <param name="prefix">Prefix.</param>
    public static string GetTimeString(string prefix, string suffix)
    {
        return prefix + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + suffix;
    }

    /// <summary>
    /// 返回时间戳
    /// </summary>
    /// <returns>The time stamp.</returns>
    public  static long  GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);  
        return Convert.ToInt64(ts.TotalSeconds);  
    }

    /// <summary>
    /// 返回毫秒时间戳 
    /// </summary>
    /// <returns>The time stamp milli.</returns>
    public static long GetTimeStampMilli()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return  (long)(DateTime.UtcNow - epochStart).TotalMilliseconds;
    }

    /// <summary>
    /// 返回纳秒
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStampNano()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(DateTime.UtcNow - epochStart).Ticks * 100;
    }

    /// <summary>
    /// long s to datetime
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeStampSecondsToDateTime(long unixTimeStamp)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }

    public static DateTime UnixTimeStampMilliSecondsToDateTime(long unixTimeStamp)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
        return dtDateTime;
    }

    /// <summary>
    ///  返回时间戳字符串
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static string GetStringFromUnixTimeStampSeconds(long unixTimeStampMilliSeconds)
    {
        DateTime dtDateTime = UnixTimeStampMilliSecondsToDateTime(unixTimeStampMilliSeconds);
        return dtDateTime.ToString("yyyy-MM-dd");
    }
}
