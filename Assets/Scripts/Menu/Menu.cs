using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    /// <summary>
    /// 单例，控制所有Menu
    /// </summary>
    public class Menu : MonoBehaviour
    {
        public static Menu INSTANCE;

        public static bool isFromGame;

        public MenuCameraController mCameraController;

        // 上下方按钮
        public MenuTopBar mTopBar;
        public MenuBottomBar mBottomBar;

        // 主界面
        public MenuMain mMain;
        //public MenuDailyReward mDailyReward;
        public MenuMail mMail;
        public MenuShop mShop;
        public MenuAchivement mAchivement;
        public MenuSetting mSetting;

        // Play界面
        public MenuFirstPage mFirstPage;


        // Mission界面
        public MenuMission mMission;

        public MenuOverlay mOverlay;

        [HideInInspector]
        public int currentMenuTag;
        [HideInInspector]
        public int preMenuTag;

        private void Awake()
        {
            INSTANCE = this;
            Profile.LoadAll();

            mCameraController = GetComponent<MenuCameraController>();
            mTopBar = Find<MenuTopBar>("TopBar");
            mBottomBar = Find<MenuBottomBar>("BottomBar");
            mMain = Find<MenuMain>("MainPanel");
            mMain.showEndDelegate = (Action<MenuAbs>)Delegate.Combine(mMain.showEndDelegate, new Action<MenuAbs>(this.MenuShowEnd));
            //mDailyReward = Find<MenuDailyReward>("DailyRewardPanel");
            mMail = Find<MenuMail>("MailPanel");
            mShop = Find<MenuShop>("ShopPanel");
            mAchivement = Find<MenuAchivement>("AchivementPanel");
            mSetting = Find<MenuSetting>("SettingPanel");
            mFirstPage = Find<MenuFirstPage>("FirstPanel");
            mFirstPage.showEndDelegate = (Action<MenuAbs>)Delegate.Combine(mFirstPage.showEndDelegate, new Action<MenuAbs>(this.MenuShowEnd));
            mMission = Find<MenuMission>("MissionPanel");
            mMission.showEndDelegate = (Action<MenuAbs>)Delegate.Combine(mMission.showEndDelegate, new Action<MenuAbs>(this.MenuShowEnd));
            mOverlay = Find<MenuOverlay>("OverlayPanel");
            // 清空才能再次加载，否则对象存在，对象里的实例已经被销毁
            MenuWeaponLoader.ClearLoaders();
        }

        private void Start()
        {
            Debug.Log("Menu.start");
            AudioManager.INSTANCE.PlayBG("Bg/menu");
            this.mAchivement.ResetItemsWithoutCheck();
            if(isFromGame)
            {
                this.mMission.Show();
                mMain.gameObject.SetActive(false);
                mFirstPage.gameObject.SetActive(false);
                this.mBottomBar.bMain.gameObject.SetActive(false);
                this.mBottomBar.bFirstPage.gameObject.SetActive(false);
                this.mBottomBar.gameObject.SetActive(false);
            }
            else
            {
                this.mMain.Show();
            }
            this.mMission.ResetState();

        }

        public void BackClick()
        {
            this.GetMenu(this.currentMenuTag).BackClick();
        }

        public MenuAbs GetMenu(int TAG)
        {
            switch (TAG)
            {
                case 0:
                    return this.mMain;
                case 1:
                    return this.mFirstPage;
                case 2:
                    return this.mMission;
                default:
                    return null;
            }
        }

        public void MenuShowEnd(MenuAbs menu)
        {
            this.currentMenuTag = menu.TAG;
        }

        public T Find<T>(string name)
        {
            if (transform.Find(name) == null)
            {
                Debug.LogError(this + "子对象: " + name + "没有找到！");
                return default(T);
            }
            return transform.Find(name).GetComponent<T>();
        }

    }
}