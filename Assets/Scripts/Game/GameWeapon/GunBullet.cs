using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GunBullet : GunBulletBase
    {
        public const float bulletSpeed = 1000f;
        private const float trailLengthOrigin = 0.5f;
        private const float trailLengthGrowSpeed = 700f;
        private const float trailLengthMax = 40f;
        private const float trailSizeOrigin = 0.1f;
        private const float trailSizeGrowSpeed = 20f;
        private const float trailSizeMax = 0.2f;
        private LineRenderer lineRender;
        private float timer;
        private float maxTime;
        private bool isArrived;
        private float distance;

        private void Awake()
        {
            this.lineRender = base.GetComponent<LineRenderer>();
        }

        public override void SetTarget(RaycastHit hit)
        {
            base.target = hit.collider.gameObject;
            base.targetNormal = hit.normal;
            base.originalPos = base.transform.position;
            base.targetPos = hit.point;
            this.distance = Vector3.Distance(base.originalPos, base.targetPos);
            this.maxTime = this.distance / 1000f;
            base.transform.LookAt(base.targetPos);
            this.lineRender.SetPosition(0, new Vector3(0,0,0));
            this.lineRender.SetPosition(1, new Vector3(0,0,0));
        }

        public override void SetTarget(Vector3 point)
        {
            base.originalPos = base.transform.position;
            base.targetPos = point;
            this.distance = Vector3.Distance(base.originalPos, base.targetPos);
            this.maxTime = this.distance / 1000f;
            base.transform.LookAt(base.targetPos);
            this.lineRender.SetPosition(0, base.originalPos);
            this.lineRender.SetPosition(1, base.originalPos);
        }

        private void Update()
        {
            //到达目的地，销毁
            if(this.isArrived)
            {
                DestroyImmediate(base.gameObject);
            }
            else
            {
                this.timer = Mathf.Min(Time.deltaTime + this.timer, this.maxTime);
                float t = this.timer / this.maxTime;
                float num2 = Mathf.Min((float)((this.timer * 700f) + 0.5f), (float)40f);
                float start = Mathf.Min((float)((this.timer * 20f) + 0.1f), (float)0.2f);
                // 设置尾迹
                Vector3 dir = (base.targetPos - base.originalPos).normalized;
                this.lineRender.SetPosition(0, new Vector3(0f, 0f, -num2 * 1.2f)); //48, 421
                this.lineRender.SetPosition(1, new Vector3(0f, 0f, num2 * 0.025f)); //1,8.75
                // 设置开始和结束时的宽度
                this.lineRender.startWidth = start;
                this.lineRender.endWidth = start;
                // 位置插值
                Vector3 currentPos = Vector3.Lerp(base.originalPos, base.targetPos, t);
                base.transform.position = currentPos;
                if(t >= 1)
                {
                    this.isArrived = true;
                    this.HitEffect();
                }
            }
        }
    }
}