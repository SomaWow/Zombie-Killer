using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    // 动画曲线
    public AnimationCurve shakeCurve;
    private Vector3 originPos;
    private Vector3 moveTrans = Vector3.zero;
    private float timer;
    public float MaxTime = 0.5f;
    public Vector3 MaxTrans = new Vector3(0f, 0f, 0f);
    public float minMul = 0.5f;
    public int shakeTimes;
    private int dir = 1;
    public float shakeMul;
    private float acc = 5f;

    private void Awake()
    {
        this.originPos = this.transform.localPosition;
        enabled = false;
    }

    private void OnDisable()
    {
        this.transform.localPosition = this.originPos;
    }

    private void ResetMoveTrans()
    {
        float x = Random.Range(this.MaxTrans.x * this.minMul, this.MaxTrans.x);
        float y = Random.Range(this.MaxTrans.y * this.minMul, this.MaxTrans.y);
        float z = Random.Range(this.MaxTrans.z * this.minMul, this.MaxTrans.z);
        this.moveTrans = new Vector3(x, y, z) * this.shakeMul;
    }

    public void SetOrig()
    {
        this.originPos = this.transform.localPosition;
    }

    public void StartShake(float x, float y, float z, float t)
    {
        this.timer = 0f;
        this.shakeMul = 0f;
        this.acc = 1f;
        this.dir = 1;
        this.MaxTrans = new Vector3(x, y, z);
        this.MaxTime = t;
        this.ResetMoveTrans();
        this.shakeTimes = 0x7fffffff;
        base.enabled = true;
    }

    public void StartShake(float x, float y, float z, float t, float acc)
    {
        this.timer = 0f;
        this.shakeMul = 0f;
        this.acc = acc;
        this.dir = 1;
        this.MaxTrans = new Vector3(x, y, z);
        this.MaxTime = t;
        this.ResetMoveTrans();
        this.shakeTimes = 0x7fffffff;
        base.enabled = true;
    }

    public void StartShake(float x, float y, float z, float t, float acc, int times)
    {
        this.timer = 0f; // 计时器归零
        this.shakeMul = 0f; // 计时器归零
        this.acc = acc;
        this.dir = 1;
        this.MaxTrans = new Vector3(x, y, z); // 最大移动位置
        this.MaxTime = t; // 一次移动的时间
        this.ResetMoveTrans(); // 随机位置
        this.shakeTimes = times; // 震动次数
        enabled = true;
            
    }

    public void StopShake()
    {
        this.dir = -1;
        this.acc = 5f;
    }
    public void StopShake(float acc)
    {
        this.dir = -1;
        this.acc = acc;
    }

    private void Update()
    {
        // 类计时器，控制余震
        this.shakeMul += (Time.unscaledDeltaTime * this.dir) * this.acc;
        if(this.shakeMul < 0f)
        {
            enabled = false;
        }
        else
        {
            // 最大值为1
            this.shakeMul = Mathf.Min(1f, this.shakeMul);
        }
        // 计时器
        this.timer += Time.unscaledDeltaTime;
        if(this.timer > this.MaxTime)
        {
            // 一次计时结束，计时器归零
            this.timer -= this.MaxTime;
            // 重置随机位置
            this.ResetMoveTrans();
            if(this.dir > 0)
            {
                this.shakeTimes--;
                if(this.shakeTimes <= 0)
                {
                    this.StopShake();
                }
            }
        }
        float time = this.timer / this.MaxTime;
        this.transform.localPosition = this.originPos + (this.shakeCurve.Evaluate(time) * this.moveTrans);
    }
}
