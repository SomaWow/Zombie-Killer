using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Menu
{
    public class MenuAchivement : MenuAbs
    {
        // 组件
        private CanvasGroup canvasGroup;
        private Button closeButton;

        private Text descriptionText;
        private GameObject stampObj;

        private MenuAchivementItem[] items;
        private MenuAchivementItem checkedItem;

        
        public override int TAG
        {
            get
            {
                return 0x3e9;
            }
        }

        private void Awake()
        {
            InitComponent();

            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }

        private void InitComponent()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            closeButton = Find<Button>("closeButton");
            closeButton.onClick.AddListener(CloseClick);

            this.descriptionText = Find<Text>("Introduction/descriptionText");
            this.stampObj = transform.Find("Introduction/stamp").gameObject;

            items = GetComponentsInChildren<MenuAchivementItem>();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].checkItemDelegate = (Action<MenuAchivementItem>)Delegate.Combine(items[i].checkItemDelegate, new Action<MenuAchivementItem>(this.SetCheck));
            }

        }

        // 清空选择，点亮成就
        public void ResetItemsWithoutCheck()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                this.items[i].Refresh();
            }
            // 清空
            this.descriptionText.text = "";
            this.stampObj.SetActive(false);
        }

        private void SetCheck(MenuAchivementItem item)
        {
            this.checkedItem = item;
            for (int i = 0; i < this.items.Length; i++)
            {
                this.items[i].SetChecked(false);
            }
            item.SetChecked(true);
            this.descriptionText.text = item.desc;
            this.stampObj.SetActive(item.isReached);
        }

        public override void BackClick()
        {
            this.CloseClick();
        }

        private void CloseClick()
        {
            this.Hide();
            // 声音
            AudioManager.INSTANCE.Play("UI/button2");

        }

        public override void Show()
        {
            Debug.Log("+++ MenuAchivement.Show()");
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f);
        }

        public override void Hide()
        {
            Debug.Log("+++ MenuAchivement.Hide()");
            canvasGroup.DOFade(0, 0.2f).OnComplete(HideEnd);
        }
        private void HideEnd()
        {
            this.gameObject.SetActive(false);
        }
        
    }
}