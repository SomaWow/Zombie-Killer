using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace Menu
{
    public class MenuFirstPage : MenuAbs
    {
        // 组件
        private CanvasGroup canvasGroup;
        private RectTransform equipments;
        private GameCameraTouchPad touchPad;
        private MenuFirstPageInfo info;
        private MenuFirstPageGun gun1;
        private MenuFirstPageGun gun2;
        private MenuFirstPageGun item1;
        
        // 枪模型
        public GameObject gameObj;

        // 枪控制参数
        private float horizontalMinRot = -200f;
        private float horizontalMaxRot = 200f;
        private float verticalMinRot = -30f;
        private float verticalMaxRot = 30f;
        private float sensitiveX = 0.7f;
        private float sensitiveY = 0.35f;
        private float currentAcc;
        // 实际速度
        private Vector2 speed = Vector2.zero;
        // 最大速度，数值
        private float baseMaxSpeed = 5f;
        private float smoothBaseAcc = 0.3f;
        private float smoothResistance = 8f;
        // 记录点击的位置
        private Vector2 downPos = Vector2.zero;
        private Vector3 targetAngle = Vector3.zero;
        private float rotationSpeed = 20;
        private float rotationDamp = 0.35f;

        private bool isViewed;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            equipments = Find<RectTransform>("LeftPanel");
            touchPad = Find<GameCameraTouchPad>("Bg");
            info = Find<MenuFirstPageInfo>("RightPanel");
            gun1 = Find<MenuFirstPageGun>("LeftPanel/Box/Weapon1");
            gun2 = Find<MenuFirstPageGun>("LeftPanel/Box/Weapon2");
            item1 = Find<MenuFirstPageGun>("LeftPanel/Box/Item1");

            this.touchPad.onDrag = (Action<Vector2>)Delegate.Combine(this.touchPad.onDrag, new Action<Vector2>(this.OnDrag));
            this.touchPad.onDown = (Action<Vector2>)Delegate.Combine(this.touchPad.onDown, new Action<Vector2>(this.OnDown));
            this.touchPad.onUp = (Action<Vector2>)Delegate.Combine(this.touchPad.onUp, new Action<Vector2>(this.OnUp));

            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        public override void BackClick()
        {
            // Main界面开启
            Menu.INSTANCE.mMain.Show();
            // Play界面隐藏
            Menu.INSTANCE.mFirstPage.Hide();
            // 下层按钮交接
            Menu.INSTANCE.mBottomBar.bMain.Show();
            Menu.INSTANCE.mBottomBar.bFirstPage.Hide();
            Menu.INSTANCE.mCameraController.ShowCameraScene();
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
            // 装备面板上显示图片
            this.gun1.ShowWeapon(Profile.Gun1Name);
            this.gun2.ShowWeapon(Profile.Gun2Name);
            this.item1.ShowWeapon(Profile.Item1Name);

            // 默认选中第一个观察
            this.SetWeapon(gun1);
            
            canvasGroup.DOFade(1, 0.2f).OnComplete(ShowEnd);
            equipments.DOAnchorPosX(0, 0.2f);
        }

        public void SetWeapon(MenuFirstPageGun firstPageGun)
        {
            this.ReleaseAllCheck();
            firstPageGun.SetChecked(true);
            if(this.gameObj != null)
            {
                this.gameObj.SetActive(false);
            }
            this.info.SetInfo(firstPageGun.weaponName.ToUpper());
            this.speed = Vector2.zero;
            this.gameObj = firstPageGun.weaponLoader.weaponInstance;
            this.gameObj.transform.rotation = Quaternion.identity;
            this.gameObj.transform.position += new Vector3(3f, 0f, 0f);
            this.gameObj.SetActive(true);
            this.gameObj.transform.DOMove(firstPageGun.weaponLoader.originPos, 0.8f).SetEase(Ease.OutCubic);
        }

        private void ReleaseAllCheck()
        {
            this.gun1.SetChecked(false);
            this.gun2.SetChecked(false);
            this.item1.SetChecked(false);
        }

        public void ShowEnd()
        {
            Menu.INSTANCE.mCameraController.ShowCameraGun1();
            if(base.showEndDelegate!= null)
            {
                base.showEndDelegate(this);
            }
        }

        public override void Hide()
        {
            canvasGroup.DOFade(0, 0.2f);
            equipments.DOAnchorPosX(-360, 0.2f).OnComplete(HideEnd);
            Menu.INSTANCE.mCameraController.gunCamera1.SetActive(false);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }

        public void MissionClick()
        {
            // 界面切换
            Menu.INSTANCE.mFirstPage.Hide();
            Menu.INSTANCE.mMission.Show();
            // 按钮隐藏
            Menu.INSTANCE.mBottomBar.bFirstPage.Hide();
            Menu.INSTANCE.mBottomBar.Hide();

            AudioManager.INSTANCE.Play("UI/button1");
        }

        // 交换武器
        public void ChangeWeaponClick()
        {

        }
        // 交换道具
        public void ChangeItemClick()
        {

        }

        private void Update()
        {
            // 拖动枪旋转
            if (this.gameObj != null)
            {
                // 通过插值进行旋转
                //targetAngle.x = Mathf.Clamp(targetAngle.x, verticalMinRot, verticalMaxRot);
                //targetAngle.z = 0;
                //gunGo.transform.localRotation = Quaternion.Euler(targetAngle);

                if (this.currentAcc != 0f)
                {
                    if (this.currentAcc > 0f)
                    {
                        this.speed.x += this.currentAcc * Time.deltaTime;
                        this.currentAcc = Mathf.Max((float)(this.currentAcc - (Time.deltaTime * 5f)), (float)0f);
                    }
                    else
                    {
                        this.speed.x += this.currentAcc * Time.deltaTime;
                        this.currentAcc = Mathf.Min((float)(this.currentAcc + (Time.deltaTime * 5f)), (float)0f);
                    }
                }
                this.gameObj.transform.Rotate(new Vector3(this.speed.y * this.sensitiveY, this.speed.x * this.sensitiveX, 0f), Space.World);
                float magnitude = this.speed.magnitude;
                // 减速中
                if (magnitude > 0f)
                {
                    float num2 = 0f;
                    if (magnitude < 1f)
                    {
                        num2 = Mathf.Max((float)(magnitude - (((this.smoothResistance * this.smoothBaseAcc) * magnitude) * Time.deltaTime)), (float)0f);
                    }
                    else
                    {
                        num2 = Mathf.Max((float)(magnitude - ((this.smoothResistance * this.smoothBaseAcc) * Time.deltaTime)), (float)0f);
                    }
                    this.speed *= num2 / magnitude;
                }
                Vector3 localEulerAngles = this.gameObj.transform.localEulerAngles;
                localEulerAngles.y = Mathf.Clamp(ClampAngle(localEulerAngles.y), this.horizontalMinRot, this.horizontalMaxRot);
                localEulerAngles.x = Mathf.Clamp(ClampAngle(localEulerAngles.x), this.verticalMinRot, this.verticalMaxRot);
                localEulerAngles.z = 0f;
                this.gameObj.transform.localEulerAngles = localEulerAngles;
            }
        }

        // 将角度限制在-180， 180之间
        public float ClampAngle(float value)
        {
            float angle = value - 180;

            if (angle > 0)
            {
                return angle - 180;
            }

            if (value == 0)
            {
                return 0;
            }

            return angle + 180;
        }

        private void OnDrag(Vector2 delta)
        {
            //targetAngle += new Vector3(delta.y * smoothBaseAcc , -delta.x * smoothBaseAcc, 0f);
            //Debug.Log(targetAngle);

            delta.x = -delta.x;
            delta *= 0.25f;
            this.speed += delta * this.smoothBaseAcc;
            float baseMaxSpeed = this.baseMaxSpeed;
            float magnitude = this.speed.magnitude;
            // 限制最大速度
            if (magnitude > baseMaxSpeed)
            {
                this.speed *= baseMaxSpeed / magnitude;
            }

        }
        private void OnDown(Vector2 dPos)
        {
            this.downPos = dPos;
        }
        private void OnUp(Vector2 uPos)
        {
            // 如果点下和弹起间距较小，同时产生点击效果
            if(Vector2.Distance(this.downPos, uPos) <= 5f)
            {
                isViewed = !isViewed;
                if (isViewed) // 放大
                {
                    this.info.HideInfo();
                    equipments.DOAnchorPosX(-360, 0.2f);
                    Menu.INSTANCE.mBottomBar.bFirstPage.Hide();
                    Menu.INSTANCE.mBottomBar.Hide();
                    Menu.INSTANCE.mTopBar.Hide();
                    Menu.INSTANCE.mCameraController.GunCamera1Zoom();
                    this.currentAcc = -2f;
                }
                else // 缩小
                {
                    this.info.ShowInfo();
                    equipments.DOAnchorPosX(0, 0.2f);
                    Menu.INSTANCE.mBottomBar.bFirstPage.Show();
                    Menu.INSTANCE.mBottomBar.Show();
                    Menu.INSTANCE.mTopBar.Show();
                    Menu.INSTANCE.mCameraController.GunCamera1Shrink();
                    if(gameObj != null)
                    {
                        this.speed = Vector2.zero;
                        gameObj.transform.DOLocalRotate(Vector3.zero, 0.8f).SetEase(Ease.InOutQuad);
                    }
                }
            }
        }

        public override int TAG
        {
            get
            {
                return 1;
            }
        }
    }
}