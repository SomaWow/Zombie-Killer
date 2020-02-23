using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Menu
{
    public class MenuBottomBarFirstPage : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private Button missionButton;

        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            missionButton = Find<Button>("missionButton");
            missionButton.onClick.AddListener(MissionClick);

            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }
        public override void Show()
        {
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.1f);
            rectTransform.DOAnchorPosY(50, 0.2f);

        }
        public override void Hide()
        {
            canvasGroup.DOFade(0, 0.2f);
            rectTransform.DOAnchorPosY(-50, 0.2f);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }

        public void MissionClick()
        {
            Menu.INSTANCE.mFirstPage.MissionClick();
        }
    }
}