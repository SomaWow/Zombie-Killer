using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuBottomBarMain : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private Button packagesButton;
        private Button trophiesButton;
        private Button optionsButton;
        private Button playButton;

        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            packagesButton = Find<Button>("packagesButton");
            packagesButton.onClick.AddListener(PackagesClick);
            trophiesButton = Find<Button>("trophiesButton");
            trophiesButton.onClick.AddListener(TrophiesClick);
            optionsButton = Find<Button>("optionsButton");
            optionsButton.onClick.AddListener(OptionsClick);
            playButton = Find<Button>("playButton");
            playButton.onClick.AddListener(PlayClick);
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
            rectTransform.DOAnchorPosY(50, 0.2f);

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

        public void PackagesClick()
        {
            Menu.INSTANCE.mMain.PackagesClick();
        }
        public void TrophiesClick() {
            Menu.INSTANCE.mMain.TrophiesClick();
        }
        
        public void OptionsClick() {
            Menu.INSTANCE.mMain.OptionsClick();
        }
        public void PlayClick() {
            Menu.INSTANCE.mMain.PlayClick();
        }

    }
}