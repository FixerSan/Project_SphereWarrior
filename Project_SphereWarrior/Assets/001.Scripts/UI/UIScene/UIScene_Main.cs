using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScene_Main : UIScene
{
    private Vector2 dragPointOne;
   

    public override bool Init()
    {
        if(! base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: BeginDrag, _type: Define.UIEventType.BeginDrag);
        BindEvent(GetImage((int)Images.Image_SwipePlace).gameObject, _dragCallback: Drag, _type: Define.UIEventType.Drag);

        Managers.UI.SceneUI = this;
        return true;
    }

    public override void RedrawUI()
    {
        GetText((int)Texts.Text_Gold).text = $"{Managers.Game.player.gold} Gold";
    }

    public void BeginDrag(PointerEventData _data)
    {
        dragPointOne = _data.position;
    }

    public void Drag(PointerEventData _data)
    {
        Managers.Input.SetSwipeDirection((_data.position - dragPointOne).normalized);
        Managers.Input.SetSwipeForce(Vector2.Distance(_data.position, dragPointOne));
        dragPointOne = _data.position;
    }

    public enum Images
    {
        Image_SwipePlace
    }

    public enum Texts
    {
        Text_Gold
    }
}
