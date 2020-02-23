using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Menu
{
    public class MenuShop : MenuAbs
    {
        // 组件
        private CanvasGroup canvasGroup;
        private Button closeButton;
        private Button goldButton;
        private Button gemButton;
        private GameObject goldTab;
        private GameObject gemTab;

        // 组件初始化后隐藏
        public void Awake()
        {
            InitComponent();
            this.HideAll();
            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);

        }

        private void InitComponent()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            closeButton = Find<Button>("closeButton");
            closeButton.onClick.AddListener(CloseClick);

            goldButton = Find<Button>("goldButton");
            goldButton.onClick.AddListener(TabGoldClick);
            gemButton = Find<Button>("gemButton");
            gemButton.onClick.AddListener(TabGemClick);
            goldTab = transform.Find("GoldTab").gameObject;
            gemTab = transform.Find("GemTab").gameObject;
        }

        public override void BackClick()
        {
            this.CloseClick();
        }

        private void CloseClick()
        {
            this.Hide();
            // 声音
            AudioManager.INSTANCE.Play("UI/button2");
        }

        public override void Show()
        {
            Debug.Log("+++ MenuShop.Show()");
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
        }
        public override void Hide()
        {
            Debug.Log("+++ MenuShop.Hide()");
            // 动画
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }
        private void HideAll()
        {

            Debug.Log("+++ MenuShop.HideAll()");
            this.goldTab.SetActive(false);
            this.gemTab.SetActive(false);
        }

        public void ShowGold()
        {
            this.HideAll();
            this.goldTab.SetActive(true);
            this.Show();
        }

        // 切换到金币购买页面
        public void TabGoldClick()
        {
            this.HideAll();
            this.goldTab.SetActive(true);
            AudioManager.INSTANCE.Play("UI/tab");
        }
        // 切换到宝石购买页面
        public void TabGemClick()
        {
            this.HideAll();
            this.gemTab.SetActive(true);
            AudioManager.INSTANCE.Play("UI/tab");
        }
        
        public void GoldClick(int num)
        {
            Profile.UpdateGold(num);
            AudioManager.INSTANCE.Play("UI/tab");
            AudioManager.INSTANCE.Play("Effect/coin_show");
            Menu.INSTANCE.mTopBar.RefreshMoney();
        }

        public void GemClick(int num)
        {
            Profile.UpdateGem(num);
            AudioManager.INSTANCE.Play("UI/tab");
            AudioManager.INSTANCE.Play("Effect/coin_show");
            Menu.INSTANCE.mTopBar.RefreshMoney();
        }
    }
}