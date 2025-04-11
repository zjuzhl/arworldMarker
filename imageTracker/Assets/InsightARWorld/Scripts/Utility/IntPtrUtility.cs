using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// intptr to structure
/// </summary>
public static class IntPtrUtility 
{
    /// <summary>
    /// 从intptr里面拷贝结构体数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayPtr"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T[] GetStructureList<T>(IntPtr arrayPtr, int length)
    {
        if (length == 0 || arrayPtr == IntPtr.Zero) return null;

        IntPtr ptr;
        T[] list = new T[length];
        ptr = arrayPtr;
        for (int i = 0; i < length; i++)
        {
            list[i] = Marshal.PtrToStructure<T>(ptr);
            ptr += Marshal.SizeOf(typeof(T));
        }
        Marshal.FreeHGlobal(arrayPtr);
        return list;
    }
}
