using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StgData
{
    public StgDataHead dataHead = new StgDataHead();
    public List<StgEventAbs> evtList = new List<StgEventAbs>();
    public int mainIndex;
    public int subIndex;
    public string resName = string.Empty;

    public StgType stgType
    {
        get { return this.dataHead.stgType; }
        set { this.dataHead.stgType = value; }
    }
    public string stgDescribe
    {
        get { return this.dataHead.describe; }
        set { this.dataHead.describe = value; }
    }
    public StgEndType stgEndType
    {
        get { return this.dataHead.stgEndType; }
        set { this.dataHead.stgEndType = value; }
    }
    public int endCount
    {
        get { return this.dataHead.endCount; }
        set { this.dataHead.endCount = value; }
    }
    public string map
    {
        get { return this.dataHead.map; }
        set { this.dataHead.map = value; }
    }
}