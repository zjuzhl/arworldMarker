using System;
using System.IO;

/// <summary>
/// url utility
/// </summary>
public class URLUtility
{
    public static string GetFileNameFromUrl(string url)
    {
        Uri uri = new Uri(url);
        return Path.GetFileName(uri.AbsolutePath);
    }

    public static string GetFileNameWithoutExtensionFromUrl(string url)
    {
        Uri uri = new Uri(url);
        return Path.GetFileNameWithoutExtension(uri.AbsolutePath);
    }

    /// <summary>
    /// 返回文件后缀名
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetFileExtensionFromUrl(string url)
    {
        string fileName = GetFileNameFromUrl(url);
        string[] strs = fileName.Split(new char[] { '.' });
        return strs.Length > 1 ? strs[strs.Length - 1] : string.Empty;
    }

    public static string[] GetSegmentsFromUrl(string url)
    {
        Uri uri = new Uri(url);
        return uri.Segments;
    }
}

