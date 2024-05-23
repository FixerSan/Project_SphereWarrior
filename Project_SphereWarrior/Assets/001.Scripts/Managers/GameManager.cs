using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public float swipeForceDownForce = 0.75f;
    public int nowEvent = 0;


    public void Awake()
    {
        player = new Player();
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("default", _completeCallback: () => 
        {
            Managers.Data.LoadData();
            Managers.Face.InitFace();
            Managers.Ball.CreateBall(Define.BallType.Default);
        });
    }

    public void FixedUpdate()
    {
        Managers.Input.DownSwipeForce();
    }

    //면과 공이 부딪쳤을 때 실행되는 코드
    public void CollisionFace(Define.Face _face)
    {
        Managers.Face.faces[(int)_face].CollisionFaceEffect();
    }

    //돈 얻는 코드
    public void GetGold(float _addGoldValue)
    {
        player.gold += _addGoldValue;
        CheckGoldEvent();
        Managers.UI.SceneUI?.RedrawUI();
    }

    //임시로 만든 몬스터 테스트용 이벤트
    public void CheckGoldEvent()
    {
        if(nowEvent == 0 && player.gold  >= 10)
        {
            LevelData data = Managers.Data.GetLevelData(1001);
            Managers.Grid.SetGrid(data);
            nowEvent++;
        }
    }
}

[System.Serializable]
public class Player
{
    public float gold;
    public float attackForce;

    public Define.FaceType faceOneType = Define.FaceType.Default;
    public Define.FaceType faceTwoType = Define.FaceType.Default;
    public Define.FaceType faceThreeType = Define.FaceType.Default;
    public Define.FaceType faceFourType = Define.FaceType.Test;
    public Define.FaceType faceFiveType= Define.FaceType.Test;
    public Define.FaceType faceSixType = Define.FaceType.Test;

    public int faceOneLevel = 1;
    public int faceTwoLevel = 1;
    public int faceThreeLevel = 1;
    public int faceFourLevel = 1;
    public int faceFiveLevel = 1;
    public int faceSixLevel = 1;

    public Dictionary<Define.BallType, float> ballSpeeds = new Dictionary<Define.BallType, float>();


    public Player()
    {
        gold = 0f;
        attackForce = 5;
        ballSpeeds.Add(Define.BallType.Default, 3);
    }
}


