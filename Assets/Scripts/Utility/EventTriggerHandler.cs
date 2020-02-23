using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Action onPointerDown;
    public Action onPointerUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onPointerDown != null)
        {
            onPointerDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(onPointerUp != null)
        {
            onPointerUp();
        }
    }
}
