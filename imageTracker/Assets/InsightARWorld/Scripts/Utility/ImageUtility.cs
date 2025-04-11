using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class ImageUtility
{
    /// <summary>
    /// image to bytes
    /// </summary>
    /// <param name="imageFileName"></param>
    /// <returns></returns>
    public static byte[] ImageToBytes(string imageFileName)
    {
        return File.ReadAllBytes(imageFileName);
    }

    /// <summary>
    /// image to base64
    /// </summary>
    /// <param name="imageFileName"></param>
    /// <returns></returns>
    public static string ImageToBase64(string imageFileName)
    {
        byte[] bytes = ImageToBytes(imageFileName);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// base64 to image
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="filePath"></param>
    public static void Base64ToImage(string base64,string filePath)
    {
        byte[] buffer = Convert.FromBase64String(base64);
        File.WriteAllBytes(filePath, buffer);
    }
}
