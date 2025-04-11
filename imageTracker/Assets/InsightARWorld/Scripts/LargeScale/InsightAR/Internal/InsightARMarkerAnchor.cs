using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsightAR.Internal
{
    /// <summary>
    /// Insight AR plane anchor For Unity
    /// </summary>
	public struct InsightARMarkerAnchor
    {
        public string identifier;
        // public InsightARPlaneAnchorAlignment alignment;     // maybe we can replace it with 2d-marker's directions.
        public Vector3 center;
        public Vector3 extent;
        public Quaternion rotation;
        public int isValid;
    }
}
