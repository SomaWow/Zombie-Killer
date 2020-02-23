using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuFirstPageInfo : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private Text nameText;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            nameText = Find<Text>("nameText");
        }

        public void SetInfo(string name)
        {
            base.gameObject.SetActive(true);
            this.nameText.text = name;
        }

        public void HideInfo()
        {
            this.gameObject.SetActive(false);
            canvasGroup.alpha = 0;
        }
        public void ShowInfo()
        {

            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InExpo);
        }
    }
}