using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Resource;

namespace Game
{
    public class GameItemBloodBoxEffect : MonoBehaviour
    {
        public ParticleSystem partSys;
        public Image img;
        private float timer;
        private bool playBack;

        private void Awake()
        {
            Debug.Log("awake");
            img.DOFade(0.8f, 0.2f);
        }

        public static void Show()
        {
            RectTransform rectTF = UnityEngine.Object.Instantiate(PrefabResources.Get("Effect/BloodBoxEffect")).GetComponent<RectTransform>();
            rectTF.parent = GameMenu.INSTANCE.effectPanelTrans;
            rectTF.offsetMin = Vector2.zero;
            rectTF.offsetMax = Vector2.zero;
            rectTF.localScale = Vector3.one;
            rectTF.gameObject.SetActive(true);
            Debug.Log(rectTF.localPosition);
        }

        private void Update()
        {
            this.timer += Time.unscaledDeltaTime;
            if (!this.playBack && this.timer >= 0.5f)
            {
                this.playBack = true;
                img.DOFade(0f, 2f);
            }
            if (this.timer >= 3f)
            {
                this.partSys.Stop();
            }
            if (this.timer > 4f)
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }
}