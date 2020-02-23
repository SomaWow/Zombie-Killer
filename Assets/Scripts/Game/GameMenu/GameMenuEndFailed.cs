using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class GameMenuEndFailed : MonoBehaviour
    {

        public GameObject bloodBgObj;
        public GameObject clawObj;
        public GameObject continueObj;
        public GameObject countObj;
        public Text missionCountText;
        public Text killCountText;
        public Text headshotCountText;
        public Text totalCountText;
        public CameraShakeEffect shakeEffect;

        public AnimationCurve bloodCurve;
        
        private float timer;
        private bool isShaked;
        private int missionReward = 10;
        private int killReward = 12;
        private int headshotReward = 5;
        private int totalReward = 30;

        public void SetReward(int missionReward, int killReward, int headshotReward)
        {
            this.missionReward = missionReward;
            this.killReward = killReward;
            this.headshotReward = headshotReward;
            this.totalReward = this.missionReward + this.killReward + this.headshotReward;
        }

        void Update()
        {
            this.timer += Time.unscaledDeltaTime;
            if ((this.timer >= 0f) && !this.bloodBgObj.activeSelf)
            {
                this.bloodBgObj.SetActive(true);
                this.bloodBgObj.transform.localScale = new Vector3(2f,2f,0f);
                this.bloodBgObj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f).SetEase(bloodCurve);
                this.bloodBgObj.GetComponent<Image>().DOFade(1f, 0.2f);

            }
            if((this.timer >= 0.25f) && !this.countObj.activeSelf)
            {
                this.ShowCount();
            }
            if((this.timer >= 1.4f) && !this.isShaked)
            {
                this.isShaked = true;
                this.shakeEffect.StartShake(50f, 10f, 0f, 0.06f, 4f, 2);
            }
            if((this.timer >= 1.4f) && !this.clawObj.activeSelf)
            {
                this.clawObj.SetActive(true);
            }
            if((this.timer >= 3f) && !this.continueObj.activeSelf)
            {
                this.continueObj.SetActive(true);
            }
        }

        private void ShowCount()
        {
            this.countObj.SetActive(true);
            countObj.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
            // MissionCount
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                missionCountText.text = temp.ToString();
            }, 0f, (float)this.missionReward, 0.7f);
            
            // KillCount
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                killCountText.text = temp.ToString();
            }, 0f, (float)this.killReward, 0.7f);
            
            // HeadShotCount
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                headshotCountText.text = temp.ToString();
            }, 0f, (float)this.headshotReward, 0.7f);
            
            // TotalCount
            DOTween.To(delegate (float value)
            {
                //向下取整
                var temp = Mathf.Floor(value);
                //向Text组件赋值
                totalCountText.text = temp.ToString();
            }, 0f, (float)this.totalReward, 1f);

        }
    }
}