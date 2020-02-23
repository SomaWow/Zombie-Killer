using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StgDataPointResource : MonoBehaviour {

    public List<StgDataConfigure> stgConfigList = new List<StgDataConfigure>();
    private static string gameDir = "GameData/StageData/";
    private static string endName = "DATA_POINT.txt";
    private static string endNameAndroid = "DATA_POINT";
    public static bool isLoad;
    private static StgDataPointResource _instance;

    public static StgDataPointResource INSTANCE
    {
        get
        {
            if (_instance == null)
            {
                _instance = GetStgPoint();
                isLoad = true;
            }
            return _instance;
        }
    }

    // 将文件内容加载出来
    public static StgDataPointResource GetStgPoint()
    {
        try
        {
            StgDataPointResource stgResource = new StgDataPointResource();
            stgResource.stgConfigList = JsonMapper.ToObject<List<StgDataConfigure>>((Resources.Load(gameDir + endNameAndroid, typeof(TextAsset)) as TextAsset).text);
            return stgResource;
        }
        catch (Exception exception)
        {
            Debug.Log("exception" + exception.ToString());
            return new StgDataPointResource();
        }
    }

    public StgDataConfigure GetStgDataConfig(int pointIndex)
    {
        if((pointIndex >= 0) && (pointIndex < this.stgConfigList.Count))
        {
            return this.stgConfigList[pointIndex];
        }
        Debug.Log("point index Failed:" + pointIndex);
        return null;
    }
}
