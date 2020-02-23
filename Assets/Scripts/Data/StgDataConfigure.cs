using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StgDataConfigure
{
    public string configureName;
    public string title;
    public string description;
    public string descriptionkey;
    //public CMissionMode mode;
    public int baseReward;
    public int hardness;

    public void Clone(StgDataConfigure clone)
    {
        this.configureName = clone.configureName;

        this.title = clone.title;
        this.description = clone.title;
        this.descriptionkey = clone.descriptionkey;
        this.baseReward = clone.baseReward;
        this.hardness = clone.hardness;
    }
}