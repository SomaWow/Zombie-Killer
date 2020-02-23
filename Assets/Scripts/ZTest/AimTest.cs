using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTest : MonoBehaviour {

    public float force;
    public GameAimPointNormal aimPoint;

    private void OnGUI()
    {
        if (GUILayout.Button("开火"))
        {
            aimPoint.Shoot(force);
        }
    }
}
