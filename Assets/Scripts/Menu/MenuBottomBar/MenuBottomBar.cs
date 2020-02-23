using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Menu
{
    public class MenuBottomBar : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        // 主界面按钮群
        public MenuBottomBarMain bMain;
        // Play界面按钮群
        public MenuBottomBarFirstPage bFirstPage;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            bMain = Find<MenuBottomBarMain>("BottomBarMain");
            bFirstPage = Find<MenuBottomBarFirstPage>("BottomBarFirstPage");
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
            rectTransform.DOAnchorPosY(70, 0.2f);
        }
        public override void Hide()
        {
            canvasGroup.DOFade(0, 0.2f);
            rectTransform.DOAnchorPosY(-50, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }
    }
}