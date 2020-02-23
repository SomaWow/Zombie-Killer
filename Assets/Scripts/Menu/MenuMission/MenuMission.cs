using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game;

namespace Menu {
    public class MenuMission : MenuAbs {

        // 组件
        private CanvasGroup canvasGroup;
        public CameraShakeEffect shakeEffect;
        private Image startEffect;
        // 开始按钮
        private Button startButton;
        // 左侧按钮
        //private Button dailyTaskButton;
        private Button trophiesButton;
        //private Button equipItemButton;
        
        public MenuMissionInfo missionInfo;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            shakeEffect = GetComponent<CameraShakeEffect>();
            startEffect = Find<Image>("startEffect");
            startButton = Find<Button>("Info/startButton");
            startButton.onClick.AddListener(StartClick);

            //dailyTaskButton = Find<Button>("LeftButtons/dailyTaskButton");
            //dailyTaskButton.onClick.AddListener(DailyTaskClick);
            trophiesButton = Find<Button>("LeftButtons/trophiesButton");
            trophiesButton.onClick.AddListener(TrophiesClick);
            //equipItemButton = Find<Button>("LeftButtons/equipItemButton");
            //equipItemButton.onClick.AddListener(EquipItemClick);

            canvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);
            this.startEffect.gameObject.SetActive(false);
        }

        public override void BackClick()
        {
            Menu.INSTANCE.mMission.Hide();
            Menu.INSTANCE.mFirstPage.Show();
            Menu.INSTANCE.mBottomBar.Show();
            Menu.INSTANCE.mBottomBar.bFirstPage.Show();
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
            this.startEffect.gameObject.SetActive(false);
            canvasGroup.DOFade(1, 0.2f).OnComplete(ShowEnd) ;
            // 每次显示该页面，更新状态
            this.ResetState();
        }
        public void ShowEnd()
        {
            if(base.showEndDelegate != null)
            {
                base.showEndDelegate(this);
            }
        }

        public override void Hide()
        {
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }

        public void ResetState()
        {
            this.missionInfo.ResetState();
        }

        private void StartClick()
        {
            AudioManager.INSTANCE.Play("Gun/fire");
            // (float x, float y, float z, float t, float acc, int times) 一次移动的时间为0.04，acc为10，主动震动五次
            Menu.INSTANCE.mMission.shakeEffect.StartShake(5f, 5f, 0f, 0.04f, 10f, 5);
            this.startEffect.gameObject.SetActive(true);
            Menu.INSTANCE.mOverlay.Show();
            GameStgLoader.stgConfig = this.missionInfo.stgConfig;

        }

        public void DailyTaskClick()
        {
            Menu.INSTANCE.mAchivement.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }
        public void TrophiesClick()
        {
            Menu.INSTANCE.mAchivement.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }
        public void EquipItemClick()
        {
            Menu.INSTANCE.mAchivement.Show();
            AudioManager.INSTANCE.Play("UI/button1");
        }

        public override int TAG
        {
            get
            {
                return 2;
            }
        }

    }
}