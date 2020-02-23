using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class GameMenuHeroBloodView : MonoBehaviour {

        public Image fillBottem;
        public Image fill;

        private float currentPercent = 1f;
        private float targetPercent = 1f;
        private float speed = 0.1f;

        void Start() {
            this.ResetPercent();
            this.currentPercent = this.targetPercent;
            fillBottem.fillAmount = this.currentPercent;
            fill.fillAmount = this.currentPercent;
        }

        void Update() {
            this.ResetPercent();
            this.fill.fillAmount = this.targetPercent;
            this.currentPercent = Mathf.Lerp(this.currentPercent, this.targetPercent, Time.unscaledDeltaTime);
            this.fillBottem.fillAmount = this.currentPercent;
        }

        private void ResetPercent()
        {
            this.targetPercent = HeroController.INSTANCE.hp / 100f;
        }
    }
}