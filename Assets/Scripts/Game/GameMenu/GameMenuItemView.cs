using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameMenuItemView : MonoBehaviour
    {
        public Image overlayImg;
        [HideInInspector]
        public float cooldownTime = 5f;
        private float timer;
        private bool isCooling;

        public Button btn;
        

        void Start()
        {
            btn.onClick.AddListener(this.OnClick);
        }

        private void OnClick()
        {
            if (!this.isCooling)
            {
                this.timer = 0f;
                this.isCooling = true;
                this.btn.enabled = false;
                this.overlayImg.gameObject.SetActive(true);
                this.overlayImg.fillAmount = 1;
                this.Use();
            }
        }

        void Update()
        {
            if (this.isCooling)
            {
                this.timer += Time.unscaledDeltaTime;
                this.overlayImg.fillAmount = 1 - this.timer / this.cooldownTime;
                if (timer >= this.cooldownTime)
                {
                    this.isCooling = false;
                    this.btn.enabled = true;
                    this.overlayImg.gameObject.SetActive(false);
                }
            }
        }

        // 只有血包的效果
        public void Use()
        {
            HeroController.INSTANCE.hp += 50f;
            if(HeroController.INSTANCE.hp > 100f)
            {
                HeroController.INSTANCE.hp = 100f;
            }
            GameItemBloodBoxEffect.Show();
        }
    }
}