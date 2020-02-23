using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


public class StgDataManager
{
    private static string editorDir = (Application.dataPath + "/Resources/GameData/StageData");
    private static string gameDir = "GameData/StageData";
    private static string extension = ".xml";
    public static char[] sep = new char[] { '-' };
    public static string sepStr = "-";

    // 读取信息，生成一个StgData，填充其中的StgDataHead和List<StgEventAbs>
    public static StgData GetStageData(string resName)
    {
        XmlDocument document = new XmlDocument();
        TextAsset asset = Resources.Load(gameDir + resName, typeof(TextAsset)) as TextAsset;
        document.Load(new StringReader(asset.text));
        XmlNode node = document.SelectSingleNode("ROOT");
        StgData data = new StgData {
            dataHead = StgDataHead.ToStgDataHead(node["H"])
        };
        XmlNode node2 = node["ES"];

        IEnumerator enumerator = node2.ChildNodes.GetEnumerator();
        try
        {
            while(enumerator.MoveNext())
            {
                XmlNode current = (XmlNode)enumerator.Current;
                StgEventAbs item = StgEventAbs.ToStgEvent(current);
                data.evtList.Add(item);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if(disposable == null)
            {
            }
            disposable.Dispose();
        }

        return data;
    }

    public static StgData LoadStgData(TextAsset textAsset)
    {
        XmlDocument document = new XmlDocument();
        document.Load(new StringReader(textAsset.text));
        XmlNode node = document.SelectSingleNode("ROOT");
        StgData data = new StgData
        {
            dataHead = StgDataHead.ToStgDataHead(node["H"])
        };
        XmlNode node2 = node["ES"];
        IEnumerator enumerator = node2.ChildNodes.GetEnumerator();
        try
        {
            while(enumerator.MoveNext())
            {
                XmlNode current = (XmlNode)enumerator.Current;
                StgEventAbs item = StgEventAbs.ToStgEvent(current);
                data.evtList.Add(item);
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
        return data;
    }
}