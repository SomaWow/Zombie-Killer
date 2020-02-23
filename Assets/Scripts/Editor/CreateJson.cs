using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateJson : MonoBehaviour {

    [MenuItem("Assets/Build Json")]
    static void BuildJson()
    {
        StgDataConfigure stgConfig1 = new StgDataConfigure()
        {
            configureName = "Town_00_00",
            title = "Mission 1",
            description = "You wake up in the hospital to the sound of moaning. Zombies are all around, protect yourself.",
            descriptionkey = "11000",
            baseReward = 300,
            hardness = 1
        };
        StgDataConfigure stgConfig2 = new StgDataConfigure()
        {
            configureName = "Town_00_01",
            title = "Mission 2",
            description = "The zombies just keep coming, you have to get out of here.",
            descriptionkey = "11001",
            baseReward = 500,
            hardness = 3
        };
        StgDataConfigure stgConfig3 = new StgDataConfigure()
        {
            configureName = "Town_00_02",
            title = "Mission 3",
            description = "You found a survivor, protect her.",
            descriptionkey = "11002",
            baseReward = 800,
            hardness = 5
        };
        List<StgDataConfigure> configs = new List<StgDataConfigure>();
        configs.Add(stgConfig1);
        configs.Add(stgConfig2);
        configs.Add(stgConfig3);
        string str = JsonMapper.ToJson(configs);
        File.WriteAllText(Application.dataPath + "/" + "Resources/GameData/StageData/DATA_POINT.txt", str);
    }
}
