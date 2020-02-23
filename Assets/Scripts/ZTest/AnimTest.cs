using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour {
    public Animation anim;
    
    public void Reset()
    {
        anim.Play("EQUIP");
    }
}
