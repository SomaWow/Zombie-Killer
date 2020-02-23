using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgHeroBornDealer : MonoBehaviour
    {
        public static void Start(StgEventAbs stgEvent)
        {
            // 设置英雄的位置和转向
            HeroController.INSTANCE.transform.position = stgEvent.HeroBornPosition;
            HeroController.INSTANCE.transform.eulerAngles = new Vector3(0f, stgEvent.HeroBornEulerAngles.y, 0f);

            HeroController.INSTANCE.GetComponent<GameCameraController>().sceneCamera.transform.localEulerAngles = new Vector3(stgEvent.HeroBornEulerAngles.x, 0f, 0f);
            //HeroController.INSTANCE.agent.enabled = true;
            HeroController.INSTANCE.StartCurrentEvent();
        }
    }
}