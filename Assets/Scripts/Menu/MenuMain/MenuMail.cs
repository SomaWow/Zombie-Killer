using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Menu
{
    public class MenuMail : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private Button closeButton;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            closeButton = Find<Button>("closeButton");
            closeButton.onClick.AddListener(CloseClick);

            this.gameObject.SetActive(false);
            canvasGroup.alpha = 0;
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
            Debug.Log("+++ MenuMail.Show()");
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
        }
        public override void Hide()
        {
            Debug.Log("+++ MenuMail.Hide()");
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }
    }
}
