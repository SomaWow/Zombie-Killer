using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class StgDataHead : MonoBehaviour
{
    public StgType stgType;
    public StgEndType stgEndType;
    public string describe = string.Empty;
    public string describekey = string.Empty;
    public string special = string.Empty;
    public int endCount;
    public string map = string.Empty;

    public static StgDataHead ToStgDataHead(XmlNode node)
    {
        StgDataHead head = new StgDataHead {
            stgType = (StgType) int.Parse(node["T"].InnerText),
            stgEndType = (StgEndType) int.Parse(node["E"].InnerText),
            endCount = int.Parse(node["C"].InnerText),
            map = node["M"].InnerText
        };
        if(node["D"] != null)
        {
            head.describe = node["D"].InnerText;
        }
        if(node["S"] != null)
        {
            head.special = node["S"].InnerText;
        }
        if(node["DK"] != null)
        {
            head.describe = node["DK"].InnerText;
        }
        return head;
    }
    public XmlElement ToXmlElement(XmlDocument xmlDoc)
    {
        XmlElement element = xmlDoc.CreateElement("H");

        XmlElement newChild1 = xmlDoc.CreateElement("T");
        newChild1.InnerText = ((int)this.stgType) + string.Empty;
        XmlElement newChild2 = xmlDoc.CreateElement("D");
        newChild2.InnerText = this.describe;
        XmlElement newChild3 = xmlDoc.CreateElement("DK");
        newChild3.InnerText = this.describekey;
        XmlElement newChild4 = xmlDoc.CreateElement("E");
        newChild4.InnerText = ((int)this.stgEndType) + string.Empty;
        XmlElement newChild5 = xmlDoc.CreateElement("C");
        newChild5.InnerText = this.endCount + string.Empty;
        XmlElement newChild6 = xmlDoc.CreateElement("M");
        newChild6.InnerText = this.map;
        XmlElement newChild7 = xmlDoc.CreateElement("S");
        newChild7.InnerText = this.special;

        element.AppendChild(newChild1);
        element.AppendChild(newChild2);
        element.AppendChild(newChild3);
        element.AppendChild(newChild4);
        element.AppendChild(newChild5);
        element.AppendChild(newChild6);
        element.AppendChild(newChild7);

        return element;
    }
}