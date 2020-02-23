using Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BloodSequence : RealTimeScale
    {
        private static BloodSequenceLibrary bloodLibrary = new BloodSequenceLibrary();
        public float interval = 0.3f;
        private float timer;
        [HideInInspector]
        public bool isAvailable; // 是否为可用对象

        public static void Show(string prefabName, Vector3 position)
        {
            BloodSequence bloodSeq = bloodLibrary.Get(prefabName);
            // 如果为空，就新实例化一个
            if(bloodSeq == null)
            {
                Transform trans = Instantiate(PrefabResources.Get(prefabName)) as Transform;
                bloodSeq = trans.GetComponent<BloodSequence>();
                bloodLibrary.Add(prefabName, bloodSeq);
            }
            bloodSeq.timer = 0f;
            // 设置
            bloodSeq.isAvailable = false;
            bloodSeq.transform.position = position;
            bloodSeq.gameObject.SetActive(true);
        }
        

        private void Update()
        {
            this.timer += base.UpdateRealTimeDelta() * CommonTools.SlowTimeScale;
            while(this.timer >= this.interval)
            {
                this.timer -= this.interval;
                // 回收
                this.isAvailable = true;
                base.gameObject.SetActive(false);
            }
        }
    }
}