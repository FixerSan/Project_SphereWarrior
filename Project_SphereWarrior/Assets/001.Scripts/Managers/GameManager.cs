using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public float swipeForceDownForce = 0.75f;
    public int nowEvent = 0;

    public Define.FaceType LookingFace 
    {
        get
        {
            return CheckLookingFace();
        }
    }

    public void Awake()
    {
        player = new Player();
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("default");
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
        CheckGoldEvent();
        Managers.UI.SceneUI.RedrawUI();
    }

    private Define.FaceType CheckLookingFace()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitData, 10, LayerMask.GetMask("Face"), QueryTriggerInteraction.Ignore))
        {
            FaceController face = hitData.transform.GetComponent<FaceController>();
            if (face != null)
                return face.faceType;

        }
        return Define.FaceType.Null;
    }

    //�ӽ÷� ���� ���� �׽�Ʈ�� �̺�Ʈ
    public void CheckGoldEvent()
    {
        if(nowEvent == 0 && player.gold  >= 10)
        {
            Managers.Object.SpawnMonster(0, 30, Vector3.zero);
            nowEvent++;
        }
    }
}

[System.Serializable]
public class Player
{
    public Face[] faces;
    public float gold;
    public float attackForce;

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
        attackForce = 5;
    }
}


