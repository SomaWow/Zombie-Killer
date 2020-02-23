using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameMenuTopbarGun : MonoBehaviour
    {
        public Image gunImg;
        public Text bulletCountText;
        private GameWeapon current;

        void Update()
        {
            GameWeapon nowWeapon = HeroController.INSTANCE.weaponController.nowWeapon;
            if(nowWeapon == null)
            {
                this.bulletCountText.text = string.Empty;
            }
            else
            {
                this.bulletCountText.text = nowWeapon.currentClipBullet + "/" + (nowWeapon.currentBullet - nowWeapon.currentClipBullet);
            }
            
            if(this.current != nowWeapon)
            {
                this.current = nowWeapon;
                this.gunImg.sprite = nowWeapon.uiGunSprite;
                this.gunImg.SetNativeSize();
                this.gunImg.gameObject.SetActive(true);
            }
        }
        
    }
}