using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


    public static class PathUtility 
    {
        public static string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetFileNameWithoutExtenstionFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }

