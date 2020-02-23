using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager {

    private const string Dir = "Sound/";
    private static Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private static AssetBundle bundle;
    
    public static void AddClip(string name, AudioClip clip)
    {
        if(clips.ContainsKey(name))
        {
            clips[name] = clip;
        }
        else
        {
            clips.Add(name, clip);
        }
    }

    public static AudioClip GetClip(string name)
    {
        // 有则返回
        if (clips.ContainsKey(name))
            return clips[name];
        // 无则加载，从Asset资源中加载
        AudioClip clip = Resources.Load(Dir + name, typeof(AudioClip)) as AudioClip;
        if (clip != null)
        {
            clips.Add(name, clip);
            return clip;
        }
        // 从AssetBundle资源中加载
        if (bundle == null)
            return null;
        clip = bundle.LoadAsset(name, typeof(AudioClip)) as AudioClip;
        if(clip != null)
        {
            clips.Add(name, clip);
        }
        return clip;
    }
}
