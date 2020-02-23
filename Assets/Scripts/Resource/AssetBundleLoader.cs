using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class AssetBundleLoader : MonoBehaviour
    {
        // 划分平台
#if UNITY_EDITOR || UNITY_STANDALONE
        public static string localURL = "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
        public static string localURL = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
        public static string localURL = "file://" + Application.dataPath + "/Raw/";
#endif

        public static string extension = ".assetbundle";
        public string assetBundleName = string.Empty;
        public int assetBundleVersion = 1;
        public WWW www;
        public bool isDone;
        private float loadEndTime;
        public LoadEndDelegate loadEnd;
        public LoadRoutineDelegate loadRoutine;
        public LoadFailedDelegate loadFailed;

        public AssetBundle bundle
        {
            get
            {
                return this.www.assetBundle;
            }
        }

        public static AssetBundleLoader GetLoader(string name)
        {
            AssetBundleLoader loader = new GameObject(name + "Loader").AddComponent<AssetBundleLoader>();
            loader.assetBundleName = name;
            return loader;
        }

        public void Load()
        {
            string str = string.Empty;
            str = localURL + this.assetBundleName + extension;
            StartCoroutine("LoadRoutine", str);
        }

        private IEnumerator LoadRoutine(string url)
        {
            this.www = WWW.LoadFromCacheOrDownload(url, this.assetBundleVersion);
            // 下载的时候一致循环
            while (!www.isDone)
            {
                if (this.loadRoutine != null)
                {
                    this.loadRoutine(this, this.www.progress);
                }
                yield return null;
            }
            // 下载完毕执行
            if(this.www.error != null) // 如果下载出错
            {
                Debug.Log("AssetBundle " + this.assetBundleName + " Load Error" + this.www.error);
            }
            else
            {
                if(this.loadEnd != null)
                {
                    loadEnd(this);
                }
                else
                {
                    this.www.assetBundle.Unload(true);
                }
                this.isDone = true;
                this.loadEndTime = Time.realtimeSinceStartup;
            }
        }

        public delegate void LoadRoutineDelegate(AssetBundleLoader loader, float progress);
        public delegate void LoadEndDelegate(AssetBundleLoader loader);
        public delegate void LoadFailedDelegate(AssetBundleLoader loader);
    }
}