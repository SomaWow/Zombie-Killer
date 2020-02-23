using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineTest : MonoBehaviour {
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 3));
    }

    
}
