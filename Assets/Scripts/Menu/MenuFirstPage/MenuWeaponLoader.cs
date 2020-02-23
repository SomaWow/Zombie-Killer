using Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuWeaponLoader : MonoBehaviour
    {
        public static Dictionary<string, MenuWeaponLoader> loaders = new Dictionary<string, MenuWeaponLoader>();

        public GameObject weaponInstance;
        public Sprite gunSprite;
        public Vector3 originPos;

        public static MenuWeaponLoader GetLoader(string name)
        {
            if(loaders.ContainsKey(name))
            {
                return loaders[name];
            }
            MenuWeaponLoader newLoader = new MenuWeaponLoader();
            newLoader.weaponInstance = Instantiate(PrefabResources.Get("Weapon/" + name).gameObject);
            newLoader.originPos = newLoader.weaponInstance.transform.localPosition;
            newLoader.weaponInstance.SetActive(false);
            newLoader.gunSprite = Resources.Load("Textures/" + name, typeof(Sprite)) as Sprite;
            Debug.Log(newLoader.gunSprite.name);
            loaders[name] = newLoader;
            return newLoader;
        }

        public static void ClearLoaders()
        {
            loaders.Clear();
        }
    }
}