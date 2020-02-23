using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameWeapon : RealTimeScale
    {
        public const string MuzzleFlashNodeName = "muzzle";
        public const string ShellNodeName = "shell";
        public const float AimTweenerTime = 0.3f;
        public const float RunTweenerTime = 0.45f;
        public const float DownTweenerTime = 0.3f;
        public const float NormalFov = 45f;

        public string weaponName;
        public int gunType;
        // 组件
        protected Animation anim;
        public MuzzleFlash muzzleFlash;
        public GameScopeAbs scope; // 透视镜瞄准
        public GameScopeAbs aimPoint; // 普通准星
        // 基本参数
        public float dmgReduce = 1f;
        public float baseDmg = 1f;
        public float baseFrate = 0.35f;
        public float basePower = 0.5f;
        public float baseZoomRate = 1.1f;
        public float baseReloadTime = 2f;
        // 状态
        public Sprite uiGunSprite;
        protected bool isComponentIntialized;
        protected bool scopeInitialized;
        public Vector3 normalPos;
        public Vector3 normaoRot;
        public Vector3 aimPos;
        public Vector3 aimRot;
        public float aimFov = 30f;
        public float aimGunFov = 30f;
        public Vector3 runPos;
        public Vector3 runRot;
        public Vector3 downPos;
        public Vector3 downRot;
        public bool isAimed;
        public bool isFiring;
        public bool canBeAimed = true;
        protected Transform[] childTrans;
        protected float timer;
        public bool isRunning;
        public int currentBullet = 180;
        public int currentClipBullet = 30;
        public int clipCapacity = 30;
        public float fireForce = 1f;
        public float fireForceAimed = 0.5f;

        public Action unequipEndDelegate;
        
        public virtual void Reset()
        {
            if(!this.isComponentIntialized)
            {
                this.anim = base.GetComponent<Animation>();
                muzzleFlash = this.transform.FindChildByName("MuzzleLight").GetComponent<MuzzleFlash>();
                
            }
            // 设置火光
            // 设置准星
            // 设置枪的各种属性
            this.isComponentIntialized = true;
        }

        public virtual void FireDown() { }
        public virtual void FireUp() { }
        public virtual void ReloadClick() { }
        public virtual void ReloadEnd()
        {
            if(this.currentBullet >= this.clipCapacity)
            {
                this.currentClipBullet = this.clipCapacity;
            }
            else
            {
                this.currentClipBullet = this.currentBullet;
            }
        }
        public virtual void ChangeClick() { }
    }
}