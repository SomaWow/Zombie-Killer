using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameStgUnloader : MonoBehaviour
    {
        public Action<GameStgUnloader, float> loadRoutineDelegate;
        public Action<GameStgUnloader> loadEndDelegate;

        public void StartUnload()
        {
            // 之前加载的资源删除掉
            AssetBundleStorer.ReleaseForced(GameStgLoader.stgDataBundleName);
            AssetBundleStorer.ReleaseForced(GameStgLoader.sceneBundleName);

            base.StartCoroutine("SceneInstantiateRoutine");
        }

        private IEnumerator SceneInstantiateRoutine()
        {
            AsyncOperation loadAsync = SceneManager.LoadSceneAsync("_menu");
            Debug.Log("开始异步加载菜单场景");
            while (loadAsync.progress < 1)
            {
                // 加载3段
                if (loadRoutineDelegate != null)
                    this.loadRoutineDelegate(this, loadAsync.progress);
                yield return null;
            }

            if (loadEndDelegate != null)
                this.loadEndDelegate(this);
            Debug.Log("结束异步加载菜单场景");
        }
    }
}