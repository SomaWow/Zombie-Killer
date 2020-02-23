using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuMissionInfo : MonoBehaviour
    {
        public GameObject leftArrowButton;
        public GameObject rightArrowButton;
        public Text missionIndexText;
        public Text missionInfoText;
        public Text rewardText;
        public MissionDiffStars diffStars;
        public static int selectedPoint;
        [HideInInspector]
        public StgDataConfigure stgConfig;
        
        // 点击左按钮
        public void MissionLeftClick()
        {
            MenuMissionInfo.selectedPoint--;
            AudioManager.INSTANCE.Play("UI/button2");
            ResetState();
        }
        // 点击右按钮
        public void MissionRightClick()
        {
            MenuMissionInfo.selectedPoint++;
            AudioManager.INSTANCE.Play("UI/button2");
            ResetState();
        }

        public void ResetState()
        {
            GameWeaponController.gun1Name = Profile.Gun1Name;
            GameWeaponController.gun2Name = Profile.Gun2Name;
            GameWeaponController.item1Name = Profile.Item1Name;
            GameWeaponController.item2Name = Profile.Item2Name;

            stgConfig = StgDataPointResource.INSTANCE.GetStgDataConfig(selectedPoint);
            int total = StgDataPointResource.INSTANCE.stgConfigList.Count;
            if (MenuMissionInfo.selectedPoint <= 0)
            {
                this.leftArrowButton.SetActive(false);
            }
            else
            {
                this.leftArrowButton.SetActive(true);
            }

            if(MenuMissionInfo.selectedPoint >= (total - 1))
            {
                this.rightArrowButton.SetActive(false);
            }
            else
            {
                this.rightArrowButton.SetActive(true);
            }

            this.missionIndexText.text = (MenuMissionInfo.selectedPoint + 1) + "/" + total;
            this.missionInfoText.text = this.stgConfig.description;
            this.diffStars.SetStars(this.stgConfig.hardness);
            this.rewardText.text = this.stgConfig.baseReward.ToString();
        }
        
    }
}