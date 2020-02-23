using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuCameraController : MonoBehaviour {

    public GameObject sceneCamera;
    public GameObject gunCamera1;

    // 场景
    public GameObject sceneObj;

    // 展示场景
    public void ShowCameraScene()
    {
        this.sceneCamera.SetActive(true);
        this.gunCamera1.SetActive(false);
    }

    // 展示枪
    public void ShowCameraGun1()
    {
        this.sceneCamera.SetActive(false);
        this.gunCamera1.SetActive(true);
    }

    public void GunCamera1Zoom()
    {
        gunCamera1.transform.DOMove(new Vector3(0, 0, -1.8f), 0.6f);
    }
    public void GunCamera1Shrink()
    {
        gunCamera1.transform.DOMove(new Vector3(-0.65f, -0.15f, -2.5f), 0.6f);
    }
}
