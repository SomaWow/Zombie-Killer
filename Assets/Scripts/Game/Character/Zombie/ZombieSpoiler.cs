using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ZombieSpoiler : ZombieAbs
    {

        public override void Update()
        {
            base.Update();
            if(base.hp > 0f)
            {
                switch (base.state)
                {
                    case 0:
                        this.ProcessIdle();
                        break;
                    case 1:
                        this.ProcessMove();
                        break;
                }
            }
        }

        protected override void ProcessIdle()
        {
            base.state = 1;
            ZombieAudioManager.Play("Zombie/zspoiler/wander", ZombieAudioType.WALK);
        }

        protected override void ProcessMove()
        {
            if((base.Agent != null) && (base.Agent.enabled))
            {
                // 拿到目标位置
                Transform target = base.target;
                if(target == null)
                {
                    target = HeroController.INSTANCE.transform;
                    base.target = target;
                }
                if(Vector3.Distance(target.position, base.Agent.destination) > 2.5f)
                {
                    base.Agent.SetDestination(target.position);
                }
                // float num = Vector3.Distance();
            }
        }

        public override void Hit(float dmg, Vector2 attackedBlend, Vector2 attackedForce, float basePower)
        {
            base.hp -= dmg;
            if(base.isDead || HeroController.INSTANCE.isLoaded)
            {
                // 
            }
            else if ((Random.Range(0f, 1f) > base.attackedSeed) && ((Time.time - base.lastAttackedTime) > 1.5f))
            {
                base.lastAttackedTime = Time.time;
                base.attackedSeed = 0.8f;
                base.Anim.CrossFade("ATTACKED", 0.05f);
                ZombieAudioManager.Play("Zombie/zspoiler/attacked", ZombieAudioType.HIT);
            }
            else
            {
                base.attackedSeed -= 0.1f;
            }
        }
    }
}