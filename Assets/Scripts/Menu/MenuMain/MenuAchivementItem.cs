using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuAchivementItem : MonoBehaviour
    {
        private GameObject chosenObj;
        private GameObject reachedObj;
        
        public int achiveType;
        public int achiveCount;
        public string desc;
        [HideInInspector]
        public bool isReached;
        [HideInInspector]
        public bool isChecked;
        public Action<MenuAchivementItem> checkItemDelegate;

        private void Awake()
        {
            this.chosenObj = transform.Find("selectedBorder").gameObject;
            this.reachedObj = transform.Find("successIcon").gameObject;
            GetComponent<Button>().onClick.AddListener(this.ClickItem);
        }
        private void Start()
        {
            Debug.Log("Start" + reachedObj);
        }

        public void ClickItem()
        {
            if(!this.isChecked)
            {
                if(checkItemDelegate != null)
                {
                    checkItemDelegate(this);
                }
                AudioManager.INSTANCE.Play("UI/tab");
            }
        }

        public void SetChecked(bool isChecked)
        {
            this.isChecked = isChecked;
            this.chosenObj.SetActive(isChecked);
        }

        public void Refresh()
        {
            this.reachedObj.SetActive(false);
            int killCount = 0;
            switch (this.achiveType)
            {
                case 0: // kill
                    killCount = Profile.killCount;
                    break;
                case 1: // headshot
                    killCount = Profile.headshotCount;
                    break;
                case 2: // no hurt
                    killCount = Profile.noHurtMissionCount;
                    break;
            }
            if(killCount >= this.achiveCount)
            {
                this.reachedObj.SetActive(true);
                this.isReached = true;
            }
        }
    }
}