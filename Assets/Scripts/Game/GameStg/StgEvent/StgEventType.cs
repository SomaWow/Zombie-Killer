using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum StgEventType
    {
        HeroBorn = 0,
        HeroMove = 1,
        HeroGet = 2,
        Conversation = 10,
        ZombieBorn = 20,
        NPCBorn = 30,
        NPCMove = 0x1f,
        NPCDeliver = 0x20,
        PrefabBorn = 40,
        PrefabBornZombie = 0x2a,
        CameraWander = 5022
    }
        
}