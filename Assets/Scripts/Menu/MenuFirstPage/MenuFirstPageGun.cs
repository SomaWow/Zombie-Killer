using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuFirstPageGun : MonoBehaviour
    {
        public Text titleText;
        public Image gunImg;
        public GameObject selectedObj;
        [HideInInspector]
        public MenuWeaponLoader weaponLoader;
        [HideInInspector]
        public string weaponName = string.Empty;
        [HideInInspector]
        public bool isChecked;
        [HideInInspector]
        public bool isEquiped = true;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(this.Select);
        }

        public void Select()
        {
            Menu.INSTANCE.mFirstPage.SetWeapon(this);
            AudioManager.INSTANCE.Play("UI/tab");
        }

        public void ShowWeapon(string name)
        {
            this.weaponName = name;
            this.weaponLoader = MenuWeaponLoader.GetLoader(name);
            this.titleText.text = this.weaponName.ToUpper();
            gunImg.sprite = this.weaponLoader.gunSprite;
            gunImg.SetNativeSize();
        }

        public void SetChecked(bool isChecked)
        {
            this.isChecked = isChecked;
            if(isChecked)
            {
                this.selectedObj.SetActive(true);
                this.titleText.color = Color.white;
            }
            else
            {
                this.selectedObj.SetActive(false);
                this.titleText.color = new Color(0.65f, 0.65f, 0.65f);
            }
        }
    }
}