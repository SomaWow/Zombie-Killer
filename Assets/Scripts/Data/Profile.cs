using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile{
    // 设置
    public static bool soundEnable = true;
    public static bool musicEnable = true;
    public static string username = "Nameless";

    public static string Gun1Name = "M629";
    public static string Gun2Name = "M10";
    public static string Item1Name = "bloodbox";
    public static string Item2Name = "grenade";

    public static int gold = 0;
    public static int gem = 0;

    public static float sensitive = 1f;

    public static int killCount = 0;
    public static int headshotCount = 0;
    public static int noHurtMissionCount = 0;

    public static void LoadAll()
    {
        LoadSettings();
        LoadAchievement();
        LoadMoney();
    }

    public static void LoadSettings()
    {
        if(PlayerPrefs.HasKey("f3592a6747b760b7"))
            musicEnable = PlayerPrefs.GetInt("f3592a6747b760b7") == 1;
        if(PlayerPrefs.HasKey("6d7b02f52d0f35b5"))
            soundEnable = PlayerPrefs.GetInt("6d7b02f52d0f35b5") == 1;
        if(PlayerPrefs.HasKey("20978c7bf19f62cf"))
            username = PlayerPrefs.GetString("20978c7bf19f62cf");
    }
    public static void LoadAchievement()
    {
        killCount = PlayerPrefs.GetInt("c775f3a4325266cb");
        headshotCount = PlayerPrefs.GetInt("276fc3aa99600068");
        noHurtMissionCount = PlayerPrefs.GetInt("82a44d4340ef0760");
    }

    public static void LoadMoney()
    {
        gold = PlayerPrefs.GetInt("fdb1f267b06093bc");
        gem = PlayerPrefs.GetInt("06d047a398f9eb09");
    }


    public static void UpdateAchievement()
    {
        PlayerPrefs.SetInt("c775f3a4325266cb", killCount);
        PlayerPrefs.SetInt("276fc3aa99600068", headshotCount);
        PlayerPrefs.SetInt("82a44d4340ef0760", noHurtMissionCount);
    }

    public static void UpdateSettings()
    {
        PlayerPrefs.SetInt("f3592a6747b760b7", !musicEnable ? 0 : 1);
        PlayerPrefs.SetInt("6d7b02f52d0f35b5", !soundEnable ? 0 : 1);
        PlayerPrefs.SetString("20978c7bf19f62cf", username);
    }

    public static void UpdateGold(int addNum)
    {
        gold += addNum;
        PlayerPrefs.SetInt("fdb1f267b06093bc", gold);
    }

    public static void UpdateGem(int addNum)
    {
        gem += addNum;
        PlayerPrefs.SetInt("06d047a398f9eb09", gem);
    }
}
