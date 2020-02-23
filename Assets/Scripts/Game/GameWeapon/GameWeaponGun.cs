using Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameWeaponGun : GameWeapon
    {
        private const float lightTime = 0.15f;
        private State state;
        private float lightTimer;

        private float fireSpeed = 1f;
        private float reloadSpeed = 1f;
        private float equipSpeed = 1.25f;
        private float unequipSpeed = 1.25f;
        private float fireTime;
        private float reloadTime;
        private float equipTime;
        private float unequipTime;
        private float lastFireTime;
        private float fireInterval;
        private bool isIgnoreTimeScale = true;
        private float deltaTime;

        public override void Reset()
        {
            base.Reset();
            base.timer = 0f;
            this.fireTime = base.anim["SHOOT"].length / this.fireSpeed;
            this.fireInterval = base.anim["SHOOT"].length / this.fireSpeed;
            this.reloadTime = base.anim["RELOAD"].length / this.reloadSpeed;
            this.unequipTime = base.anim["UNEQUIP"].length / this.unequipSpeed;
            this.equipTime = base.anim["EQUIP"].length / this.equipSpeed;
            this.Equip();
        }

        private void Update()
        {
            // 当时间停滞
            if((Time.timeScale <= 0f) && (RealTimeScale.timeScale <= 0))
            {
                //
            }
            else
            {
                if(this.isIgnoreTimeScale)
                {
                    // 不受时间缩放的影响，使用真实的帧
                    this.deltaTime = base.UpdateRealTimeDelta();
                }
                else
                {
                    this.deltaTime = Time.deltaTime;
                }
                this.lightTimer -= this.deltaTime;
                this.lightTimer = Mathf.Max(0, this.lightTimer);
                if(this.lightTimer > 0f)
                {
                    
                }
                switch(this.state)
                {
                    case State.EQUIP:
                        this.ProcessEquip();
                        break;
                    case State.IDLE:
                        this.ProcessIdle();
                        break;
                    case State.FIRE:
                        this.ProcessFire();
                        break;
                    case State.RELOAD:
                        this.ProcessReload();
                        break;
                    case State.UNEQUIP:
                        this.ProcessUnequip();
                        break;
                }
            }
        }




        public override void FireDown()
        {
            base.isFiring = true;
            if(this.CanFire)
            {
                // 判断是否有子弹
                if(base.currentBullet > 0)
                {
                    this.state = State.FIRE;
                }
                else
                {
                    AudioManager.INSTANCE.Play("Gun/block");
                }
            }
        }
        public override void FireUp()
        {
            base.isFiring = false;
            if (this.state == State.FIRE)
            {
                this.state = State.IDLE;
            }
        }

        // 换子弹
        public override void ReloadClick()
        {
            if(this.CanReload && (((base.currentBullet - base.currentClipBullet) > 0) && (base.currentClipBullet > 0)))
            {
                this.Reload();
            }
        }

        public override void ChangeClick()
        {
            Debug.Log("GameWeaponGun.ChangeClick");
            if (this.CanChange)
            {
                this.Change();
            }
        }

        private void Reload()
        {
            //...
            AudioManager.INSTANCE.Play("Gun/" + base.weaponName + "_Reload");
            //...
            base.timer = 0f;
            base.isAimed = false;
            base.isFiring = false;
            this.state = State.RELOAD;
        }

        private void ProcessEquip()
        {
            base.timer += this.deltaTime;
            if (base.timer >= this.equipTime)
            {
                if(base.isFiring)
                {
                    this.state = State.FIRE;
                }
                else
                {
                    this.state = State.IDLE;
                }
            }
            else if(Time.timeScale <= 0)
            {
                base.anim["EQUIP"].speed = 0f;
            }
            else
            {
                // 保证换枪时间不受时间缩放的影响，但是用不上，因为没有打针的能力。。
                base.anim["EQUIP"].speed = this.equipSpeed * (1f / Time.timeScale);
                // 进行到一般时可以出现准星
                if (base.anim["EQUIP"].normalizedTime > 0.5F)
                {
                    base.aimPoint.Show();
                }
            }
        }

        private void ProcessIdle()
        {
            if ((Time.realtimeSinceStartup - this.lastFireTime) >= this.fireTime)
            {
                base.anim.CrossFade("IDLE");
            }
            if (base.isFiring)
            {
                this.state = State.FIRE;
            }
        }

        private void ProcessFire()
        {
            if(this.CanFire)
            {
                if (base.currentClipBullet > 0)
                {
                    base.currentBullet--;
                    base.currentClipBullet--;
                    this.Fire();
                }
                else if (base.currentBullet > 0)
                {
                    this.Reload();
                }
                else
                {
                    base.isFiring = false;
                }
            }
            // 在进行下次射击之前检测还有木有子弹
            else if(((Time.realtimeSinceStartup - this.lastFireTime) >= (this.fireInterval / 2f)) && (base.currentClipBullet <= 0))
            {
                if(base.currentBullet > 0)
                {
                    this.Reload();
                }
                else
                {
                    base.isFiring = false;
                }
            }
        }

        private void ProcessReload()
        {
            base.timer += this.deltaTime;
            if (base.timer >= this.reloadTime * 0.9f)
            {
                this.ReloadEnd();
            }
            if (base.timer >= this.reloadTime) // 当时间到达0.9，就可以开火
            {
                this.ReloadEnd();
                if (base.isFiring)
                {
                    this.state = State.FIRE;
                }
                else
                {
                    this.state = State.IDLE;
                }
            }
            else
            {
                if (Time.timeScale <= 0)
                {
                    base.anim["RELOAD"].speed = 0f;
                }
                else
                {
                    base.anim["RELOAD"].speed = this.reloadSpeed * (1f / Time.timeScale);
                }
                base.anim.Play("RELOAD");
            }
        }

        private void ProcessUnequip()
        {
            base.timer += this.deltaTime;
            if(base.timer >= this.unequipTime)
            {
                if(this.unequipEndDelegate != null)
                {
                    Debug.Log("ProcessUnequip");
                    unequipEndDelegate();
                    base.gameObject.SetActive(false);
                }
            }
        }

        private void Equip()
        {
            base.anim.Play("EQUIP");
            this.state = State.EQUIP;
            if(base.aimPoint != null)
            {
                base.aimPoint.Show();
            }
        }

        private void Change()
        {
            base.aimPoint.Hide();
            base.isFiring = false;
            base.timer = 0;
            this.state = State.UNEQUIP;
            base.anim.CrossFade("UNEQUIP", 0.3f);
        }

        private void Fire()
        {
            this.lightTimer = 0.15f;
            // ?
            base.timer = 0f;
            if(this.isIgnoreTimeScale)
            {
                this.lastFireTime = Time.realtimeSinceStartup;
            }
            else
            {
                this.lastFireTime = Time.time;
            }
            Debug.Log("fire");
            base.anim.Stop();
            base.anim.Play("SHOOT");

            // 初始化方位
            Vector3 mainCameraForward = GameCameraController.INSTANCE.MainCameraTrans.forward;
            Vector3 mainCameraRight = GameCameraController.INSTANCE.MainCameraTrans.right;
            Vector3 mainCameraUp = GameCameraController.INSTANCE.MainCameraTrans.up;
            float num = 0.01f;
            // 测试
            //if(base.isAimed)
            //{
            //    num = 0f;
            //}
            //else if((base.aimPoint != null) && (base.aimPoint is GameAimPointNormal))
            //{

            //}
            float num2 = Random.Range(-1f, 1) * num;
            float num3 = Random.Range(-1f, 1) * num;
            // 微小的偏移
            Vector3 direction = (mainCameraForward + (num2 * mainCameraRight)) + (num3 * mainCameraUp);
            Ray ray = new Ray(GameCameraController.INSTANCE.MainCameraTrans.position, direction);
            GunBulletBase gunBulletBaseComp = null;
            RaycastHit hit;
            // 射线检测，少了layer参数，待补
            if(Physics.Raycast(ray, out hit, 1000))
            {
                Transform trans;
                // 待补
                trans = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletNormal")) as Transform;
                // 起始位置为枪口火花处
                trans.position = base.muzzleFlash.transform.position;
                // 设置上面挂着的GunBulletBase对象
                gunBulletBaseComp = trans.GetComponent<GunBulletBase>();
                gunBulletBaseComp.baseDmg = base.baseDmg;
                gunBulletBaseComp.basePower = base.basePower;
                gunBulletBaseComp.SetTarget(hit);
            }
            else // 如果没有检测到
            {
                Debug.Log("没检测到目标，向前跑500m");
                Transform trans2;
                // 待补
                trans2 = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletNormal")) as Transform;
                trans2.position = base.muzzleFlash.transform.position;
                gunBulletBaseComp = trans2.GetComponent<GunBulletBase>();
                gunBulletBaseComp.SetTarget(base.muzzleFlash.transform.position + (mainCameraForward * 500));
            }


            // 开枪震动效果
            GameCameraController.INSTANCE.fireRot.ApplyFire(base.fireForce * 5f);
            GameCameraController.INSTANCE.fireShake.ApplyFire(base.fireForce * 10, base.fireForce * 0.1f);
            base.aimPoint.Shoot(base.fireForce);

            if(base.muzzleFlash != null)
            {
                muzzleFlash.SetFire();
            }
            if(Time.timeScale < 0.5f)
            {
                // 受时间影响的声音
            }
            else
            {
                AudioManager.INSTANCE.Play("Gun/" + base.weaponName + "_Shoot", 0.6f);
            }
        }
        // 补间动画
        private void TweenTo(Vector3 position, Vector3 angles, float fov, float time, string message)
        {

        }


        private bool CanFire
        {
            get
            {
                float num = (!this.isIgnoreTimeScale ? Time.time : Time.realtimeSinceStartup) - this.lastFireTime;
                return ((((num >= this.fireInterval) && (this.state != State.RELOAD)) && (this.state != State.EQUIP)) && (this.state != State.UNEQUIP));
            }
        }

        private bool CanReload
        {
            get {
                return (((this.state != State.EQUIP) && (this.state != State.UNEQUIP)) && this.state != State.RELOAD);
            }
        }

        private bool CanChange
        {
            get
            {
                return ((this.state != State.EQUIP) && (this.state != State.UNEQUIP));
            }
        }
    }

    

    public enum State
    {
        IDLE, // 默认
        FIRE, // 开枪
        RELOAD, // 换子弹
        UNEQUIP, // 卸下装备
        EQUIP, // 装备
        AIM // 瞄准
    }
}