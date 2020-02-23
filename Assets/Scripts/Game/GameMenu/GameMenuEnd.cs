using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Menu;

namespace Game
{
    public class GameMenuEnd : MonoBehaviour
    {
        public Image bgImg;
        public Image overlayImg;

        public GameObject succeedObj;
        public GameObject failedObj;

        private bool isSuccess = true;
        private float timer;
        private int missionReward;
        private int killReward;
        private int headshotReward;


        private void Update()
        {
            this.timer += Time.unscaledDeltaTime;
            if (this.timer >= 0.15f)
            {
                //if ((this.isSuccess || (GameStgLoader.missionName == CMissionName.ACTIVITY)) && !this.succeedObj.activeSelf)
                if (this.isSuccess && !this.succeedObj.activeSelf)
                {
                    AudioManager.INSTANCE.Play("Bg/win");
                    this.succeedObj.SetActive(true);
                    this.DisableAll();
                }
                //if ((!this.isSuccess && (GameStgLoader.missionName != CMissionName.ACTIVITY)) && !this.failedObj.activeSelf)
                if (!this.isSuccess && !this.failedObj.activeSelf)
                {
                    AudioManager.INSTANCE.Play("Bg/lose");
                    this.failedObj.SetActive(true);
                    this.DisableAll();
                }
            }
        }

        public void Show(bool success)
        {
            this.isSuccess = success;
            base.gameObject.SetActive(true);
            // 屏幕变黑动画
            bgImg.DOFade(0.95f, 0.15f);
            
            // 结算金钱
            if(this.isSuccess)
            {
                int baseReward = GameStgLoader.stgConfig.baseReward;
                this.missionReward = baseReward;
                this.killReward = StgZombieBornDealer.normalKillCount * 5;
                this.headshotReward = StgZombieBornDealer.normalHeadshotCount * 10;
                this.succeedObj.GetComponent<GameMenuEndSucceed>().SetReward(missionReward, killReward, headshotReward);
            }
            else
            {
                this.missionReward = 0;
                this.killReward = StgZombieBornDealer.normalKillCount * 5;
                this.headshotReward = StgZombieBornDealer.normalHeadshotCount * 10;
                this.failedObj.GetComponent<GameMenuEndFailed>().SetReward(missionReward, killReward, headshotReward);
            }
            Profile.UpdateGold(this.missionReward + this.killReward + this.headshotReward);

            // 更新统计数据
            this.ResetProfile();
        }
        // 更新数据
        private void ResetProfile()
        {
            Menu.Menu.isFromGame = true;
            Profile.killCount += StgZombieBornDealer.normalKillCount;
            Profile.headshotCount += StgZombieBornDealer.normalHeadshotCount;

            Debug.Log("kill count " + StgZombieBornDealer.normalKillCount);
            Debug.Log("headshot count " + StgZombieBornDealer.normalHeadshotCount);
            Debug.Log("Profile.killCount " + Profile.killCount);
            Debug.Log("Profile.headshotCount " + Profile.headshotCount);

            if(HeroController.INSTANCE.hp >= 100f)
            {
                Profile.noHurtMissionCount++;
            }
            Profile.UpdateAchievement();
            PlayerPrefs.Save();
        }

        // 无效所有僵尸
        private void DisableAll()
        {
            for (int i = 0; i < StgZombieBornDealer.zombies.Count; i++)
            {
                StgZombieBornDealer.zombies[i].gameObject.SetActive(false);
            }
        }

        public void ContinueClick()
        {
            this.Hide();
        }

        public void Hide()
        {
            this.overlayImg.gameObject.SetActive(true);
            this.overlayImg.DOFade(1f, 1f).OnComplete(HideEnd);
        }

        public void HideEnd()
        {
            Debug.Log("_gameUnloader");
            SceneManager.LoadScene("_gameUnloader");
        }
    }
}