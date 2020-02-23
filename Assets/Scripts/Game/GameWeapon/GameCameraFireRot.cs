using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameCameraFireRot : RealTimeScale
    {
        public float backBaseSpeed = 2.2f;
        public float backSpeedAdd = 3f;
        private Vector3 speed = Vector3.zero;
        private float resistance = 100f;
        private static Vector3 maxSpeed = new Vector3(300f, 450f, 450f);
        private static float maxSpeedMag = 1000f;
        private float deltaTime;

        public void ApplyFire(float force)
        {
            // 俯仰
            base.transform.Rotate(GameCameraController.INSTANCE.MainCameraTrans.right, -force, Space.Self);
            // 翻转
            base.transform.Rotate(GameCameraController.INSTANCE.MainCameraTrans.forward, -force / 10f, Space.Self);
        }

        private float ApplyForce(float angle)
        {
            angle = this.WrapAngle(angle); //[-180, 180]
            float num = this.backBaseSpeed + (this.backSpeedAdd * Mathf.Abs(angle));
            if (angle > 0f)
            {
                angle = Mathf.Max((float)(angle - (num * this.deltaTime)), (float)0f);
                return angle;
            }
            if (angle < 0f)
            {
                angle = Mathf.Min((float)(angle + (num * this.deltaTime)), (float)0f);
            }
            return angle;
        }

        public void ApplyForce(Vector3 force)
        {
            force = (Vector3)(GameCameraController.INSTANCE.MainCameraTrans.rotation * force);
            this.speed += force;
            this.speed.x = Mathf.Clamp(this.speed.x, -maxSpeed.x, maxSpeed.x);
            this.speed.y = Mathf.Clamp(this.speed.y, -maxSpeed.y, maxSpeed.y);
            this.speed.z = Mathf.Clamp(this.speed.z, -maxSpeed.z, maxSpeed.z);
            float magnitude = this.speed.magnitude;
            if(magnitude > maxSpeedMag)
            {
                this.speed = this.speed * (magnitude / maxSpeedMag);
            }
        }

        private void Update()
        {
            this.deltaTime = base.UpdateRealTimeDelta();
            base.transform.Rotate(this.speed * this.deltaTime, Space.World);
            this.speed -= Vector3.one * this.resistance;
            this.speed.x = Mathf.Max(0f, this.speed.x);
            this.speed.y = Mathf.Max(0f, this.speed.y);
            this.speed.z = Mathf.Max(0f, this.speed.z);
            Vector3 localEulerAngles = base.transform.localEulerAngles;
            localEulerAngles.x = this.ApplyForce(localEulerAngles.x);
            localEulerAngles.y = this.ApplyForce(localEulerAngles.y);
            localEulerAngles.z = this.ApplyForce(localEulerAngles.z);
            base.transform.localEulerAngles = localEulerAngles;
        }

        public float WrapAngle(float angle)
        {
            while(angle > 180f)
            {
                angle -= 360f;
            }
            while(angle < -180)
            {
                angle += 360f;
            }
            return angle;
        }
    }
}