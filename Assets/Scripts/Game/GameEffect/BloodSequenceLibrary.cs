using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BloodSequenceLibrary
    {
        private Dictionary<string, BloodSequenceList> listDict = new Dictionary<string, BloodSequenceList>();

        public void Add(string name, BloodSequence bloodSeq)
        {
            if(!this.listDict.ContainsKey(name))
            {
                this.listDict.Add(name, new BloodSequenceList());
            }
            this.listDict[name].Add(bloodSeq);
        }

        public BloodSequence Get(string name)
        {
            if(this.listDict.ContainsKey(name))
            {
                return this.listDict[name].Get();
            }
            return null;
        }

        public void Remove(string name)
        {
            this.listDict.Remove(name);
        }
    }
}