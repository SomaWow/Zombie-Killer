using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeScale : MonoBehaviour {

    private float mRt;
    private float mTimeStart;
    private float mTimeDelta;
    private float mActual;
    private bool mTimeStarted;
    public static float timeScale = 1f;

    // 属性
    // 返回此时此刻的时间
    public float realTime
    {
        get { return this.mRt; }
    }
    // 返回上一帧花费的时间
    public float realTimeDelta
    {
        get { return this.mTimeDelta; }
    }

    protected virtual void OnEnable()
    {
        this.mTimeStarted = true;
        this.mTimeDelta = 0f;
        this.mTimeStart = Time.realtimeSinceStartup;
    }
    protected float UpdateRealTimeDelta()
    {
        this.mRt = Time.realtimeSinceStartup;
        if(this.mTimeStarted)
        {
            float b = this.mRt - this.mTimeStart;
            this.mActual += Mathf.Max(0f, b);
            // 保留三位小数
            this.mTimeDelta = 0.001f * Mathf.Round(this.mActual * 1000f);
            this.mActual -= this.mTimeDelta;
            if(this.mTimeDelta > 1f)
            {
                this.mTimeDelta = 1f;
            }
            this.mTimeStart = this.mRt;
        }
        else
        {
            this.mTimeStarted = true;
            this.mTimeStart = this.mRt;
            this.mTimeDelta = 0f;
        }
        return (this.mTimeDelta * timeScale);
    }
}
