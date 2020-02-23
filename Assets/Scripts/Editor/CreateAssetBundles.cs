using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")] // 在Assets菜单下拓展Build AssetBundles按钮
    static void BuildAllAssetBundles() // 点击该按钮将执行本函数
    {
        string outputPath = Application.streamingAssetsPath;
        // 如果已经存在，删除已经存在的文件
        if(Directory.Exists(outputPath))
        {
            // 递归的删除
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.Android); // 支持路径，如 Asset/AssetBundle
    }
}
