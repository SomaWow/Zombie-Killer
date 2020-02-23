using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class GameMenuController : MonoBehaviour
    {
        public Action fireDown;
        public Action fireRelease;
        public Action addBulletClick;
        public Action reloadClick;
        public Action changeWeaponClick;
        public Action item1Click;
        public Action item2Click;
        public Action pauseClick;

        public CanvasGroup canvasGroup;
        public bool canControl = true;
        public bool canControlItem = true;

        public EventTriggerHandler fireButton;
        public Button reloadButton;
        public Button changeWeaponButton;

        //public GameMenuItemView item1View;

        public static string item1Name = string.Empty;
        public static string item2Name = string.Empty;

        private void Start()
        {
            fireButton = Find<EventTriggerHandler>("Anchor_BottomRight/fireButton");
            fireButton.onPointerDown = (Action)Delegate.Combine(fireButton.onPointerDown, new Action(this.DownFire));
            fireButton.onPointerUp = (Action)Delegate.Combine(fireButton.onPointerUp, new Action(this.ReleaseFire));

            reloadButton = Find<Button>("Anchor_BottomRight/reloadButton");
            reloadButton.onClick.AddListener(this.ClickReload);

            changeWeaponButton = Find<Button>("Anchor_BottomRight/changeWeaponButton");
            changeWeaponButton.onClick.AddListener(this.ClickChangeWeapon);
            
        }
        private void Update()
        {
            this.ResetButton();
        }
        /// <summary>
        /// 重置各按钮状态，由Update每帧调用
        /// </summary>
        private void ResetButton()
        {

        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            base.gameObject.SetActive(false);
        }

        private void Show()
        {
            base.gameObject.SetActive(true);
        }
        /// <summary>
        /// 增加子弹
        /// </summary>
        private void ClickAddBullet()
        {
            if(this.addBulletClick != null)
            {
                this.addBulletClick();
            }
        }

        /// <summary>
        /// 切换武器
        /// </summary>
        private void ClickChangeWeapon()
        {
            if((changeWeaponClick != null) && this.canControl)
            {
                this.changeWeaponClick();
            }
        }
        /// <summary>
        /// 使用道具1
        /// </summary>
        private void ClickItem1(GameMenuItemView itemView)
        {

        }
        /// <summary>
        /// 使用道具2
        /// </summary>
        private void ClickItem2()
        {

        }
        /// <summary>
        /// 暂停
        /// </summary>
        private void ClickPause()
        {
            if(this.pauseClick != null)
            {
                this.pauseClick();
            }
        }
        /// <summary>
        /// 开枪
        /// </summary>
        private void DownFire()
        {
            if((this.fireDown != null) && this.canControl)
            {
                this.fireDown();
            }
        }
        /// <summary>
        /// 停止开枪
        /// </summary>
        private void ReleaseFire()
        {
            if((this.fireRelease != null) && this.canControl)
            {
                this.fireRelease();
            }
        }
        /// <summary>
        /// 换弹夹
        /// </summary>
        private void ClickReload()
        {
            if((this.reloadClick != null) && this.canControl)
            {
                reloadClick();
            }
        }

        // 工具
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