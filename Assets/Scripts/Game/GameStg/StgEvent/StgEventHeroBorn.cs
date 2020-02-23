using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Game
{
    public class StgEventHeroBorn : StgEventAbs
    {
        private Vector3 position = Vector3.zero;
        private Vector3 eulerAngles = Vector3.zero;

        public StgEventHeroBorn()
        {
            base.type = StgEventType.HeroBorn;
        }

        public static StgEventHeroBorn ToStgEvent(XmlNode node)
        {
            return new StgEventHeroBorn
            {
                isBlock = node["EB"].InnerText.Equals("1"),
                delay = float.Parse(node["ED"].InnerText),
                position = CommonTools.StringToVector3(node["P"].InnerText),
                eulerAngles = CommonTools.StringToVector3(node["A"].InnerText)
            };
        }

        public override XmlElement ToXmlElement(XmlDocument xmlDoc)
        {
            XmlElement element = xmlDoc.CreateElement("E");
            element.SetAttribute("Type", ((int)base.type) + string.Empty);
            XmlElement newChild1 = xmlDoc.CreateElement("EB");
            newChild1.InnerText = !base.isBlock ? "0" : "1";
            XmlElement newChild2 = xmlDoc.CreateElement("ED");
            newChild2.InnerText = base.delay + string.Empty;
            XmlElement newChild3 = xmlDoc.CreateElement("P");
            newChild3.InnerText = CommonTools.Vector3ToString(this.position);
            XmlElement newChild4 = xmlDoc.CreateElement("A");
            newChild4.InnerText = CommonTools.Vector3ToString(this.eulerAngles);

            element.AppendChild(newChild1);
            element.AppendChild(newChild2);
            element.AppendChild(newChild3);
            element.AppendChild(newChild4);

            return element;

        }

        public override Vector3 HeroBornPosition
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }
        public override Vector3 HeroBornEulerAngles
        {
            get
            {
                return this.eulerAngles;
            }

            set
            {
                this.eulerAngles = value;
            }
        }
    }
}