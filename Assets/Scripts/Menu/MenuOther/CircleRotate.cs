using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class CircleRotate : MonoBehaviour
    {

        private float rotateAngle = 400;

        void Update()
        {
            transform.Rotate(Vector3.forward, rotateAngle * Time.deltaTime, Space.World);
        }
    }
}