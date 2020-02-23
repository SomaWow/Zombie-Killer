using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Menu
{
    public class MenuMainNewsContent : MonoBehaviour, IDragHandler, IPointerUpHandler
    {
        public RectTransform rectTF;
        private float lastCheckTime;

        private float moveX;

        public void OnDrag(PointerEventData eventData)
        {
            this.moveX += eventData.delta.x;
            rectTF.localPosition = new Vector3(this.moveX, 0f, 0f);
            if ((Time.realtimeSinceStartup - lastCheckTime) > 1f)
            {
                // 下一个
                if (this.moveX < -150f)
                {
                    Menu.INSTANCE.mMain.mainNews.ShowNext();
                    this.moveX = 0;
                    this.lastCheckTime = Time.realtimeSinceStartup;
                }
                else if (this.moveX > 150f) // 上一个
                {
                    Menu.INSTANCE.mMain.mainNews.ShowPrevious();
                    this.moveX = 0;
                    this.lastCheckTime = Time.realtimeSinceStartup;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(this.moveX > 0)
            {
                rectTF.DOMoveX(0f, 0.5f).SetEase(Ease.InQuint);
            }
        }
    }
}