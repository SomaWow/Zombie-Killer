using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgZombieBornDealer : MonoBehaviour
    {
        private StgEventAbs stgEvent;
        public static List<StgZombieBornDealer> zombieDealers = new List<StgZombieBornDealer>();
        public static List<ZombieAbs> zombies = new List<ZombieAbs>();
        private List<ZombieBornPool> pools = new List<ZombieBornPool>();

        public int totalCount;
        public int nowCount;
        public int deadCount;
        private bool isWaiting = true;
        private float timer;

        public static int normalKillCount;
        public static int normalHeadshotCount;


        private static float lastDieTime = 0f;


        /// <summary>
        /// Start
        /// </summary>
        /// <param name="stgEvent"></param>
        /// <returns></returns>
        public static StgZombieBornDealer Start(StgEventAbs stgEvent)
        {
            StgZombieBornDealer dealer = new GameObject("StgZombieBornDealer").AddComponent<StgZombieBornDealer>();
            dealer.stgEvent = stgEvent;
            for (int i = 0; i < stgEvent.WaveCount; i++)
            {
                ZombieBornPool pool = new ZombieBornPool(stgEvent.Wave(i));
                dealer.totalCount += pool.count;
                dealer.pools.Add(pool);
            }
            zombieDealers.Add(dealer);
            // 如果为true，则先不进行下一步，所有这波僵尸死掉了，再进行下一步
            if(!stgEvent.isBlock)
            {
                HeroController.INSTANCE.StartCurrentEvent();
            }
            return dealer;
        }

        public static void Clear()
        {
            zombieDealers.Clear();
            zombies.Clear();
            normalKillCount = 0;
            normalHeadshotCount = 0;
            normalHeadshotCount = 0;
        }

        public void HandleZombieDie(ZombieAbs zombieController)
        {
            zombies.Remove(zombieController);
            this.nowCount--;
            this.deadCount++;
            lastDieTime = Time.realtimeSinceStartup;
            Debug.Log("僵尸死亡：" + deadCount + "/" + totalCount);
            // zombieDieAct暂时用不上
            if(this.deadCount >= this.totalCount)
            {
                // 所有僵尸死掉了再进行下一步
                if(this.stgEvent.isBlock)
                {
                    Debug.Log("进行下一步：HeroController.INSTANCE.StartCurrentEvent();");
                    HeroController.INSTANCE.StartCurrentEvent();
                }
                zombieDealers.Remove(this);
                Destroy(base.gameObject);
            }
        }

        private void Update()
        {
            if(!HeroController.INSTANCE.isLoaded)
            {
                if(this.isWaiting)
                {
                    this.timer += Time.deltaTime;
                    if(this.timer >= this.stgEvent.delay)
                    {
                        this.isWaiting = false;
                        this.timer = 0f;
                    }
                }
                if(!this.isWaiting)
                {
                    for (int i = 0; i < this.pools.Count; i++)
                    {
                        this.pools[i].Update(this);
                    }
                }
            }
        }
    }
}