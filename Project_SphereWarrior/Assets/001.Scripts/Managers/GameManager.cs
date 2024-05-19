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

    //면과 공이 부딪쳤을 때 실행되는 코드
    public void CollisionFace(Define.FaceType _faceType)
    {
        player.faces[(int)_faceType].CollisionFaceEffect();
    }

    //돈 얻는 코드
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

    //임시로 만든 몬스터 테스트용 이벤트
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


