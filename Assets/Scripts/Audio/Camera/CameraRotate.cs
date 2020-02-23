using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    private float rotateAngle = 1;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateAngle * Time.deltaTime, Space.World);
    }
}
