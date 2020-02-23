using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class GameAimPointNormal : GameScopeAbs
    {
        public CanvasGroup canvasGroup;
        public Transform offsetTrans;
        public Transform[] arrowTrans;
        
        private float scale = 1f;
        private float speed = 1f;
        private float scaleUp = 0.02f;
        private float arrowOffset = 10f;
        private float arrowScale = 1f;
        private float arrowScaleUp = 4f;
        protected bool isScopeEffectOn;

        private void Update()
        {
            float delta = base.UpdateRealTimeDelta();
            this.SetScale();
            this.arrowScale -= ((delta * this.speed) * this.arrowScale) * this.arrowScale;
            this.arrowScale = Mathf.Max(this.arrowScale, 1f);
        }

        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            if(this.isScopeEffectOn)
            {
                this.isScopeEffectOn = false;
                
            }
        }

        public override void Show()
        {
            canvasGroup.DOFade(1f, 0.2f);
            this.scale = 1f;
            this.arrowScale = 1f;
            this.SetScale();
            base.gameObject.SetActive(true);
        }
        public override void Hide()
        {
            this.gameObject.SetActive(false);
        }
        // 设置准星的比例大小，位置
        private void SetScale()
        {
            for(int i = 0; i < this.arrowTrans.Length; i++)
            {
                this.arrowTrans[i].localPosition = new Vector3(0f, (this.arrowScale - 1f) * 20f, 0f);
            }
        }

        public override void Shoot(float fireForce)
        {
            this.arrowScale += fireForce * 3f;
        }
    }
}