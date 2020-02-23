using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class ZombieAbs : MonoBehaviour
    {
        public Transform target;

        public const float attackedSeedMax = 0.8f;
        public float hp;
        public float attackDmg;
        public int dangerLevel = 5;
        public bool isDead;
        public int state;
        protected float attackedSeed = 0.6f;
        public float desireSpeed;
        public float acc;
        public float lastAttackTime;
        public float lastAttackedTime;
        public float currentSpeed;
        public bool isMirror;
        public float startAttackInterval;
        public float attackInterval;
        public float timer;
        protected static Vector3 attackForce = new Vector3(300f, 400f, 300f);
        protected float attackDistance = 2.5f;
        protected float slowdownDistance = 3f;
        protected bool isSlowDownThreatened;
        protected float walkSoundTimer;
        protected float walkSoundDuration;
        protected static float[] attackedApprox = new float[] { -1f, -0.2f, 0.2f, 1f };


        public Action<ZombieAbs> zombieDieDelegate;


        private Animator _anim;
        private NavMeshAgent _agent;

        private void Start()
        {
            walkSoundDuration = UnityEngine.Random.Range(0.5f, 3f);
        }

        public virtual void Update()
        {
            if ((!this.isDead && !HeroController.INSTANCE.isLoaded) && (this.hp <= 0f))
            {
                this.isDead = true;
                this.currentSpeed = 0f;
                this.Anim.SetFloat("Speed", 0f);
                this.Agent.avoidancePriority = 30; // 修改回避优先级，默认为50，0为最重要，99为最不重要
                this.PlayDieAnim();
                if (this.zombieDieDelegate != null)
                {
                    this.zombieDieDelegate(this);
                }
                Destroy(this.gameObject, 3f);
            }
        }

        protected virtual void ProcessIdle()
        {

        }

        protected virtual void ProcessMove()
        {

        }

        protected virtual void ProcessAttack()
        {

        }

        public virtual void Hit(float dmg, Vector2 attackedBlend, Vector2 attackedForce, float basePower)
        {

        }

        public virtual void PlayDieAnim()
        {

        }

        public virtual void ShowAttackBlood(int dir, bool isMirror, float angle) { }

        // 属性
        public Animator Anim
        {
            get
            {
                if(this._anim == null)
                {
                    this._anim = GetComponent<Animator>();
                }
                return _anim;
            }
        }

        public NavMeshAgent Agent
        {
            get
            {
                if(this._agent == null)
                {
                    this._agent = base.GetComponent<NavMeshAgent>();
                    this._agent.enabled = true;
                }
                return this._agent;
            }
        }
    }
}