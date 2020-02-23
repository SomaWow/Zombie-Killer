using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuMain : MenuAbs
    {
        //private Button dailyRewardButton;
        private Button mailButton;
        // 新闻
        public MenuMainNews mainNews;

        public override int TAG
        {
            get
            {
                return 0;
            }
        }

        private void Awake()
        {
            //dailyRewardButton = Find<Button>("LeftBar/dailyRewardButton");
            //dailyRewardButton.onClick.AddListener(DailyRewardClick);
            mailButton = Find<Button>("LeftBar/mailButton");
            mailButton.onClick.AddListener(MailClick);
            mainNews = transform.Find("News").GetComponent<MenuMainNews>();

            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public override void BackClick()
        {
            Debug.Log("+++ MenuMain.BackClick()");
            PlayerPrefs.Save();
            // 询问用户是否想退出TODO
            Application.Quit();
        }
        
        /// <summary>
        /// 显示窗口
        /// </summary>
        public override void Show()
        {
            Debug.Log("+++ MenuMain.Show()");
            this.gameObject.SetActive(true);
            ShowEnd();
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public override void Hide()
        {
            Debug.Log("+++ MenuMain.Hide()");
            this.gameObject.SetActive(false);
            HideEnd();
        }

        public void ShowEnd()
        {
            if (showEndDelegate != null)
                showEndDelegate(this);
        }
        public void HideEnd()
        {
            if (hideEndDelegate != null)
                hideEndDelegate(this);
        }

        // left button bar
        private void CheckDailyReward() { }
        private void CheckDailyRewardEnd() { }
        //public void DailyRewardClick() {
        //    Menu.INSTANCE.mDailyReward.Show();
        //    AudioManager.INSTANCE.Play("UI/button1");
        //}
        private void LoadDailyRewardEnd() { }
        private void LoadDailyRewardFailed() { }

        public void MailClick()
        {
            Menu.INSTANCE.mMail.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }

        // bottom button bar
        public void PackagesClick()
        {
            Menu.INSTANCE.mShop.ShowGold();
            AudioManager.INSTANCE.Play("UI/button1");
        }
        public void TrophiesClick() {
            Menu.INSTANCE.mAchivement.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }
        public void OptionsClick() {
            Menu.INSTANCE.mSetting.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }
        public void PlayClick() {
            // Main界面隐藏
            Menu.INSTANCE.mMain.Hide();
            // Play界面开启
            Menu.INSTANCE.mFirstPage.Show();
            // 下层按钮交接
            Menu.INSTANCE.mBottomBar.bMain.Hide();
            Menu.INSTANCE.mBottomBar.bFirstPage.Show();
            
            AudioManager.INSTANCE.Play("UI/button1");
        }
    }
}