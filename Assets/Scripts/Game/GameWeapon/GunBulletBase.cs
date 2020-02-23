using Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GunBulletBase : MonoBehaviour
    {
        public const int SPECIAL_HIT = 2;
        public const float minForceY = 0.3f;
        public bool isHero = true;
        public Vector3 originalPos;
        // 目标位置
        public Vector3 targetPos;
        // 击中的目标物
        public GameObject target;
        // 目标的法线
        public Vector3 targetNormal;
        public float baseDmg = 1f;
        public float basePower = 1f;

        public virtual void HitEffect()
        {
            if(this.target != null)
            {
                // 换成小写
                string str = this.target.tag.ToLower();
                if(str.StartsWith("scene"))
                {
                    this.ProcessHitScene();
                }
                else if(str.StartsWith("enemy"))
                {
                    this.ProcessHitZombie();
                    AudioManager.INSTANCE.Play("Gun/hit_zombie", Vector3.zero, 1f, Random.Range((float)0.75f, (float)1.25f), false, false);
                }
                else if(str.StartsWith("interactive"))
                {
                    this.ProcessHitInteractiveObj();
                }
            }
        }

        // 击中互动物体，比如车，汽油桶
        protected virtual void ProcessHitInteractiveObj()
        {
            GameItemInteractive component = this.target.GetComponent<GameItemInteractive>();
            if(component != null)
            {
                Vector3 vector = this.targetPos - this.originalPos;
                component.Hit(this.baseDmg, this.basePower, vector.normalized, this.targetPos, true);
            }
        }

        // 击中场景
        protected virtual void ProcessHitScene()
        {
            string tag = this.target.tag;
            Transform transform = null;
            if (tag.EndsWith("Metal"))
            {
                transform = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletBoomMetal")) as Transform;
                AudioManager.INSTANCE.Play("Gun/hit_metal", Vector3.zero, 1f, Random.Range((float)0.9f, (float)1.1f), false, false);
            }
            else if (tag.EndsWith("Glass"))
            {
                transform = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletBoomMetal")) as Transform;
                AudioManager.INSTANCE.Play("Gun/hit_metal", Vector3.zero, 1f, Random.Range((float)0.75f, (float)1.25f), false, false);
            }
            else if (tag.EndsWith("MetalMove"))
            {
                transform = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletBoomMetalNoHole")) as Transform;
                AudioManager.INSTANCE.Play("Gun/hit_metal", Vector3.zero, 1f, Random.Range((float)0.9f, (float)1.1f), false, false);
            }
            else if (tag.EndsWith("NormalMove"))
            {
                transform = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletBoomSandNoHole")) as Transform;
                AudioManager.INSTANCE.Play("Gun/hit_wood", Vector3.zero, 1f, Random.Range((float)0.75f, (float)1.25f), false, false);
            }
            else
            {
                transform = Instantiate(PrefabResources.Get("Weapon/Bullet/BulletBoomSand")) as Transform;
                if(Random.Range(0f, 1f) > 0.3f)
                {
                    AudioManager.INSTANCE.Play("Gun/hit_rock", Vector3.zero, 1f, Random.Range((float)0.75f, (float)1.25f), false, false);
                }
                else
                {
                    AudioManager.INSTANCE.Play("Gun/hit_wood", Vector3.zero, 1f, Random.Range((float)0.75f, (float)1.25f), false, false);
                }
            }
            // 向法线方向移动一点点，避免重合
            transform.position = this.targetPos + (this.targetNormal * 0.025f);
            transform.forward = this.targetNormal;
        }

        // 击中僵尸
        protected virtual void ProcessHitZombie()
        {
            string str = this.target.tag.ToLower();
            ZombieAbs zombieAbsComp = this.target.transform.root.GetComponent<ZombieAbs>();
            if(zombieAbsComp != null)
            {
                float num = Random.Range(0f, 1f);
                // 僵尸出血，需要位置点产生血
                BloodSequence.Show("Effect/Blood/BloodSplatEffect", this.targetPos);
                // 僵尸的血溅到屏幕上，需要距离判断是否会溅到
                float hitPointDist = Vector3.Distance(this.targetPos, GameCameraController.INSTANCE.MainCameraTrans.position);
                ZombieAttackedBlood.Show("Effect/ZombieAttacked", hitPointDist);
                Vector3 backward = -zombieAbsComp.transform.forward;
                Vector2 vertBack = new Vector2(backward.x, backward.y);
                Vector2 normVertBack = vertBack.normalized;
                Vector2 lhs = new Vector2(-normVertBack.y, normVertBack.x);

                Vector3 targetDir = this.targetPos - this.originalPos; // 从玩家指向集中目标
                Vector2 vertTargetDir = new Vector2(targetDir.x, targetDir.z);
                Vector2 rhs = vertTargetDir.normalized;

                Vector2 attackedForce = new Vector2(Vector2.Dot(lhs, rhs), Vector2.Dot(normVertBack, rhs));
                Vector2 zero = Vector2.zero;
                float baseDmg = this.baseDmg;

                if(str.EndsWith("head"))
                {
                    zero = new Vector2(0f, 1f);
                    //baseDmg *= 2f;
                    baseDmg = 100f;
                }
                else if(str.EndsWith("body"))
                {
                    zero = new Vector2(0f, 0f);
                    attackedForce.y *= 0.7f;
                }
                else if (str.EndsWith("leftarm"))
                {
                    zero = new Vector2(-1f, 1f);
                    attackedForce.x -= 0.15f;
                    attackedForce /= 2f;
                }
                else if (str.EndsWith("rightarm"))
                {
                    zero = new Vector2(1f, 1f);
                    attackedForce.x -= 0.15f;
                    attackedForce /= 2f;
                }
                else if (str.EndsWith("leftleg"))
                {
                    zero = new Vector2(-1f, -1f);
                    attackedForce /= 2f;
                }
                else if (str.EndsWith("rightleg"))
                {
                    zero = new Vector2(1f, -1f);
                    attackedForce /= 2f;
                }
                else if (str.EndsWith("special"))
                {
                    zero = new Vector2(2f, 0f);
                    attackedForce /= 2f;
                }
                if(((zombieAbsComp.hp <= baseDmg) && (zombieAbsComp.hp > 0f)) && (zombieAbsComp.dangerLevel < 0x3e8))
                {
                    // 计算各种统计信息
                    StgZombieBornDealer.normalKillCount++;
                    if(zero.x == 0 && zero.y == 1)
                    {
                        StgZombieBornDealer.normalHeadshotCount++;
                        AudioManager.INSTANCE.Play("Effect/headshot");
                    }
                }
                attackedForce.y *= this.basePower;
                // attackedForce.y的取值范围为(-∞, -0.3f] && [0.3f, ∞)
                if (attackedForce.y > -0.3f && attackedForce.y < 0.3f)
                {
                    attackedForce.y = (attackedForce.y >= 0f) ? 0.3f : -0.3f;
                }
                zombieAbsComp.Hit(baseDmg, zero, attackedForce, this.basePower);
            }
        }
        public virtual void SetTarget(RaycastHit hit)
        {

        }
        public virtual void SetTarget(Vector3 point)
        {

        }
    }
}