using Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    // 僵尸受到伤害溅到的血
    public class ZombieAttackedBlood : RealTimeScale
    {
        private const float startScale = 0.1f;
        private const float minScale = 0.5f;
        private const float maxScale = 1.5f;
        private const float minDist = 1.5f;
        private const float maxDist = 6f;
        private const float uiRange = 0.75f;
        private const float fadeDelay = 1.5f;
        private const float fadeDuration = 2f;

        public CanvasGroup canvasGroup;
        public Transform sprite1Trans;
        public Transform sprite2Trans;
        
        private float timer;
        private bool isFading;

        public void FadeEnd()
        {
            if(this.isFading)
            {
                Destroy(base.gameObject);
            }
        }

        public static void Show(string prefabName, float hitPointDist)
        {
            if(hitPointDist < 6f)
            {
                hitPointDist = Mathf.Clamp(hitPointDist, 1.5f, 6f);
                // 获得距离的比例
                float t = 1f - ((hitPointDist - 1.5f) / 4.5f);
                float x = Mathf.Lerp(0.5f, 1.5f, t) + Random.Range((float)-0.5f, (float)0.5f);
                // 计算位置
                float num3 = ((float) GameMenu.INSTANCE.GetComponent<CanvasScaler>().referenceResolution.x / ((float)Screen.height));
                float num4 = Screen.width * num3;
                float num5 = Screen.height * num3;
                float max = (num4 * 0.75f) / 3f;
                float num7 = (num5 * 0.75f) / 3f;
                // 实例化
                RectTransform rectTF = Instantiate(PrefabResources.Get(prefabName)) as RectTransform;
                // 放到画布下面
                rectTF.parent = GameMenu.INSTANCE.effectPanelTrans;
                ZombieAttackedBlood attackedBlood = rectTF.GetComponent<ZombieAttackedBlood>();
                // 设置位置，旋转的角度
                rectTF.localPosition = new Vector3(Random.Range(-max, max), Random.Range(-num7, num7), 0);
                attackedBlood.sprite1Trans.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
                attackedBlood.sprite2Trans.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
                rectTF.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                // 开始动画，透明度、尺寸、位置，尺寸根据距离远近变化
                attackedBlood.canvasGroup.DOFade(1f, 0.2f); // 显示
                rectTF.DOScale(new Vector3(x,x,1f), 0.2f);
                rectTF.DOLocalMoveY((rectTF.localPosition.y - 25f), 1.5f).SetEase(Ease.OutCubic);
                // 可见
                rectTF.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if(!this.isFading)
            {
                this.timer += base.UpdateRealTimeDelta();
                if(this.timer >= 1.5f)
                {
                    this.isFading = true;
                    this.canvasGroup.DOFade(0f, 2f).OnComplete(FadeEnd);
                }
            }
        }
    }
}