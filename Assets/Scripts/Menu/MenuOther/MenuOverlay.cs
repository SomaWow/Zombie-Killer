using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuOverlay : MenuAbs
    {
        private Image panel;
        private void Awake()
        {
            panel = GetComponent<Image>();
            this.gameObject.SetActive(false);
        }


        public override void Show()
        {
            this.gameObject.SetActive(true);
            panel.color = Color.clear;
            Sequence sequence = DOTween.Sequence();
            sequence.PrependInterval(1f).Append(panel.DOFade(1f, 0.8f).SetEase(Ease.InOutQuad).OnComplete(ShowEnd));
            
        }
        private void ShowEnd()
        {
            // 加载场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("_gameLoader");
            // 停止背景音乐
            AudioManager.INSTANCE.StopBG();
        }
        public override void Hide()
        {
            
        }
    }
}