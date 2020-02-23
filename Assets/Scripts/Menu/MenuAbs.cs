using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /// <summary>
    /// 控制界面的基类
    /// </summary>
    public class MenuAbs : MonoBehaviour
    {

        // 形参为MenuAbs的函数
        public Action<MenuAbs> showEndDelegate;
        public Action<MenuAbs> hideEndDelegate;

        public virtual void BackClick() { }

        public virtual void Hide() { }

        public virtual void Show() { }
        
        public T Find<T>(string name)
        {
            if(transform.Find(name) == null)
            {
                Debug.LogError(this + "子对象: " + name + "没有找到！");
                return default(T);
            }
            return transform.Find(name).GetComponent<T>();
        }

        // 这是属性，字段不能被重写
        // lamda表达式相当于写了一个只有get的属性
        public virtual int TAG
        {
            get { return -1; }
        }
    }
}