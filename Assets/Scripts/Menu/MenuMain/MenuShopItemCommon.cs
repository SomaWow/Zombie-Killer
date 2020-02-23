using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuShopItemCommon : MonoBehaviour
    {
        public int type;
        public int num;
        private Button btn;

        private void Awake()
        {
            btn = GetComponentInChildren<Button>();
            btn.onClick.AddListener(this.Click);
        }

        public void Click()
        {
            if (this.type == 0)
            {
                Menu.INSTANCE.mShop.GoldClick(this.num);
            }
            else
            {
                Menu.INSTANCE.mShop.GemClick(this.num);
            }
            AudioManager.INSTANCE.Play("UI/tab");
        }
    }
}