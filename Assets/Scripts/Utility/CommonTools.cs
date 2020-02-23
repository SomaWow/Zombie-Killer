using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonTools {

    public static float Approximate(float value, float[] array)
    {
        if (array.Length > 0)
        {
            if (array.Length <= 1)
            {
                return array[0];
            }
            if (value <= array[0])
            {
                return array[0];
            }
            if (value >= array[array.Length - 1])
            {
                return array[array.Length - 1];
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (value <= array[i])
                {
                    if (value <= ((array[i - 1] + array[i]) / 2f))
                    {
                        return array[i - 1];
                    }
                    return array[i];
                }
            }
        }
        return value;
    }

    public static Vector3 StringToVector3(string str)
    {
        char[] separator = new char[] { ','};
        string[] strArray = str.Split(separator);
        Vector3 zero = Vector3.zero;
        try
        {
            zero.x = float.Parse(strArray[0]);
            zero.y = float.Parse(strArray[1]);
            zero.z = float.Parse(strArray[2]);
        }
        catch(Exception exception)
        {
            Debug.LogException(exception);
        }
        return zero;
    }
    public static string Vector3ToString(Vector3 vec)
    {
        object[] objArray = new object[] { vec.x, ",", vec.y, ",", vec.z };
        return string.Concat(objArray);
    }

    public static float SlowTimeScale
    {
        get
        {
            return Mathf.Max(Time.timeScale, 0.5f);
        }
    }
}
