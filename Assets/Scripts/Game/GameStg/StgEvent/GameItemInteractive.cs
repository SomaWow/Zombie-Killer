using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameItemInteractive : StgPrefabAbs
    {
        public virtual void Hit(float dmg, float force, Vector3 forceDir, Vector3 position, bool isHitByBullet = true)
        {

        }
    }
}