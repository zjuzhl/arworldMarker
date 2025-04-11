using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility 
{
    /// <summary>
    /// 朝向相机，不弯腰
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="camera"></param>
    /// <param name="y"></param>
    public static void LookAtXZPlane(Transform trans, Camera camera,bool useTargetHeight = true)
    {
        Vector3 worldPosition = camera.transform.position;
        if (useTargetHeight)
        {
            worldPosition.y = trans.position.y;
        }
        trans.LookAt(worldPosition);
    }

    /// <summary>
    /// 克隆物体
    /// </summary>
    /// <param name="cloneTrans"></param>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static Transform Clone(Transform prefabTrans, Transform parent, string layer)
    {
        if (prefabTrans == null) return null;
        GameObject prefab = GameObject.Instantiate(prefabTrans.gameObject);
        prefab.name = prefab.name.Replace("(Clone)", "");
        Transform trans = prefab.transform;
        trans.SetParent(parent);
        trans.position = Vector3.zero;
        trans.rotation = Quaternion.identity;
        trans.localScale = Vector3.one;
        return trans;
    }

}
