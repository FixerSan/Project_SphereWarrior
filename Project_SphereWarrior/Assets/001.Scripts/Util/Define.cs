using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public static readonly Vector3 gridOffset = new Vector3(-0.8f, -0.8f, -0.8f);
    public static readonly Vector3 gridScale = new Vector3(0.2f, 0.2f, 0.2f);

    /// <summary>
    /// �������� �ʿ��� ī��Ʈ
    /// </summary>
    public static readonly int NeedLevelUpCount = 30;


    /// <summary>
    /// ������ ������ ���� ���� ����
    /// </summary>
    public static readonly int MinMonsterCount = 5;


    public static readonly int GoldMonsterSpawnpBasePercentage = 10;
    public static readonly int CashMonsterSpawnpBasePercentage = 1;

    public static readonly int BaseMonsterHP = 10;

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
    
    public enum Face
    {
        FaceOne, FaceTwo, FaceThree, FaceFour, FaceFive, FaceSix, Null
    }

    public enum FaceType
    {
        Default, Test
    }

    public enum BallType
    {
        Default
    }

    public enum WorldTheme
    {
        Default,
    }

    public enum MonsterType
    {
        Default, Gold, Cash
    }
}
