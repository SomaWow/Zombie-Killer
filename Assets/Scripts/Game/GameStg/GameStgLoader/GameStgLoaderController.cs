using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game
{
    public class GameStgLoaderController : MonoBehaviour
    {
        // 组件
        private CanvasGroup canvasGroup;
        public GameStgLoader loader;
        public Transform circleTF;
        // 参数
        private float targetProgress;
        private float currentProgress;
        private bool isLoadEnd;
        private bool isHided;

        private void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
            this.loader.loadRoutineDelegate = new Action<GameStgLoader, float>(this.GameLoadRoutine);
            this.loader.loadEndDelegate = new Action<GameStgLoader>(this.GameLoadEnd);
            // 开始加载
            this.loader.StartLoad();
            this.Show();
            //Time.timeScale = 0f;
            //RealTimeScale.timeScale = 0f;
            // 待补充
            // 下面随机加载的提示
        }

        private void Show()
        {
            canvasGroup.DOFade(1, 0.2f);
        }
        private void Hide()
        {
            Time.timeScale = 1f;
            RealTimeScale.timeScale = 1f;
            this.canvasGroup.DOFade(0, 0.8f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            // weapon设置
            HeroController.INSTANCE.weaponController.EnableWeapon();
            Destroy(this.gameObject);
        }

        private void GameLoadRoutine(GameStgLoader gameLoader, float progress)
        {
            this.targetProgress = progress;
        }
        private void GameLoadEnd(GameStgLoader gameLoader)
        {
            this.targetProgress = 1f;
            this.isLoadEnd = true;
        }

        private void Update()
        {
            circleTF.Rotate(Vector3.forward, 400 * Time.unscaledDeltaTime, Space.World);
            // 这里可以做一个进度条
            this.currentProgress += Time.unscaledDeltaTime * 0.3f;
            if (this.currentProgress > this.targetProgress)
                this.currentProgress = this.targetProgress;
            // 加载完成且未隐藏
            if((this.isLoadEnd && (this.currentProgress >= 1f)) && !this.isHided)
            {
                this.isHided = true;
                this.Hide();
            }
        }
    }
}