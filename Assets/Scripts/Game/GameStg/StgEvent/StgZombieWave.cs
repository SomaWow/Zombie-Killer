using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgZombieWave : MonoBehaviour
    {
        public string zombieName;
        public int waveDelay;
        public int minCount;
        public int maxCount;
        public float minInterval;
        public float maxInterval;
        public Vector3 position = Vector3.zero;
        public float range;

        public int count
        {
            get { return Random.Range(this.minCount, this.maxCount + 1); }
        }

        public float interval
        {
            get { return Random.Range(this.minInterval, this.maxInterval); }
        }


        public override string ToString()
        {
            return
                "\n zombieName " + zombieName +
                "\n waveDelay " + waveDelay +
                "\n minCount " + minCount +
                "\n maxCount " + maxCount +
                "\n minInterval " + minInterval +
                "\n maxInterval " + maxInterval +
                "\n position " + position +
                "\n range " + range
                ;
        }
    }
}