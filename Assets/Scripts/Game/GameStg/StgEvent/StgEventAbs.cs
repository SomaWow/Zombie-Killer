using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Game
{
    public class StgEventAbs
    {
        // 区分事件类型
        public StgEventType type;
        public bool isBlock = true;
        public float delay;

        public static StgEventAbs ToStgEvent(XmlNode node)
        {
            switch ((StgEventType)int.Parse(node.Attributes.GetNamedItem("Type").InnerText))
            {
                case StgEventType.HeroBorn:
                    return StgEventHeroBorn.ToStgEvent(node);
                case StgEventType.ZombieBorn:
                    return StgEventZombieBorn.ToStgEvent(node);
                //case StgEventType.HeroMove:
                //    return StgEventHeroMove.ToStgEvent(node);
                //case StgEventType.HeroGet:
                //    return StgEventHeroGet.ToStgEvent(node);
                //case StgEventType.NPCBorn:
                //    return StgEventNPCBorn.ToStgEvent(node);
                //case StgEventType.NPCMove:
                //    return StgEventNPCMove.ToStgEvent(node);
                //case StgEventType.NPCDeliver:
                //    return StgEventNPCDeliver.ToStgEvent(node);
                //case StgEventType.PrefabBorn:
                //    return StgEventPrefabBorn.ToStgEvent(node);
                //case StgEventType.PrefabBornZombie:
                //    return StgEventPrefabBornZombie.ToStgEvent(node);
                //case StgEventType.Conversation:
                //    return StgEventConversation.ToStgEvent(node);
            }
            return new StgEventAbs();
        }

        public virtual XmlElement ToXmlElement(XmlDocument xmlDoc)
        {
            XmlElement element = xmlDoc.CreateElement("E");
            element.SetAttribute("Type", ((int)this.type + string.Empty));
            XmlElement newChild1 = xmlDoc.CreateElement("EB");
            newChild1.InnerText = !this.isBlock ? "0" : "1";
            XmlElement newChild2 = xmlDoc.CreateElement("ED");
            newChild2.InnerText = this.delay + string.Empty;

            element.AppendChild(newChild1);
            element.AppendChild(newChild2);
            return element;
        }

        public virtual Vector3 HeroBornPosition
        {
            get { return Vector3.zero; }
            set { }
        }
        public virtual Vector3 HeroBornEulerAngles
        {
            get { return Vector3.zero; }
            set { }
        }

        public virtual int WaveCount
        {
            get
            {
                return 0;
            }
        }
        public virtual StgZombieWave Wave(int index)
        {
            return new StgZombieWave();
        }
        public virtual Vector3 HeroMovePosition
        {
            get { return Vector3.zero; }
            set { }
        }
        public virtual bool HeroMoveNotifyZombie
        {
            get { return true; }
            set { }
        }
    }
}