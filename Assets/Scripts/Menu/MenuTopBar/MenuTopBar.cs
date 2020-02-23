using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuTopBar : MenuAbs
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Button addButton;
        private Button backButton;
        public Text greetBoardText;
        public Text goldText;
        public Text gemText;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            goldText = Find<Text>("Money/coinText");
            gemText = Find<Text>("Money/gemText");
            addButton = Find<Button>("Money/addMoneyButton");
            addButton.onClick.AddListener(AddMoney);

            backButton = Find<Button>("backButton");
            backButton.onClick.AddListener(BackClick);

            greetBoardText = Find<Text>("GreetBoard/Text");
        }

        private void Start()
        {
            greetBoardText.text = "Hi! " + Profile.username;
            this.ResetMoney();
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
            rectTransform.DOAnchorPosY(-55, 0.2f);
        }
        public override void Hide()
        {
            canvasGroup.DOFade(0, 0.2f);
            rectTransform.DOAnchorPosY(55, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }

        private void AddMoney()
        {
            Menu.INSTANCE.mShop.ShowGold();
            AudioManager.INSTANCE.Play("UI/button1");
        }

        public override void BackClick()
        {
            Debug.Log("+++ MenuTopbar.backclick");
            Menu.INSTANCE.BackClick();
            AudioManager.INSTANCE.Play("UI/back");
        }

        public void ResetMoney()
        {
            goldText.text = Profile.gold.ToString();
            gemText.text = Profile.gem.ToString();
        }

        public void RefreshMoney()
        {
            int goldNum = int.Parse(goldText.text);
            int gemNum = int.Parse(gemText.text);
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                goldText.text = temp.ToString();
            }, goldNum, Profile.gold, 0.9f);
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                gemText.text = temp.ToString();
            }, gemNum, Profile.gem, 0.9f);
        }
    }
}