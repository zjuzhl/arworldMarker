using UnityEngine;
using UnityEditor;
using InsightAR.Internal;

namespace InsightAR.Internal
{
    public class InsightARRecognizedResult
    {
        //[obsolete]
        public bool isCloudMode;
        public InsightARClassifiedType type;
        public string recognizedResult;
    }
}