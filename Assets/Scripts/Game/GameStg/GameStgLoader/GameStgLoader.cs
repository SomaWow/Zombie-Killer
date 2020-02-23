using Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameStgLoader : MonoBehaviour
    {
        // 没有区分关卡类型，只有主线
        // public static CMissionName missionName;
        public static StgDataConfigure stgConfig;
        public static string sceneBundleName = string.Empty;
        public static string sceneName = string.Empty;
        public static string stgDataBundleName = string.Empty;
        public static StgData stgData;

        public Action<GameStgLoader, float> loadRoutineDelegate;
        public Action<GameStgLoader> loadEndDelegate;

        public static string[] AlwaysLoadPrefabs = new string[]
        {
            "Main/Hero",
            "Gun/M629",
            "UI/M629_AimPoint",
            "Gun/M10",
            "UI/M10_AimPoint",
            "Effect/Blood/BloodSplatEffect",
            "Effect/Blood/GreenBloodSplatEffect",
            //"UI/GameHeadshotEffect",
        };

        public void StartLoad()
        {
            // 初始化
            this.InitState();
            // 加载数据
            this.LoadStgData();
        }

        // 1.初始化，加载对应关卡的文本信息
        private void InitState()
        {
            stgDataBundleName = "_stgdatas";
        }

        // 加载文本数据
        private void LoadStgData()
        {
            AssetBundleLoader loader = AssetBundleLoader.GetLoader(stgDataBundleName);
            // 加载进程
            loader.loadRoutine = new AssetBundleLoader.LoadRoutineDelegate(this.StgDataBundleLoadRoutine);
            // 加载完毕进行下一步
            loader.loadEnd = new AssetBundleLoader.LoadEndDelegate(this.StgDataBundleLoadEnd);
            loader.loadFailed = new AssetBundleLoader.LoadFailedDelegate(this.StgDataBundleLoadFailed);
            loader.Load();
        }

        private void StgDataBundleLoadRoutine(AssetBundleLoader loader, float progress)
        {
            // 加载1段
            if (this.loadRoutineDelegate != null)
                this.loadRoutineDelegate(this, progress * 0.1f);
        }
        // 2.文本信息包加载完成后加载场景包
        private void StgDataBundleLoadEnd(AssetBundleLoader loader)
        {
            // 将文本资源存储起来
            AssetBundleStorer.Add(stgDataBundleName, loader.bundle);

            TextAsset textAsset = loader.bundle.LoadAsset(stgConfig.configureName, typeof(TextAsset)) as TextAsset;
            Debug.Log(textAsset.text);
            stgData = StgDataManager.LoadStgData(textAsset);
            sceneName = stgData.dataHead.map;
            sceneBundleName = "_scene" + sceneName;
            // 加载场景包
            this.LoadScene();
        }
        private void StgDataBundleLoadFailed(AssetBundleLoader loader)
        {
        }

        private void LoadScene()
        {
            AssetBundleLoader loader = AssetBundleLoader.GetLoader(sceneBundleName);
            loader.loadRoutine = new AssetBundleLoader.LoadRoutineDelegate(this.SceneBundleLoadRoutine);
            loader.loadEnd = new AssetBundleLoader.LoadEndDelegate(this.SceneBundleLoadEnd);
            loader.loadFailed = new AssetBundleLoader.LoadFailedDelegate(this.SceneBundleLoadFailed);
            loader.Load();
        }
        private void SceneBundleLoadRoutine(AssetBundleLoader loader, float progress)
        {
            // 加载2段
            if (this.loadRoutineDelegate != null)
                this.loadRoutineDelegate(this, (progress * 0.5f) + 0.1f);
        }
        // 3.加载完成后实例化场景
        private void SceneBundleLoadEnd(AssetBundleLoader loader)
        {
            if (loader != null)
            {
                // 把资源存储起来
                AssetBundleStorer.Add(sceneBundleName, loader.bundle);
            }
            base.StartCoroutine("SceneInstantiateRoutine");
        }
        private void SceneBundleLoadFailed(AssetBundleLoader loader)
        {

        }
        // 场景实例化的协程
        private IEnumerator SceneInstantiateRoutine()
        {
            AsyncOperation loadAsync = SceneManager.LoadSceneAsync(GameStgLoader.sceneName);
            Debug.Log("开始异步加载");
            while(loadAsync.progress < 1)
            {
                // 加载3段
                if(loadRoutineDelegate != null)
                    this.loadRoutineDelegate(this, ((loadAsync.progress * 0.3f) + 0.5f) + 0.1f);
                yield return null;
            }

            if (loadRoutineDelegate != null)
                this.loadRoutineDelegate(this, 0.9f);
            Debug.Log("结束异步加载");
            this.LoadAdditiveRoutine();
        }
        // 4.加载预制体
        private void LoadAdditiveRoutine()
        {
            // 加载预制体
            for (int i = 0; i < AlwaysLoadPrefabs.Length; i++)
            {
                PrefabResources.Get(AlwaysLoadPrefabs[i]);
            }
            //HeroController.isMocked = false;
            // 实例化英雄
            Debug.Log("实例化英雄");
            Transform transform = Instantiate(PrefabResources.Get("Main/Hero")) as Transform;
            transform.gameObject.SetActive(true);


            // 加载4段
            if (this.loadRoutineDelegate != null)
                this.loadRoutineDelegate(this, 1f);
            // 加载结束
            if (this.loadEndDelegate != null)
                this.loadEndDelegate(this);
        }
    }
}