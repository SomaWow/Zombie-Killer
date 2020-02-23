using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameCameraController : RealTimeScale {

    public static GameCameraController INSTANCE;
    private const float NormalFov = 45f;
    private const float AimFov = 30f;
    // 摄像机们
    public Camera sceneCamera;
    public Camera gunCamera;
    public Camera effectCamera;
    public Camera sceneBgCamera;
    // 开枪效果
    public GameCameraFireRot fireRot; // 挂在主相机上
    public GameGunFireShake mainFireShake; 
    public GameGunFireShake fireShake; // 挂在枪上
    // 移动效果
    public CameraShakeEffect cameraShake; // 挂在主相机上
    public CameraShakeEffect slowMotionShake;
    public CameraShakeEffect gunShake; // 挂在枪挂的地方，可能在走路的时候控制枪的摆动

    public Transform gunNode;

    public GameObject cameraObj;

    // 主摄像头移动参数
    private bool canMove = true;
    private bool isVerticalLocked;
    private bool isHorizontalLocked;
    private Vector2 speed = Vector2.zero;
    private float baseMaxSpeed = 4f;
    private float rotMul = 0.5f;
    public float settingMul = 1f;
    private float smoothBaseAcc = 0.5f;
    private float smoothResistance = 5000f;
    private static float sensitiveMul = (854f / (float) Screen.width);
    private float sensitiveX = (1.2f * sensitiveMul);
    private float sensitiveY = (0.65f * sensitiveMul);
    public float verticalMinRot = -60f;
    public float verticalMaxRot = 60f;
    private Vector3 upLocalPos;
    private Vector3 upLocalAngle;

    public GameCameraType camType = GameCameraType.SMOOTH;

    private Transform _cachedMainCameraTransform;

    private void Awake()
    {
        INSTANCE = this;
        // 场景摄像机
        this._cachedMainCameraTransform = this.sceneCamera.transform;
        this.sceneCamera.fieldOfView = 45f;
        //GameObject obj2 = new GameObject("_CameraFocus");
        //obj2.AddComponent<CameraFocus>().cameraPrefab = this.cameraObj;
        //GameObject obj3 = new GameObject("_CameraDragFocus");
        //obj3.AddComponent<CameraDragFocus>().cameraPrefab = this.cameraObj;
        this.sensitiveX *= Mathf.Lerp(0.5f, 1.5f, Profile.sensitive / 2f);
        this.sensitiveY *= Mathf.Lerp(0.5f, 1.5f, Profile.sensitive / 2f);
    }

    private void Start()
    {
        GameMenu.INSTANCE.touchPad.onDrag = (Action<Vector2>)Delegate.Combine(GameMenu.INSTANCE.touchPad.onDrag, new Action<Vector2>(this.OnCameraTouchMove));
    }

    // 受力，主相机受力效果
    public void ApplyForce(Vector3 force)
    {
        this.fireRot.ApplyForce(force);
    }
    
    private void OnCameraTouchMove(Vector2 delta)
    {
        if(this.canMove)
        {
            delta.y = -delta.y;
            if(this.isVerticalLocked)
            {
                delta.y = 0;
            }
            if(this.isHorizontalLocked)
            {
                delta.x = 0;
            }
            switch(this.camType)
            {
                case GameCameraType.NOSMOOTH:
                    this.speed += (delta * this.rotMul) * this.settingMul;
                    break;
                case GameCameraType.SMOOTH:
                    float num = 0f;
                    if(this.sceneCamera.enabled)
                    {
                        // 和视角相关
                        num = this.sceneCamera.fieldOfView / 45f;
                    }
                    this.speed += ((delta * this.smoothBaseAcc) * this.settingMul) * num;
                    float num2 = 0.1f;
                    this.MainCameraTrans.localEulerAngles += new Vector3(
                        ((delta.y * sensitiveY) * num2) * num,
                        ((delta.x * sensitiveX) * num2) * num,
                        0f);

                    break;
            }
            // 限制垂直方向的转动
            Vector3 localEulerAngles = this.MainCameraTrans.localEulerAngles;
            localEulerAngles.x = Mathf.Clamp(ClampAngle(localEulerAngles.x), verticalMinRot, verticalMaxRot);
            this.MainCameraTrans.localEulerAngles = localEulerAngles;
            // 限制最大速度
            float num3 = this.baseMaxSpeed * this.settingMul;
            float magnitude = this.speed.magnitude;
            if(magnitude > num3)
            {
                this.speed *= num3 / magnitude;
            }
            // 修改准星
            // 测试
            if((HeroController.INSTANCE.weaponController.nowWeapon != null) && (HeroController.INSTANCE.weaponController.nowWeapon.aimPoint != null))
            {
                float f = delta.magnitude;
                // 测试
                HeroController.INSTANCE.weaponController.nowWeapon.aimPoint.Shoot(Mathf.Sqrt(f) / 20f);
            }
        }
    }

    public void TweenToGround(Vector3 hitDirection)
    {
        hitDirection.y = 0f;
        float duration = 2f;
        // 位置动画
        Tween positionTween = this.MainCameraTrans.DOLocalMove(new Vector3(0f, -1.2f, -3f), duration);
        positionTween.SetEase(Ease.OutBounce);
        positionTween.Play();
        // 转向动画
        this.upLocalAngle = this.MainCameraTrans.localEulerAngles;
        this.MainCameraTrans.forward = -hitDirection;
        Vector3 localEulerAngles = this.MainCameraTrans.localEulerAngles;
        localEulerAngles.x = 0f;
        this.MainCameraTrans.localEulerAngles = this.upLocalAngle;
        Tween rotationTween = this.MainCameraTrans.DOLocalRotate(localEulerAngles, duration);
        rotationTween.SetEase(Ease.OutCirc);
        rotationTween.Play();
    }

    // 工具方法
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
    // 属性
    public Transform MainCameraTrans
    {
        get { return this._cachedMainCameraTransform; }
    }
}
