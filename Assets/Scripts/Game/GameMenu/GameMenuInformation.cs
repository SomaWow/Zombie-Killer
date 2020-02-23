using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameMenuInformation : MonoBehaviour
    {
        [HideInInspector]
        public StgType stgType;
        [HideInInspector]
        public StgEndType stgEndType;
        [HideInInspector]
        public string describe = string.Empty;
        [HideInInspector]
        public string special = string.Empty;
        [HideInInspector]
        public int count;

        public void SetStageHead(StgDataHead dataHead)
        {
            this.stgType = dataHead.stgType;
            this.stgEndType = dataHead.stgEndType;
            this.describe = dataHead.describe;
            if(this.describe.Equals(string.Empty))
            {
                // 待补充
            }
            this.special = dataHead.special;
            this.count = dataHead.endCount;
            // 待补充
        }
    }
}