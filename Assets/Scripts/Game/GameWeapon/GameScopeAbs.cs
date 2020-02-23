using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameScopeAbs : RealTimeScale 
    {
        public virtual void Hide() { }

        public virtual void SetAcc(float acc) { }

        public virtual void SetAimed(bool aimed) { }

        public virtual void Shoot(float fireForce) { }

        public virtual void Show() { }
    }
}