using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class BloodSequenceList
    {
        private int count;
        private List<BloodSequence> bloodList = new List<BloodSequence>();

        public void Add(BloodSequence bloodSeq)
        {
            this.count++;
            this.bloodList.Add(bloodSeq);
        }

        public BloodSequence Get()
        {
            for (int i = 0; i < this.count; i++)
            {
                if (this.bloodList[i].isAvailable)
                {
                    return this.bloodList[i];
                }
            }
            return null;
        }
    }
}