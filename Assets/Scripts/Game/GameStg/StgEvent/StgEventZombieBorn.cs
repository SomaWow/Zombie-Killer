using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Game
{
    public class StgEventZombieBorn : StgEventAbs
    {
        private List<StgZombieWave> waves = new List<StgZombieWave>();

        public StgEventZombieBorn()
        {
            base.type = StgEventType.ZombieBorn;
        }

        public static StgEventZombieBorn ToStgEvent(XmlNode node)
        {
            StgEventZombieBorn born = new StgEventZombieBorn
            {
                isBlock = node["EB"].InnerText.Equals("1"),
                delay = float.Parse(node["ED"].InnerText)
            };
            XmlNode node2 = node["WS"];
            IEnumerator enumerator = node2.ChildNodes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    XmlNode current = (XmlNode)enumerator.Current;
                    StgZombieWave item = new StgZombieWave
                    {
                        zombieName = current["T"].InnerText,
                        waveDelay = int.Parse(current["D"].InnerText),
                        minCount = int.Parse(current["IC"].InnerText),
                        maxCount = int.Parse(current["AC"].InnerText),
                        minInterval = float.Parse(current["IT"].InnerText),
                        maxInterval = float.Parse(current["AT"].InnerText),
                        position = CommonTools.StringToVector3(current["P"].InnerText),
                        range = float.Parse(current["R"].InnerText)
                    };
                    born.waves.Add(item);
                    Debug.Log(item.ToString());
                }
            }
            finally
            {
                //IDisposable disposable = enumerator as IDisposable;
                //if (disposable == null)
                //{
                //}
                //disposable.Dispose();
            }
            return born;
        }

        public override StgZombieWave Wave(int index)
        {
            if((index >= 0) && (index < this.waves.Count))
            {
                return this.waves[index];
            }
            return null;
        }

        public override int WaveCount
        {
            get
            {
                return waves.Count;
            }
        }

    }

}