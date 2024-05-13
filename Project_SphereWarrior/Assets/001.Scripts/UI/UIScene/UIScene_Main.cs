using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScene_Main : UIScene
{
    private Vector2 dragPointOne;
    
    public float dragForce;

    public override bool Init()
    {
        if(! base.Init()) return false;
        BindImage(typeof(Images));
        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: BeginDrag, _type: Define.UIEventType.BeginDrag);
        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: Drag, _type: Define.UIEventType.Drag);
        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: EndDrag, _type: Define.UIEventType.EndDrag);
        return true;
    }

    public void BeginDrag(PointerEventData _data)
    {
        dragPointOne = _data.position;
    }

    public void Drag(PointerEventData _data)
    {
        Managers.Input.swipeDirection = (_data.position - dragPointOne).normalized;
        Managers.Input.swipeForce = Vector2.Distance(_data.position, dragPointOne) * dragForce;
        dragPointOne = _data.position;
    }

    public void EndDrag(PointerEventData _data)
    {

    }

    public enum Images
    {
        Image_SwipePlace
    }
}
