using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Resource;
namespace Game {
    public class ZombieAttackBlood : RealTimeScale
    {
        public float startScaleX = 0.5f;
        public float startScaleY = 0.25f;
        public float minScaleX = 1.5f;
        public float maxScaleX = 3.3f;
        public float minScaleY = 0.75f;
        public float maxScaleY = 1.3f;
        public float scaleMoveMul = 50f;
        private float timer;
        private bool isFading;

        private void Update()
        {
            if (!this.isFading)
            {
                this.timer += base.UpdateRealTimeDelta();
                if (this.timer >= 1.5f)
                {
                    this.isFading = true;
                    Tween tween = GetComponent<Image>().DOFade(0f, 2f).OnComplete(FadeEnd);
                    tween.SetUpdate(true);
                    tween.Play();
                }
            }
        }

        private void FadeEnd()
        {
            if (this.isFading)
            {
                Destroy(base.gameObject);
            }
        }

        public static void Show(string prefabName, Vector3 position, int dir, float angle)
        {
            RectTransform rectTF = Instantiate(PrefabResources.Get(prefabName)) as RectTransform;
            rectTF.parent = GameMenu.INSTANCE.effectPanelTrans;
            ZombieAttackBlood component = rectTF.GetComponent<ZombieAttackBlood>();
            float startScaleX = component.startScaleX;
            float startScaleY = component.startScaleY;
            float minScaleX = component.minScaleX;
            float maxScaleX = component.maxScaleX;
            float minScaleY = component.minScaleY;
            float maxScaleY = component.maxScaleY;
            float scaleMoveMul = component.scaleMoveMul;
            float x = Random.Range(minScaleX, maxScaleX);
            float y = Random.Range(minScaleY, maxScaleY);
            if(dir > 0)
            {
                angle += Random.Range(-5, 5);
                position += new Vector3((Mathf.Cos(angle) * x) * scaleMoveMul, (Mathf.Sin(angle) * y) * scaleMoveMul, 0f);
                rectTF.localPosition = position;
                rectTF.localEulerAngles = new Vector3(0f, 0f, angle);
                rectTF.localScale = new Vector3(startScaleX, startScaleY, 1f);
            }
            else if (dir < 0)
            {
                angle += UnityEngine.Random.Range(-5, 5);
                position += new Vector3((-Mathf.Cos(angle) * x) * scaleMoveMul, (Mathf.Sin(angle) * y) * scaleMoveMul, 0f);
                rectTF.localPosition = position;
                rectTF.localEulerAngles = new Vector3(0f, 0f, 180f - angle);
                rectTF.localScale = new Vector3(startScaleX, startScaleY, 1f);
            }
            else
            {
                angle += UnityEngine.Random.Range(-5, 5);
                position += new Vector3((-Mathf.Sin(angle) * y) * scaleMoveMul, (Mathf.Cos(angle) * x) * scaleMoveMul, 0f);
                rectTF.localPosition = position;
                rectTF.localEulerAngles = new Vector3(0f, 0f, 90f + angle);
                rectTF.localScale = new Vector3(startScaleX, startScaleY, 1f);
            }
            Image img = rectTF.GetComponent<Image>();
            Tween imgTween = img.DOFade(1, 0.5f);
            imgTween.SetUpdate(true);
            imgTween.Play();
            Tween rectTFTween = rectTF.DOScale(new Vector3(x, y, 1f), 0.3f);
            rectTFTween.SetUpdate(true);
            rectTFTween.Play();
            rectTF.transform.gameObject.SetActive(true);
        }
    }
}