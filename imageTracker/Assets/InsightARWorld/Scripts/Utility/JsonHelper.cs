using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonHelper  {
    public static T[] GetJsonArray<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class JsonWrapper<T>
    {
        public T[] array;
    }
}
