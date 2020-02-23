using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    /// <summary>
    /// 主要有两个任务
    /// 1.渐隐渐显的效果
    /// 2.检测点击
    /// </summary>
    public class GameMenuEndContinue : MonoBehaviour
    {
        private bool isTouched;

        void Start()
        {
            GetComponent<Text>().DOFade(0f, 1.5f).SetLoops(1000, LoopType.Yoyo);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameMenu.INSTANCE.endMenu.ContinueClick();
                isTouched = true;
            }
        }
    }
}