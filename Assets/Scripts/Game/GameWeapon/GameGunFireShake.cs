using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameGunFireShake : MonoBehaviour
    {
        public Vector3 basePos = Vector3.zero;
        public float backBaseSpeed = 1f;
        public float backSpeedAdd = 3f;
        public float backRotBaseSpeed = 0.5f;
        public float backRotSpeedAdd = 1f;
        private float deltaTime;
        public float forceRotMulX = 0.75f;
        public float forceMulZ = 0.0005f;
        private List<int> toDelete = new List<int>();
        private List<Vector3> offsets = new List<Vector3>();
        private List<Vector3> rots = new List<Vector3>();

        // 参数为后坐力
        public void ApplyFire(float force)
        {
            // 前后
            base.transform.Translate(new Vector3(0f, 0f, -force * this.forceMulZ), Space.Self);
            // 俯仰
            base.transform.localEulerAngles += new Vector3(-this.forceRotMulX * force, 0f, 0f);
        }

        public void ApplyFire(float force, float randomForce)
        {
            base.transform.Translate(
                new Vector3(
                    (Random.Range(-1f, 1f) * randomForce) * forceMulZ, // 随机
                    (Random.Range(-1f, 1f) * randomForce) * forceMulZ, // 随机
                    -force * this.forceMulZ // 后坐力
                ), Space.Self);
            // 俯仰
            base.transform.localEulerAngles += new Vector3(-this.forceRotMulX * force, 0f, 0f);
        }

        private void Update()
        {
            if(this.forceMulZ > 0f)
            {
                base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.basePos, Time.unscaledDeltaTime * 3f);
            }
            if (this.forceRotMulX > 0f)
            {
                base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.identity, Time.unscaledDeltaTime * 5f);
            }
        }
    }
}