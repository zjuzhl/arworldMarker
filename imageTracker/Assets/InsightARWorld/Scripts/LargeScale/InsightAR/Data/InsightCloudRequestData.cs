using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InsightAR.Internal {
    /// <summary>
    /// cloud request model
    /// </summary>
    [Serializable]
    public class InsightCloudRequestData
    {
        public CloudData alg;

        public InsightCloudRequestData()
        {
            alg = new CloudData();
        }
    }

    [Serializable]
    public class CloudData
    {
        public string imageEncodingData;
        public string protobufEncodingData;
    }

}
