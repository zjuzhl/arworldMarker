using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;

public static class EncodeUtility
{
    /// <summary>
    /// 返回md5字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(string strToEncrypt)
    {
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    /// <summary>
    /// sha256
    /// </summary>
    /// <param name="pass"></param>
    /// <returns></returns>
    public static string Sha256(string pass)
    {
        if (pass == null || pass == string.Empty) { return null; }
        byte[] buffer = Encoding.UTF8.GetBytes(pass);

        byte[] hash = SHA256Managed.Create().ComputeHash(buffer);
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            builder.Append(hash[i].ToString("X2"));
        }
        return builder.ToString();
    }
}
