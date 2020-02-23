using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class HeroController : RealTimeScale
    {
        // 单例
        public static HeroController INSTANCE;

        public const float maxHP = 100f;
        public const float lowHP = 40f;
        private NavMeshAgent _agent;
        private GameWeaponController _weaponController;
        public StgData stgData;
        [HideInInspector]
        public int eventIndex;
        [HideInInspector]
        public bool isDead;
        [HideInInspector]
        public float timer;
        [HideInInspector]
        public float loadedTimer;
        [HideInInspector]
        public bool isLoaded; // 游戏结束，判定完成（胜利/失败），出结算画面
        private float loadTimer = 2f;
        private float _hp = 100f;
        private bool isLowHpEffectOn; // 低血量效果
        //private AudioSource footstepAudio;
        private AudioSource heartbeatAudio;
        private float lastAttackTime;
        private bool isAttackedFromRight = true;
        public static string stageName = "test";

        public NavMeshAgent agent
        {
            get
            {
                if (this._agent == null)
                    this._agent = base.GetComponent<NavMeshAgent>();
                return this._agent;
            }
        }
        public GameWeaponController weaponController
        {
            get
            {
                if (this._weaponController == null)
                    this._weaponController = base.gameObject.GetComponent<GameWeaponController>();
                return this._weaponController;
            }
        }
        public float hp
        {
            get { return this._hp; }
            set { this._hp = value; }
        }

        private void Awake()
        {
            INSTANCE = this;
            // 开启gamemenu
            GameMenu.CreateGameMenu();
            //GameMenu.INSTANCE.SetVisible(false);
            // 清理僵尸信息
            this.ClearZombieInformation();
        }

        private void Start()
        {
            this.LoadAllObject();
            this.StartCurrentEvent(); // 一切的开始
        }

        private void OnDisable()
        {
            if(this.heartbeatAudio != null)
            {
                Destroy(this.heartbeatAudio);
            }
        }

        private void Update()
        {
            // 血量低于40，开启心跳声
            if(((this.hp <= 40f) && (Time.timeScale >= 0.1)) && (this.hp > 0f))
            {
                if(this.heartbeatAudio == null)
                {
                    this.heartbeatAudio = AudioManager.INSTANCE.Play("Hero/heartbeat_lowhp", Vector3.zero, true);
                }

                if(this.heartbeatAudio != null)
                {
                    this.heartbeatAudio.pitch = (((40f - this.hp) / 40f) * 0.6f) + 0.4f;
                    this.heartbeatAudio.volume = 0.5f;
                    if (!this.heartbeatAudio.isPlaying)
                    {
                        this.heartbeatAudio.Play();
                    }
                }
            }
            else if((this.heartbeatAudio != null) && this.heartbeatAudio.isPlaying)
            {
                this.heartbeatAudio.Stop();
            }
            // 死掉了，任务失败
            if(this.isDead)
            {
                this.timer += Time.unscaledDeltaTime;
                if(this.timer >= 2f)
                {
                    Time.timeScale = Mathf.Min((Time.unscaledDeltaTime * 2f) + Time.timeScale, (float)1f);
                }
                if(this.timer >= 3)
                {
                    this.MissionFailed();
                }
            }

            // 加载好1s后显示结算页面
            if (this.isLoaded && (this.loadedTimer < 1f))
            {
                this.loadedTimer += Time.unscaledDeltaTime;
                if (this.loadedTimer >= 1f)
                {
                    if (Constants.IsMissionComplete)
                    {
                        GameMenu.INSTANCE.endMenu.Show(true);
                    }
                    else
                    {
                        GameMenu.INSTANCE.endMenu.Show(false);
                    }
                    Time.timeScale = 1f;
                }
            }
        }

        private void ClearZombieInformation()
        {
            StgZombieBornDealer.Clear();
            ZombieAudioManager.INSTANCE.ClearAll();
        }

        private void LoadAllObject()
        {
            // 没用上isMocked
            //if(isMocked)
            //{
            //    this.weaponController.EnableWeapon();
            //    this.stgData = StgDataManager.GetStageData(stageName);
            //}
            //else
            //{
            //    this.stgData = GameStgLoader.stgData;
            //}
            //ZombieTypeManager.LoadAll();
            //GameMenu.INSTANCE.SetVisible(true);
            //GameMenu.INSTANCE.gameInformation.SetStageHead(this.stgData.dataHead);
            this.stgData = GameStgLoader.stgData;
            //// 武器初始化
            this.weaponController.Init();
        }
        /// <summary>
        /// 控制结点
        /// </summary>
        public void StartCurrentEvent()
        {
            if(!this.isLoaded)
            {
                int count = this.stgData.evtList.Count;
                if(this.eventIndex >= count)
                {
                    // 所有结点完成，开协程 任务结束
                    base.StartCoroutine("MissionCompletedRoutine");
                }
                else
                {
                    StgEventAbs stgEvent = this.stgData.evtList[this.eventIndex];
                    this.eventIndex++;
                    StgEventDealer.Start(stgEvent);
                }
            }
        }

        private IEnumerator MissionCompletedRoutine()
        {
            while(this.loadedTimer > 0)
            {
                this.loadedTimer -= Time.unscaledDeltaTime;
                yield return null;
            }
            this.LoadMissionCompleted();
        }

        public void Hit(float dmg, bool isRight)
        {
            if(!this.isLoaded)
            {
                this._hp -= dmg;
                this.lastAttackTime = Time.time;
                this.isAttackedFromRight = isRight;
                
                if ((this._hp <= 0f) && !this.isDead)
                {
                    // Die
                    this.isDead = true;
                    // this._agent.enabled = false;
                    this.PlayHeroDie();
                }
                else
                {
                    // 显示被攻击的画面

                }
                float pitch = Random.Range((float)0.9f, (float)1.1f);
                AudioManager.INSTANCE.Play("Hero/attacked", Vector3.zero, 1f, pitch, false, false);
                AudioManager.INSTANCE.Play("Hero/hurt1", Vector3.zero, 1f, pitch, false, false);
            }
        }

        private void PlayHeroDie()
        {
            this.timer = 0f;
            // 设置武器
            this.weaponController.nowWeapon.gameObject.SetActive(false);
            // 设置倒下的位置，播放动画
            Vector3 right = GameCameraController.INSTANCE.MainCameraTrans.right;
            if(!this.isAttackedFromRight)
            {
                right = -right;
            }
            GameCameraController.INSTANCE.TweenToGround(right);
            // 设置GameMenu
            GameMenu.INSTANCE.SetControllerVisible(false);
            // 时间放慢
            Time.timeScale = 0.01f;
        }
        // 获得上一次事件
        public StgEventAbs GetPreviousEvent()
        {
            if(this.eventIndex <= 1)
            {
                return null;
            }
            return this.stgData.evtList[this.eventIndex - 1];
        }

        // 任务完成系列方法
        private void LoadMissionCompleted()
        {
            if(!this.isLoaded && (this.hp > 0))
            {
                this.isLoaded = true;
                Constants.IsMissionComplete = true;
                Time.timeScale = 1f;
                RealTimeScale.timeScale = 1f;
            }
        }

        public void MissionCompleted()
        {
            this.loadedTimer = 1f;
            base.StartCoroutine("MissionCompletedRoutine");
        }

        //private IEnumerator MissionCompletedRoutine()
        //{

        //}

        // 任务失败系列方法
        public void MissionFailed()
        {
            if(!this.isLoaded)
            {
                this.isLoaded = true;
                Constants.IsMissionComplete = false;
                Time.timeScale = 1f;
                RealTimeScale.timeScale = 1f;
            }
        }
    }
}