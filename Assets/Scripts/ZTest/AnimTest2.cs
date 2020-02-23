using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest2 : MonoBehaviour {

    public AnimTest animTest;

    private void OnGUI()
    {
        if(GUILayout.Button("按钮"))
        {
            animTest.gameObject.SetActive(true);
            animTest.Reset();
        }
    }
}
