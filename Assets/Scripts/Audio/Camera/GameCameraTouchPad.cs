using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameCameraTouchPad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 prePos = Vector2.zero;
    // 委托
    public Action<Vector2> onDown;
    public Action<Vector2> onUp;
    public Action<Vector2> onDrag;

    public void OnDrag(PointerEventData eventData)
    {
        if (this.onDrag != null)
            this.onDrag(eventData.position - prePos);
        prePos = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prePos = eventData.position;
        if (this.onDown != null)
            this.onDown(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.onUp != null)
            this.onUp(eventData.position);
    }
}
