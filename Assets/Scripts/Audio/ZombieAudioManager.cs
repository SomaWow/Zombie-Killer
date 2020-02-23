using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudioManager : MonoBehaviour {
    private const float minTimeScale = 0.75f;
    private static ZombieAudioManager _INSTANCE;
    private static Transform root;
    private static TypeInfo[] typeInfoArray = new TypeInfo[6];
    private static Dictionary<string, ClipInfo> clipInfoDict = new Dictionary<string, ClipInfo>();
    private static List<string> clipInfoDeleteList = new List<string>();

    private static void IntializeTypeInfo()
    {
        TypeInfo info = new TypeInfo
        {
            volumeMul = 0.5f,
            max = 3,
            interval = 1f
        };
        typeInfoArray[0] = info;
        TypeInfo info2 = new TypeInfo
        {
            max = 5,
            interval = 0.1f
        };
        typeInfoArray[1] = info2;
        TypeInfo info3 = new TypeInfo
        {
            max = 3
        };
        typeInfoArray[2] = info3;
        TypeInfo info4 = new TypeInfo
        {
            volumeMul = 0.65f,
            max = 5,
            interval = 0.25f
        };
        typeInfoArray[3] = info4;
        TypeInfo info5 = new TypeInfo
        {
            max = 3,
            interval = 3f
        };
        typeInfoArray[4] = info5;
        TypeInfo info6 = new TypeInfo
        {
            max = 10,
            interval = 0.1f
        };
        typeInfoArray[5] = info6;
    }

    public void ClearAll()
    {
        foreach (ClipInfo info in clipInfoDict.Values)
        {
            if (info.audioSource != null)
            {
                Destroy(info.audioSource.gameObject);
            }
        }
        clipInfoDict.Clear();
    }

    public static void Play(string clipName, ZombieAudioType type)
    {
        Play(clipName, type, null, 0f, 1f, RandomPitch);
    }

    public static void Play(string clipName, ZombieAudioType type, float delay)
    {
        Play(clipName, type, null, delay, 1f, RandomPitch);
    }

    public static void Play(string clipName, ZombieAudioType type, Transform zombieTrans)
    {
        Play(clipName, type, zombieTrans, 0f, 1f, RandomPitch);
    }

    public static void Play(string clipName, ZombieAudioType type, Transform zombieTrans, float delay)
    {
        Play(clipName, type, zombieTrans, delay, 1f, RandomPitch);
    }

    // 参数：clipName，僵尸声音类型，僵尸Transform，延迟，声音大小，音调
    public static void Play(string clipName, ZombieAudioType type, Transform zombieTrans, float delay, float volMul, float pitch)
    {
        if (Profile.soundEnable)
        {
            int index = (int)type;
            string key = clipName + index;
            if (!clipInfoDict.ContainsKey(key))
            {
                TypeInfo info = typeInfoArray[index];
                if ((info.current < info.max) && (Time.realtimeSinceStartup >= (info.lastCallTime + info.interval)))
                {
                    AudioClip clip = AudioClipManager.GetClip(clipName);
                    if (clip == null)
                    {
                        Debug.Log("no clip" + clipName);
                    }
                    else
                    {
                        ClipInfo clipInfo = new ClipInfo
                        {
                            zombieTrans = zombieTrans,
                            name = clipName,
                            type = type,
                            playTime = Time.realtimeSinceStartup,
                            delay = delay,
                            volMul = volMul,
                            pitch = pitch
                        };
                        info.current++;
                        info.lastCallTime = Time.realtimeSinceStartup;
                        // 如果没有延迟，直接播放，有延迟在update中处理
                        if (clipInfo.delay <= 0f)
                        {
                            AudioSource source = new GameObject("Audio:" + clipName) { transform = { parent = root } }.AddComponent<AudioSource>();
                            source.clip = clip;
                            source.volume = clipInfo.volMul * info.volumeMul;
                            source.loop = false;
                            float num2 = Mathf.Clamp(Time.timeScale, 0.75f, 1f);
                            source.pitch = clipInfo.pitch * num2;
                            clipInfo.length = clip.length / source.pitch;
                            clipInfo.audioSource = source;
                            source.Play();
                            SpecialDeal(clipInfo);
                        }
                        clipInfoDict.Add(key, clipInfo);
                    }
                }
            }
        }
    }

    // 如果该僵尸在接近中，所有声音音量减半
    private static void SpecialDeal(ClipInfo clipInfo)
    {
        if (clipInfo.type == ZombieAudioType.APPROACH)
        {
            foreach (ClipInfo info in clipInfoDict.Values)
            {
                info.volMul *= 0.5f;
                if (info.audioSource != null)
                {
                    info.audioSource.volume *= 0.5f;
                }
            }
        }
    }

    private void Update()
    {
        foreach (string str in clipInfoDict.Keys)
        {
            ClipInfo clipInfo = clipInfoDict[str];
            int type = (int)clipInfo.type;
            TypeInfo info2 = typeInfoArray[type];
            // 处理延迟播放
            if ((clipInfo.delay > 0f) && ((Time.realtimeSinceStartup - clipInfo.playTime) >= clipInfo.delay))
            {
                clipInfo.delay = 0f;
                clipInfo.playTime = Time.realtimeSinceStartup;
                AudioClip clip = AudioClipManager.GetClip(clipInfo.name);
                AudioSource source = new GameObject("Audio:" + clipInfo.name) { transform = { parent = root } }.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = clipInfo.volMul * info2.volumeMul;
                source.loop = false;
                float num2 = Mathf.Clamp(Time.timeScale, 0.75f, 1f);
                source.pitch = clipInfo.pitch * num2;
                clipInfo.length = clip.length / source.pitch;
                clipInfo.audioSource = source;
                source.Play();
                SpecialDeal(clipInfo);
            }
            else if (Time.realtimeSinceStartup >= (clipInfo.playTime + clipInfo.length))
            {
                clipInfoDeleteList.Add(str);
                info2.current--;
                if (clipInfo.audioSource != null)
                {
                    Destroy(clipInfo.audioSource.gameObject);
                }
            }
        }
        if (clipInfoDeleteList.Count > 0)
        {
            for (int i = 0; i < clipInfoDeleteList.Count; i++)
            {
                clipInfoDict.Remove(clipInfoDeleteList[i]);
            }
            clipInfoDeleteList.Clear();
        }
    }

    // 属性
    public static ZombieAudioManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                GameObject target = new GameObject("ZombieAudioManager");
                _INSTANCE = target.AddComponent<ZombieAudioManager>();
                root = target.transform;
                IntializeTypeInfo();
                DontDestroyOnLoad(target);
            }
            return _INSTANCE;
        }
    }

    private static float RandomPitch
    {
        get
        {
            return Random.Range(0.8f, 1.2f);
        }
    }

    private class ClipInfo
    {
        public Transform zombieTrans;
        public AudioSource audioSource;
        public string name = string.Empty;
        public ZombieAudioType type;
        public float playTime;
        public float length;
        public float delay;
        public float volMul = 1f;
        public float pitch = 1f;
    }

    private class TypeInfo
    {
        public float volumeMul = 1f;
        public int current;
        public int max = 1;
        public float interval;
        public float lastCallTime;
    }
}
