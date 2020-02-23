using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgHeroMoveDealer : MonoBehaviour
    {
        private const float ArriveDistance = 0.5f;
        private StgEventAbs stgEvent;
        private StgEventAbs preEvent;
        private Vector3 position;
        private bool notifyZombie;
        private bool isArrived;
        private bool isWaiting = true;
        private float timer;


        public static void Start(StgEventAbs stgEvent)
        {
            StgHeroMoveDealer dealer = new GameObject("StgHeroMoveDealer").AddComponent<StgHeroMoveDealer>();
            dealer.stgEvent = stgEvent;
            dealer.position = stgEvent.HeroMovePosition;
            dealer.notifyZombie = stgEvent.HeroMoveNotifyZombie;
            dealer.preEvent = HeroController.INSTANCE.GetPreviousEvent();
            if(!stgEvent.isBlock)
            {
                HeroController.INSTANCE.StartCurrentEvent();
            }
        }

        private void Update()
        {
            if(this.isWaiting)
            {
                this.timer += Time.deltaTime;
                if(this.timer >= 3f)
                {
                    this.isWaiting = false;
                    this.timer = 0f;
                    this.Go();
                }
            }
            // 
        }

        private void Go()
        {
            GameCameraController.INSTANCE.cameraShake.StartShake(0f, 0.05f, 0f, 0.45f);
            Debug.Log("调用go");
        }
    }
}