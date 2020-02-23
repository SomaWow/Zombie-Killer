using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MuzzleFlash : RealTimeScale
    {
        public float flashTime = 0.15f;
        public float timer;

        public virtual void SetFire()
        {
            timer = 0;
            base.gameObject.SetActive(true);
        }

        private void Update()
        {
            timer += base.UpdateRealTimeDelta();
            if(timer >= flashTime)
            {
                base.gameObject.SetActive(false);
            }
        }
    }
}