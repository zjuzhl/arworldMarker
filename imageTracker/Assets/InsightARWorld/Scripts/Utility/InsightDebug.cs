using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum LogLevel
{
    IUserEventTypeLogLevelVerbose = 400,
    IUserEventTypeLogLevelDebug = 401,
    IUserEventTypeLogLevelWarn = 402,
    IUserEventTypeLogLevelError = 403,
    IUserEventTypeLogLevelNone = 404,
}

/// <summary>
/// 处理日志打印
/// </summary>
public static class InsightDebug
{
    #region params
    private const string UNITY = "Unity";
 #if DEBUG_TEST
    private static bool sEnableDebug = true;
#else
    private static bool sEnableDebug = true;
#endif
    public static LogLevel sEnableLogLevel = LogLevel.IUserEventTypeLogLevelVerbose;

    #endregion

    #region custom functions
    public static void Log(string tag, string content)
    {
        if (sEnableLogLevel == LogLevel.IUserEventTypeLogLevelVerbose || sEnableLogLevel == LogLevel.IUserEventTypeLogLevelDebug)
        {
            Debug.Log(UNITY + "_" + tag + "_" + content);
        }
    }

    public static void LogWaring(string tag, string content)
    {
        if (sEnableLogLevel == LogLevel.IUserEventTypeLogLevelVerbose || sEnableLogLevel == LogLevel.IUserEventTypeLogLevelWarn)
        {
            Debug.LogWarning(UNITY + "_" + tag + "_" + content);
        }
    }

    public static void LogError(string tag, string content)
    {
        if (sEnableLogLevel == LogLevel.IUserEventTypeLogLevelVerbose || sEnableLogLevel == LogLevel.IUserEventTypeLogLevelError)
        {
            Debug.LogError(UNITY + "_" + tag + "_" + content);
        }
    }

    public static void LogAssertion(string tag, string content)
    {
        if (sEnableDebug)
        {
            Debug.LogAssertion(UNITY + "_" + tag + "_" + content);
        }
    }
#endregion
}

