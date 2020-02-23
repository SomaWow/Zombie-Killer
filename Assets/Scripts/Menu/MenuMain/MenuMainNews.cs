using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Menu
{
    public class MenuMainNews : MonoBehaviour
    {
        public Image newsImg;
        public Text titleText;
        public Text contentText;

        public GameObject[] dots;
        public string[] titles;
        public string[] contents;
        public Sprite[] imgs;

        private int currentIndex;
        private float updateTimer;
        private float updateInterval = 5;
        private bool isFromRight = true;

        private void Awake()
        {
            this.Check(0);
        }

        public void ShowPrevious()
        {
            this.isFromRight = false;
            this.currentIndex = ((this.currentIndex - 1) + 3) % 3;
            this.Check(this.currentIndex);
            this.updateTimer = 0;
        }

        public void ShowNext()
        {
            this.isFromRight = true;
            this.currentIndex = (this.currentIndex + 1) % 3;
            this.Check(this.currentIndex);
            this.updateTimer = 0;
        }

        public void Check(int index)
        {
            newsImg.sprite = imgs[index];
            titleText.text = titles[index];
            contentText.text = contents[index];
            if(isFromRight)
            {
                newsImg.rectTransform.localPosition = new Vector3(250f, 0f, 0f);
                newsImg.rectTransform.DOAnchorPosX(0f, 1f).SetEase(Ease.InQuint);
            }
            else
            {
                newsImg.rectTransform.localPosition = new Vector3(-250f, 0f, 0f);
                newsImg.rectTransform.DOAnchorPosX(0f, 1f).SetEase(Ease.InQuint);
            }

            for (int i = 0; i < dots.Length; i++)
            {
                if(i == index)
                {
                    dots[i].SetActive(true);
                }
                else
                {
                    dots[i].SetActive(false);
                }
            }
        }

        private void Update()
        {
            this.updateTimer += Time.deltaTime;
            if(this.updateTimer > this.updateInterval)
            {
                this.ShowNext();
                this.updateTimer = 0;
            }
        }
    }
}