using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 僵尸类型管理
    /// </summary>
    public class ZombieResource
    {
        public static List<ZombieAbs> zombies = new List<ZombieAbs>();
        private static bool isLoaded = false;

        public static void LoadAll()
        {
            if (!isLoaded)
            {
                LoadZombieType();
                LoadZombieBehaviour();
                isLoaded = true;
            }
        }

        public static void LoadZombieType()
        {

        }
        public static void LoadZombieBehaviour()
        {

        }
    }
}