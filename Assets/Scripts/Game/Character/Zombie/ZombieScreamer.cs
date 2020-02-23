using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    /// <summary>
    /// 不会吼叫的吼叫者
    /// </summary>
    public class ZombieScreamer : ZombieAbs {

        private Transform rightHandTrans;
        private Transform mouthTrans;

        private void Start()
        {
            this.SetProp();
        }

        public override void Update()
        {
            base.Update();
            if (base.hp > 0f)
            {
                switch (base.state)
                {
                    case 0:
                        this.ProcessIdle();
                        break;
                    case 1:
                        this.ProcessMove();
                        break;
                    case 2:
                        this.ProcessAttack();
                        break;
                }
            }
        }

        public virtual void SetProp()
        {
            Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].name.Equals("Bip001 R Hand"))
                {
                    this.rightHandTrans = componentsInChildren[i];
                }
                if (componentsInChildren[i].name.Equals("Bip001 Head"))
                {
                    this.mouthTrans = componentsInChildren[i];
                }
            }
        }

        // 攻击效果
        public void AnimatorRightAttack()
        {
            if ((Time.time - base.lastAttackedTime) >= 0.5f)
            {
                base.lastAttackedTime = Time.time;
                base.Anim.SetBool("Attack", false);
                Vector3 attackForce = ZombieAbs.attackForce;

                if ((base.target == null) || (base.target == HeroController.INSTANCE.transform))
                {
                    GameCameraController.INSTANCE.ApplyForce(attackForce);
                    HeroController.INSTANCE.Hit(base.attackDmg, false);
                    Debug.Log("受到攻击");
                    this.ShowAttackBlood(-1, false, Random.Range(45f, 60f));
                }
                // 还会攻击其他人物和物体
                else
                {

                }
                ZombieAudioManager.Play("Zombie/zscreamer/attack", ZombieAudioType.ATTACKE);
            }
        }

        public override void Hit(float dmg, Vector2 attackedBlend, Vector2 attackedForce, float basePower)
        {
            // 掉血
            base.hp -= dmg;
            if (base.isDead || HeroController.INSTANCE.isLoaded)
            {
                //this.SetMultiImage(0.1f);
            }
            else
            {
                // 减速效果
                if (base.currentSpeed > (base.desireSpeed / 2f))
                {
                    base.currentSpeed /= 1.1f;
                    base.currentSpeed = Mathf.Max(0.15f, base.currentSpeed);
                    base.Anim.SetFloat("Speed", base.currentSpeed);
                    float num = Mathf.Max((float)1f, (float)(3f - base.currentSpeed)) / 2f;
                }

                if (((Random.Range(0f, 1f) > base.attackedSeed) && (Time.time - base.lastAttackedTime) > 1.5f))
                {
                    base.lastAttackedTime = Time.time;
                    base.attackedSeed = 0.8f;
                    base.Anim.CrossFade("ATTACKED", 0.05f);
                    ZombieAudioManager.Play("Zombie/zscreamer/attacked", ZombieAudioType.HIT);
                }
                else
                {
                    base.attackedSeed -= 0.1f;
                }
                // 当被打到，唤醒所有僵尸
                //StgZombieBornDealer.WakeUpAllZombies();
            }
        }

        protected override void ProcessIdle()
        {
            base.state = 1;
            base.Agent.isStopped = false;
            ZombieAudioManager.Play("Zombie/zscreamer/wander", ZombieAudioType.WALK);
        }

        protected override void ProcessMove()
        {
            if((base.Agent != null) && base.Agent.enabled)
            {
                Transform target = base.target;
                if(target == null)
                {
                    target = HeroController.INSTANCE.transform;
                    base.target = target;
                }
                // 目标位置和设置位置有偏差，重新设置
                if(Vector3.Distance(target.position, base.Agent.destination) > 2.5f)
                {
                    base.Agent.SetDestination(target.position);
                }
                float num = Vector3.Distance(target.position, base.transform.position);
                // 当进入攻击距离，停下来开始初次攻击
                if (num <= base.attackDistance)
                {
                    // 停下来，开始攻击
                    base.Agent.isStopped = true;
                    base.Agent.velocity = Vector3.zero;
                    if(base.startAttackInterval <= 0f)
                    {
                        base.timer = 0f;
                        base.Anim.SetBool("Attack", true);
                        base.Anim.CrossFade("ATTACK", 0.2f);
                    }
                    else
                    {
                        base.timer = base.attackInterval - base.startAttackInterval;
                    }
                    base.currentSpeed = 0f;
                    base.Anim.SetFloat("Speed", 0f);
                    base.state = 2;
                }
                // 进入过渡地带，仍未进入攻击距离
                else if(num <= base.slowdownDistance)
                {
                    if (!base.isSlowDownThreatened)
                    {
                        base.isSlowDownThreatened = true;
                        ZombieAudioManager.Play("Zombie/zscreamer/born", ZombieAudioType.APPROACH);
                    }

                    // 调整速度
                    if (base.currentSpeed >= 2.5f)
                    {
                        base.currentSpeed -= ((base.currentSpeed - 1f) * 0.5f) * Time.deltaTime;
                        base.Anim.SetFloat("Speed", base.currentSpeed);
                    }
                    else if (base.currentSpeed <= 0.2f)
                    {
                        base.currentSpeed += base.acc * Time.deltaTime;
                        base.Anim.SetFloat("Speed", base.currentSpeed);
                    }
                    else
                    {
                        base.currentSpeed -= ((base.currentSpeed - 1f) * 3f) * Time.deltaTime;
                        base.Anim.SetFloat("Speed", base.currentSpeed);
                    }
                }
                else
                {
                    // 增加速度
                    base.currentSpeed += base.acc * Time.deltaTime;
                    base.currentSpeed = Mathf.Min(base.currentSpeed, base.desireSpeed);
                    base.Anim.SetFloat("Speed", base.currentSpeed);
                }

                base.walkSoundTimer += Time.deltaTime;
                if (base.walkSoundTimer >= base.walkSoundDuration)
                {
                    base.walkSoundTimer = 0f;
                    base.walkSoundDuration = Random.Range((float)0.5f, (float)4f);
                    ZombieAudioManager.Play("Zombie/zscreamer/wander", ZombieAudioType.WALK);
                }
            }
        }

        protected override void ProcessAttack()
        {
            base.timer += Time.deltaTime;
            Transform target = base.target;
            if(target == null)
            {
                target = HeroController.INSTANCE.transform;
            }

            // 目标位置和设置位置有偏差，停止攻击，重新寻路
            if (Vector3.Distance(target.position, base.Agent.destination) > 2.5f)
            {
                base.Anim.SetBool("Attack", false);
                base.state = 0;
            }
            Vector3 forward = target.position - base.transform.position;
            forward.y = 0;
            Quaternion to = Quaternion.LookRotation(forward);
            base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, Time.deltaTime);
            // 计时超过攻击间隔，再次攻击
            if(base.timer >= base.attackInterval)
            {
                base.timer = 0f;
                base.Anim.Play("ATTACK", 0, 0.05f);
            }
        }

        public override void PlayDieAnim()
        {
            //GameMenuKillSpecialEffect.Show("screamer");
            // 随机撒币特效
            //GameUIGoldBoomEffect.Show(GameMenu.INSTANCE.effect2PanelTrans, 15, new Vector3(0f, 120f, 0f));
            base.Agent.isStopped = true;
            base.Agent.velocity = Vector3.zero;
            base.Anim.CrossFade("DIE", 0.15f);
            base.Anim.speed = UnityEngine.Random.Range((float)0.8f, (float)1f);
            ZombieAudioManager.Play("Zombie/zscreamer/die", ZombieAudioType.HIT);
        }

        public override void ShowAttackBlood(int dir, bool isMirror, float angle)
        {
            if(this.rightHandTrans != null)
            {
                Vector3 position = this.rightHandTrans.position;
                // 物体在摄像机范围内，则x、y的范围为[0, 1]
                Vector3 vector2 = GameCameraController.INSTANCE.sceneCamera.WorldToViewportPoint(position) - new Vector3(0.5f, 0.5f, 0f);

                float num = ((float)GameMenu.INSTANCE.GetComponent<CanvasScaler>().referenceResolution.y / ((float)Screen.height));
                float num2 = Screen.width * num;
                float num3 = Screen.height * num;
                Vector3 vector3 = new Vector3(num2 * vector2.x, num3 * vector2.y, vector2.z);
                ZombieAttackBlood.Show("Effect/ZombieAttack", vector3, dir, angle);
            }
        }
    }
}