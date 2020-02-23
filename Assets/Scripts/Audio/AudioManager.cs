using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager _INSTANCE;
    private AudioSource audioMgr;
    private AudioClip audioClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static AudioManager INSTANCE
    {
        get
        {
            if(_INSTANCE == null)
            {
                GameObject target = new GameObject("AudioManager");
                _INSTANCE = target.AddComponent<AudioManager>();
                _INSTANCE.audioMgr = target.AddComponent<AudioSource>();
                DontDestroyOnLoad(target);
            }
            return _INSTANCE;
        }
    }

    // 播放
    public AudioSource Play(string clipName)
    {
        return Play(clipName, Vector3.zero, 1f, 1f, false, false);
    }
    public AudioSource Play(string clipName, float volume)
    {
        return Play(clipName, Vector3.zero, volume, 1f, false, false);
    }
    public AudioSource Play(string clipName, Vector3 point, bool loop)
    {
        return Play(clipName, point, 1f, 1f, loop, false);
    }
    public AudioSource Play(string clipName, Vector3 point, float volume, bool loop)
    {
        return Play(clipName, point, volume, 1f, loop, false);
    }
    public AudioSource Play(string clipName, Vector3 point, float volume, float pitch, bool loop, bool ignoreTimeScale)
    {
        if (!Profile.soundEnable)
        {
            return null;
        }
        AudioClip clip = AudioClipManager.GetClip(clipName);
        if(clip == null)
        {
            Debug.Log("no clip " + clipName);
            return null;
        }
        GameObject go = new GameObject("Audio:" + clipName)
        {
            transform =
            {
                parent = base.transform,
                localPosition = point
            }
        };
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        float num = 1f;
        // 时间缩放影响声音
        if(!ignoreTimeScale)
        {
            num = Mathf.Clamp(Time.timeScale, 0.75f, 1f);
        }
        source.pitch = pitch * num;
        source.loop = loop;
        source.Play();
        // 如果不是循环的，播放完消除掉
        if(!loop)
        {
            Destroy(go, source.clip.length + 0.5f);
        }
        return source;
    }

    public void PlayBG(string clipName)
    {
        if (Profile.musicEnable)
        {
            string[] strArray = clipName.Split(new char[] { '/'});
            string str = strArray[strArray.Length - 1];
            if((this.audioMgr != null) && (this.audioMgr.clip != null) && this.audioMgr.clip.name.Equals(str))
            {
                if(!this.audioMgr.isPlaying)
                {
                    this.audioMgr.volume = 1f;
                    this.audioMgr.Play();
                }
            }
            else
            {
                this.audioMgr.volume = 1f;
                this.audioMgr.clip = AudioClipManager.GetClip(clipName);
                this.audioMgr.Play();
            }
        }
    }
    public void StopBG()
    {
        this.audioMgr.Stop();
    }
}
