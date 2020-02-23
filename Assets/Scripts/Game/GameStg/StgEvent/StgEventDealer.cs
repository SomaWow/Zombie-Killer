using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StgEventDealer : MonoBehaviour
    {
        public static void Start(StgEventAbs stgEvent)
        {
            switch (stgEvent.type)
            {
                case StgEventType.HeroBorn:
                    StgHeroBornDealer.Start(stgEvent);
                    break;
                case StgEventType.ZombieBorn:
                    StgZombieBornDealer.Start(stgEvent);
                    break;
                case StgEventType.HeroMove:
                    break;
                case StgEventType.PrefabBorn:
                    break;
                case StgEventType.PrefabBornZombie:
                    break;
                case StgEventType.Conversation:
                    break;
                case StgEventType.NPCMove:
                    break;
            }
        }
    }
}