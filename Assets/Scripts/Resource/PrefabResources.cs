using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class PrefabResources : MonoBehaviour
    {
        private static Dictionary<string, Transform> prefabDict = new Dictionary<string, Transform>();
        private static string resDir = "Prefabs/";
        public static Action<string> loadEndDelegate;

        public static Transform Get(string path)
        {
            Load(path);
            if (prefabDict.ContainsKey(path))
                return prefabDict[path];
            return null;
        }

        private static void Load(string path)
        {
            if (!prefabDict.ContainsKey(path))
            {
                Transform transform = null;
                transform = Resources.Load(resDir + path, typeof(Transform)) as Transform;
                if (transform != null)
                {
                    prefabDict.Add(path, transform);
                    LoadEnd(path);
                }
                else
                {
                    Debug.LogError("No Prefab Exist : " + path);
                }
            }
        }
        private static void LoadEnd(string path)
        {
            if(loadEndDelegate != null)
            {
                loadEndDelegate(path);
            }
        }

        public static void Unload(string path)
        {
            if(prefabDict.ContainsKey(path))
            {
                Transform assetToUnload = prefabDict[path];
                prefabDict.Remove(path);
                Resources.UnloadAsset(assetToUnload);
            }
        }
    }

}