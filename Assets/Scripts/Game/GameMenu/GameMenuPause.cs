using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class GameMenuPause : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Text infoText;

        private bool isShown;


        private void Update()
        {
            if (this.isShown)
            {
                Time.timeScale = 0f;
                RealTimeScale.timeScale = 0f;
            }
        }

        public void Show()
        {
            this.isShown = true;
            base.gameObject.SetActive(true);
            Tween tween = this.canvasGroup.DOFade(1f, 0.5f);
            tween.SetUpdate(true);
            tween.Play();
            // 待补充
            Time.timeScale = 0f;
            RealTimeScale.timeScale = 0f;

        }

        public void PlayClick()
        {
            this.Hide();
        }

        public void QuitClick()
        {
            this.Hide();
            // 直接算作任务失败
            //HeroController.INSTANCE.PlayHeroDieNoSlomo();
        }

        public void Hide()
        {
            this.isShown = false;
            this.canvasGroup.DOFade(0f,0.5f).OnComplete(HideEnd);
            Time.timeScale = 1f;
            RealTimeScale.timeScale = 1f;

        }

        public void HideEnd()
        {
            base.gameObject.SetActive(false);
        }
    }
}