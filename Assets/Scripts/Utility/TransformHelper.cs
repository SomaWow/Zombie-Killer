using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    /// <summary>
    /// 未知层级查找后代物体
    /// </summary>
    /// <param name="currentTF"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindChildByName(this Transform currentTF, string childName)
    {
        Transform childTF = currentTF.Find(childName);
        if (childTF != null) return  childTF;
        for (int i = 0; i < currentTF.childCount; i++)
        {
            childTF = FindChildByName(currentTF.GetChild(i), childName);
            if (childTF != null) return childTF;
        }
        return null;
    }
}