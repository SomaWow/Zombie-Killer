using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleStorer : MonoBehaviour {

    private static Dictionary<string, AssetBundle> dict = new Dictionary<string, AssetBundle>();

    public static void Add(string name, AssetBundle bundle)
    {
        if(!dict.ContainsKey(name))
        {
            dict[name] = bundle;
        }
    }

    public static AssetBundle Get(string name)
    {
        if(dict.ContainsKey(name))
        {
            return dict[name];
        }
        return null;
    }

    public static void ReleaseForced(string name)
    {
        if (dict.ContainsKey(name))
        {
            AssetBundle bundle = dict[name];
            dict.Remove(name);
            bundle.Unload(true);
        }
    }
}
