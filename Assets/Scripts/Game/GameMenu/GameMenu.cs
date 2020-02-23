using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameMenu : MonoBehaviour
    {
        private const string prefabPath = "Prefabs/UI/GameMenuCanvas";
        public static GameMenu INSTANCE;
        //public Camera uiCamera;
        public GameMenuController gameController;
        public GameMenuInformation gameInformation;
        public GameMenuPause pauseMenu;
        public GameMenuEnd endMenu;
        // 控制观察方向的移动
        public GameCameraTouchPad touchPad;

        // 效果设置为其子物体
        public Transform effectPanelTrans;
        public Transform effect2PanelTrans;
        //public GameGunUIProjector lightProj;
        //public GameUIColorFlash colorFlash;
        public CameraShakeEffect camShake;
        //public GameJoyStick joystick;
        public Transform aimPointNode;

        private void Awake()
        {
            INSTANCE = this;
            // 委托 暂停
            this.gameController.pauseClick = (Action)Delegate.Combine(this.gameController.pauseClick, new Action(this.ShowPausePanel));
        }
        /// <summary>
        /// 静态方法创建游戏页面
        /// </summary>
        public static void CreateGameMenu()
        {
            Instantiate(Resources.Load<Transform>(prefabPath));
            Instantiate(Resources.Load<Transform>("Prefabs/UI/EventSystem"));
        }
        
        // 设置ui可见
        //public void SetVisible(bool isVisible)
        //{
        //    this.uiCamera.gameObject.SetActive(false);
        //}
        /// <summary>
        /// 显示暂停页面
        /// </summary>
        private void ShowPausePanel()
        {
            this.gameController.Hide();
            this.pauseMenu.Show();
        }

        // 设置是否可见
        public void SetControllerVisible(bool isVisible)
        {
            this.gameController.canvasGroup.alpha = isVisible ? 1f : 0f;
            this.gameController.canControl = isVisible;
        }
    }
}