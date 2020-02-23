using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuDailyReward : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private Button closeButton;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            closeButton = Find<Button>("closeButton");
            closeButton.onClick.AddListener(CloseClick);

            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }


        public override void BackClick()
        {
            CloseClick();
        }
        public void CloseClick()
        {
            Hide();
            AudioManager.INSTANCE.Play("UI/button2");
        }

        public override void Show()
        {
            Debug.Log("+++ MenuDailyReward.Show()");
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
        }
        public override void Hide()
        {
            Debug.Log("+++ MenuDailyReward.Hide()");
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }

    }
}