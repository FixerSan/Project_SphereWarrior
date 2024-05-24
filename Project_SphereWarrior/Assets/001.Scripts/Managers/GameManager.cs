using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public WorldSystem world = new WorldSystem();
    public float swipeForceDownForce = 0.75f;
    public int nowEvent = 0;


    public void Awake()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("default", _completeCallback: () => 
        {
            player = new Player(); //Todo::나중에 저장된 녀석 불러오는 코드로 수정
            Managers.Data.LoadData();
            Managers.Face.InitFace();
            Managers.Ball.CreateBall(Define.BallType.Default);
            Managers.Ball.CreateBall(Define.BallType.Default);
            Managers.Ball.CreateBall(Define.BallType.Default);
            Managers.Ball.CreateBall(Define.BallType.Default);
            Managers.Ball.CreateBall(Define.BallType.Default);

            world.CheckMonsterCount();
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

    public void KillMonster(MonsterController _monster)
    {
        world.KillMonster(_monster);
    }

    public void LevelUpEvent()
    {
        world.LevelUpEvent();
    }

    //돈 얻는 코드
    public void GetGold(float _addGoldValue)
    {
        player.gold += _addGoldValue;
        Managers.UI.SceneUI?.RedrawUI();
    }


}

[System.Serializable]
public class Player
{
    //플레이어 골드
    public float gold;
    public float cash;
    public float attackForce;

    //면의 타입
    public Define.FaceType faceOneType = Define.FaceType.Default;
    public Define.FaceType faceTwoType = Define.FaceType.Default;
    public Define.FaceType faceThreeType = Define.FaceType.Default;
    public Define.FaceType faceFourType = Define.FaceType.Test;
    public Define.FaceType faceFiveType= Define.FaceType.Test;
    public Define.FaceType faceSixType = Define.FaceType.Test;

    //면의 레벨
    public int faceOneLevel = 1;
    public int faceTwoLevel = 1;
    public int faceThreeLevel = 1;
    public int faceFourLevel = 1;
    public int faceFiveLevel = 1;
    public int faceSixLevel = 1;

    //볼 타입 별 스피드
    public Dictionary<Define.BallType, float> ballSpeeds = new Dictionary<Define.BallType, float>();

    //플레이어 재화, 캐쉬 몬스터 확률 레벨
    public int cashMonsterSpawnPercentageLevel = 1;
    public int goldMonsterSpawnPercentageLevel = 1;

    public Player()
    {
        gold = 0f;
        attackForce = 5;
        ballSpeeds.Add(Define.BallType.Default, 3);
    }
}

public class WorldSystem
{
    public Define.WorldTheme currentWorldTheme = Define.WorldTheme.Default;
    public int worldLevel = 1;
    public int currentKillMonsterCount = 0;

    public void KillMonster(MonsterController _monster)
    {
        currentKillMonsterCount++;
        CheckLevelUp();
        Managers.Object.DespawnMonster(_monster);
        CheckMonsterCount();
    }

    public void LevelUpEvent()
    {

    }

    public void CheckLevelUp()
    {
        if(currentKillMonsterCount == Define.NeedLevelUpCount)
        {
            currentKillMonsterCount = 0;
            Managers.Game.LevelUpEvent();
        }
    }

    //몬스터 존재 수 체크
    public void CheckMonsterCount()
    {
        if (Define.NeedLevelUpCount - currentKillMonsterCount < Define.MinMonsterCount)
        {
            Debug.Log($"레벨업까지 남은 몬스터 수 {Define.NeedLevelUpCount - currentKillMonsterCount}");

            return;
        }

        if (Managers.Object.monsters.Count < Define.MinMonsterCount)
        {
            while(Managers.Object.monsters.Count < Define.MinMonsterCount)
                SpawnRandomMonster();
        }

        Debug.Log($"레벨업까지 남은 몬스터 수 {Define.NeedLevelUpCount - currentKillMonsterCount}");
    }

    public void SpawnRandomMonster()
    {
        int goldMonster = Managers.Game.player.goldMonsterSpawnPercentageLevel * Define.GoldMonsterSpawnpBasePercentage;
        int cashMonster = goldMonster + Define.CashMonsterSpawnpBasePercentage;

        int randomPercentage = UnityEngine.Random.Range(1, 101);
        Debug.Log(randomPercentage);

        if(randomPercentage > cashMonster)
            Managers.Object.SpawnMonster(currentWorldTheme, Define.MonsterType.Default);

        else if(randomPercentage == cashMonster)
            Managers.Object.SpawnMonster(currentWorldTheme, Define.MonsterType.Cash);

        else
            Managers.Object.SpawnMonster(currentWorldTheme, Define.MonsterType.Gold);
    }
}


