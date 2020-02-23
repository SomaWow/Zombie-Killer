using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgPrefabAbs : MonoBehaviour
    {
        public Action endDelegate;
        public Action notifyZombieBorn;
        [HideInInspector]
        public string behaviour = string.Empty;

        public virtual string GetInfo
        {
            get { return "No Info"; }
        }

        public virtual void Show()
        {
            if (this.endDelegate != null)
            {
                this.endDelegate();
            }
        }
    }
}