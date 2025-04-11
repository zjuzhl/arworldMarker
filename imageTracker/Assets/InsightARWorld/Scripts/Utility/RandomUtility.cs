using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class RandomUtility
{
    /// <summary>
    /// 根据种子，生成随机数，保证各端一致
    /// /* This algorithm is mentioned in the ISO C standard, here extended for 32bits */
    /// </summary>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static int Random(uint seed)
    {
        uint next = seed;
        int result;

        next *= 1103515245;
        next += 12345;
        result =(int)( (uint) (next / 65536) % 2048);

        next *= 1103515245;
        next += 12345;
        result <<= 10;
        result ^=(int)( (uint) (next / 65536) % 1024);

        next *= 1103515245;
        next += 12345;
        result <<= 10;
        result ^=(int)( (uint) (next / 65536) % 1024);

        seed = next;

        return result;
    }

    /// <summary>
    /// guid 随机数
    /// </summary>
    /// <returns></returns>
    public static string Random()
    {
        return System.Guid.NewGuid().ToString().Substring(0,8);
    }
}
