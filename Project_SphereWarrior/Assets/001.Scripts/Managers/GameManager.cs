using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public float swipeForceDownForce = 0.75f;


    public void Awake()
    {
        player = new Player();
    }

    public void FixedUpdate()
    {
        Managers.Input.DownSwipeForce();
    }

    //��� ���� �ε����� �� ����Ǵ� �ڵ�
    public void CollisionFace(Define.FaceType _faceType)
    {
        player.faces[(int)_faceType].CollisionFaceEffect();
    }



    //�� ��� �ڵ�
    public void GetGold(float _addGoldValue)
    {
        player.gold += _addGoldValue;
        Managers.UI.SceneUI.RedrawUI();
    }
}

[System.Serializable]
public class Player
{
    public Face[] faces;

    public float gold;

    public Player()
    {
        faces = new Face[6];
        faces[(int)Define.FaceType.FaceOne] = new Face(1);
        faces[(int)Define.FaceType.FaceTwo] = new Face(1);
        faces[(int)Define.FaceType.FaceThree] = new Face(1);
        faces[(int)Define.FaceType.FaceFour] = new Face(1);
        faces[(int)Define.FaceType.FaceFive] = new Face(1);
        faces[(int)Define.FaceType.FaceSix] = new Face(1);

        gold = 0f;
    }
}


