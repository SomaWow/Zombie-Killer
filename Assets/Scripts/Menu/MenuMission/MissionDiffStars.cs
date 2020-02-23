using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MissionDiffStars : MonoBehaviour
    {
        public List<GameObject> starObjList;

        public void SetStars(int count)
        {
            for (int i = 0; i < starObjList.Count; i++)
            {
                if(i < count)
                {
                    starObjList[i].SetActive(true);
                }
                else
                {
                    starObjList[i].SetActive(false);
                }
            }
        }
    }
}