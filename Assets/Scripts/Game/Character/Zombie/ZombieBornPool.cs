using Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class ZombieBornPool {
        public string zombieName;
        public int delay;
        public int count;
        public float minInterval;
        public float maxInterval;
        public float interval;
        public Vector3 position;
        public float range;
        public float timer;

        public bool isWaiting = true;
        private int nowCount;

        public ZombieBornPool(StgZombieWave wave)
        {
            this.zombieName = wave.zombieName;
            this.delay = wave.waveDelay;
            this.count = UnityEngine.Random.Range(wave.minCount, wave.maxCount);
            this.minInterval = wave.minInterval;
            this.maxInterval = wave.maxInterval;
            this.interval = UnityEngine.Random.Range(this.minInterval, this.maxInterval);
            this.position = wave.position;
            this.range = wave.range;
            // 缺
        }

        public void Update(StgZombieBornDealer zombieDealer)
        {
            if (this.isWaiting)
            {
                this.timer += Time.deltaTime;
                if (this.timer >= this.delay)
                {
                    this.isWaiting = false;
                    this.timer = 10000f;
                }
            }
            if (!this.isWaiting && (this.nowCount < this.count))
            {
                this.timer += Time.deltaTime;
                if (this.timer >= this.interval)
                {
                    this.timer = 0f;
                    this.interval = UnityEngine.Random.Range(this.minInterval, maxInterval);
                    this.CreateZombie(zombieDealer);
                    this.nowCount++;
                    zombieDealer.nowCount++;
                }
            }
        }

        public void CreateZombie(StgZombieBornDealer zombieDealer)
        {
            Transform trans = UnityEngine.Object.Instantiate(PrefabResources.Get("Zombie/" + zombieName));
            trans.position = this.position + new Vector3(UnityEngine.Random.Range(-this.range, this.range), 0.3f, UnityEngine.Random.Range(-this.range, this.range));
            trans.eulerAngles = new Vector3(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
            ZombieAbs zombieAbs = trans.GetComponent<ZombieAbs>();
            zombieAbs.Agent.Warp(trans.position);
            zombieAbs.zombieDieDelegate = (Action<ZombieAbs>)Delegate.Combine(zombieAbs.zombieDieDelegate, new Action<ZombieAbs>(zombieDealer.HandleZombieDie));
            trans.gameObject.SetActive(true);
            StgZombieBornDealer.zombies.Add(zombieAbs);
        }
    }
}