using Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameWeaponController : MonoBehaviour
    {
        private const float doubleDmgMaxTime = 15f;
        private const float ammoBoxMaxTime = 20f;
        public GameInjection injection;
        private List<GameWeapon> weapons = new List<GameWeapon>();

        private int nowIndex;
        public int totalCount = 2;
        private int nowCount;
        private bool enableWeapon;

        public static string gun1Name = string.Empty;
        public static string gun2Name = string.Empty;
        public static string item1Name = string.Empty;
        public static string item2Name = string.Empty;

        public static float dmgReduce = 1f;
        public static float gun1Reduce = 1f;
        public static float gun2Reduce = 1f;
        public static string lastPlayedWeaponName = string.Empty;
        public static bool isDoubleDmgEffectOn;
        public static bool isDoubleDmgOn;
        public static float doubleDmgTimer;
        public static bool isAmmoBoxEffectOn;
        public static bool isAmmoBoxOn;
        private static float ammoBoxTimer;

        // 属性
        public GameWeapon nowWeapon
        {
            get
            {
                if (this.weapons.Count > 0)
                {
                    return this.weapons[this.nowIndex];
                }
                return null;
            }
        }
        // 初始化
        public void Init()
        {
            // 实例化第一支枪
            Transform gun1Trans = Instantiate(PrefabResources.Get("Gun/" + Profile.Gun1Name));
            gun1Trans.parent = GameCameraController.INSTANCE.gunNode;
            gun1Trans.localPosition = new Vector3(0,0,0);
            gun1Trans.localRotation = Quaternion.identity;
            gun1Trans.localScale = new Vector3(1f, 1f, 1f);
            gun1Trans.gameObject.SetActive(false);
            // 已经挂上了GameWeaponGun并进行了设置，所以直接获取即可
            GameWeaponGun gun1 = gun1Trans.gameObject.GetComponent<GameWeaponGun>();
            gun1.unequipEndDelegate = (Action)Delegate.Combine(gun1.unequipEndDelegate, new Action(this.WeaponUnequipEnd));
            Transform aimPoint1 = Instantiate(PrefabResources.Get("UI/" + Profile.Gun1Name + "_AimPoint"));
            aimPoint1.parent = GameMenu.INSTANCE.aimPointNode;
            aimPoint1.localScale = new Vector3(1f, 1f, 1f);
            gun1.aimPoint = aimPoint1.GetComponent<GameAimPointNormal>();
            weapons.Add(gun1);

            // 实例化第二支枪
            Transform gun2Trans = Instantiate(PrefabResources.Get("Gun/" + Profile.Gun2Name));
            gun2Trans.parent = GameCameraController.INSTANCE.gunNode;
            gun2Trans.localPosition = new Vector3(0, 0, 0);
            gun2Trans.localRotation = Quaternion.identity;
            gun2Trans.localScale = new Vector3(1f, 1f, 1f);
            gun2Trans.gameObject.SetActive(false);
            GameWeaponGun gun2 = gun2Trans.gameObject.GetComponent<GameWeaponGun>();
            gun2.unequipEndDelegate = (Action)Delegate.Combine(gun2.unequipEndDelegate, new Action(this.WeaponUnequipEnd));
            Transform aimPoint2 = Instantiate(PrefabResources.Get("UI/" + Profile.Gun2Name + "_AimPoint"));
            aimPoint2.parent = GameMenu.INSTANCE.aimPointNode;
            aimPoint2.localScale = new Vector3(1f, 1f, 1f);
            gun2.aimPoint = aimPoint2.GetComponent<GameAimPointNormal>();
            weapons.Add(gun2);

            this.nowIndex = 0;
            this.nowCount = 2;

            // 画布上的按钮挂上委托，使可攻击、攻击结束、换子弹、换枪
            InstantiateAllEnd();
        }

        private void InstantiateAllEnd()
        {
            // 按下攻击键
            GameMenu.INSTANCE.gameController.fireDown = (Action)Delegate.Combine(GameMenu.INSTANCE.gameController.fireDown, new Action(this.FireDown));
            // 抬起攻击键
            GameMenu.INSTANCE.gameController.fireRelease = (Action)Delegate.Combine(GameMenu.INSTANCE.gameController.fireRelease, new Action(this.FireUp));
            // 上子弹
            GameMenu.INSTANCE.gameController.reloadClick = (Action)Delegate.Combine(GameMenu.INSTANCE.gameController.reloadClick, new Action(this.ReloadClick));
            // 换枪
            GameMenu.INSTANCE.gameController.changeWeaponClick = (Action)Delegate.Combine(GameMenu.INSTANCE.gameController.changeWeaponClick, new Action(this.ChangeWeapon));
            // 道具1
            // 道具2

        }

        public void EnableWeapon()
        {
            this.enableWeapon = true;
            if (this.nowWeapon != null)
                this.ShowCurrentWeapon();
        }

        private void ShowCurrentWeapon()
        {
            this.nowWeapon.gameObject.SetActive(true);
            this.nowWeapon.Reset();
            lastPlayedWeaponName = this.nowWeapon.weaponName;
        }

        private void WeaponUnequipEnd()
        {
            this.nowIndex = (this.nowIndex + 1) % this.weapons.Count;
            this.ShowCurrentWeapon();
        }


        private void FireDown()
        {
            if(this.nowWeapon != null)
            {
                this.nowWeapon.FireDown();
            }
        }

        private void FireUp()
        {
            if(this.nowWeapon != null)
            {
                this.nowWeapon.FireUp();
            }
        }

        private void ReloadClick()
        {
            if(this.nowWeapon != null)
            {
                this.nowWeapon.ReloadClick();
            }
        }

        private void ChangeWeapon()
        {
            Debug.Log("GameWeaponController.ChangeWeapon");
            if(this.nowWeapon != null)
            {
                this.nowWeapon.ChangeClick();
            }
        }
    }
}