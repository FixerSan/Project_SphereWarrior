using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum UIEventType
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
        Drop
    }

    public enum UIPopupType
    {

    }

    public enum UISceneType
    {
        UIScene_Main
    }

    public enum Scene
    {
        Scene_Test, Scene_TestBoss
    }
    
    public enum FaceType
    {
        FaceOne, FaceTwo, FaceThree, FaceFour, FaceFive, FaceSix
    }
}
