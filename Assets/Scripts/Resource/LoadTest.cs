using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadTest : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(LoadFromFile("M10.assetbundle", AddTank));
    }

    private void LoadScene(AssetBundle ab)
    {
        SceneManager.LoadScene("TownHospital01Normal");
    }

    private void AddTank(AssetBundle ab)
    {
        // 加载所有GameObject
        GameObject[] objs = ab.LoadAllAssets<GameObject>();
        foreach (GameObject go in objs)
        {
            Debug.Log(go.name);
            Instantiate(go);
        }
        // 只加载其中一个
        //GameObject tank = ab.LoadAsset<GameObject>("TankPrefab");
        //Instantiate(tank);
    }

    // 从本地文件加载
    private IEnumerator LoadFromFile(string abName, UnityAction<AssetBundle> OnLoaded)
    {
        string path;

#if UNITY_EDITOR || UNITY_STANDALONE
        path = "file://" + Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
        path = "jar:file://" + Application.dataPath + "!/assets/Android";
#elif UNITY_IPHONE
        path = "file://" + Application.dataPath + "/Raw";
#endif

        path = path + "/" + abName;

        Debug.Log(path);

        WWW www = WWW.LoadFromCacheOrDownload(path, 0);
        while(!www.isDone)
        {
            yield return null;
        }

        if(www.error != null)
        {
            Debug.Log("Load Error: " + www.error);
        }

        AssetBundle ab = www.assetBundle;
        if (ab == null)
            Debug.LogError("资源为空");
        else
            OnLoaded(ab);

    }
}