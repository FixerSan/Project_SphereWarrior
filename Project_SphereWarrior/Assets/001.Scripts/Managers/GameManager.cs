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
            player = new Player(); //Todo::���߿� ����� �༮ �ҷ����� �ڵ�� ����
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


    //��� ���� �ε����� �� ����Ǵ� �ڵ�
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

    //�� ��� �ڵ�
    public void GetGold(float _addGoldValue)
    {
        player.gold += _addGoldValue;
        Managers.UI.SceneUI?.RedrawUI();
    }


}

[System.Serializable]
public class Player
{
    //�÷��̾� ���
    public float gold;
    public float cash;
    public float attackForce;

    //���� Ÿ��
    public Define.FaceType faceOneType = Define.FaceType.Default;
    public Define.FaceType faceTwoType = Define.FaceType.Default;
    public Define.FaceType faceThreeType = Define.FaceType.Default;
    public Define.FaceType faceFourType = Define.FaceType.Test;
    public Define.FaceType faceFiveType= Define.FaceType.Test;
    public Define.FaceType faceSixType = Define.FaceType.Test;

    //���� ����
    public int faceOneLevel = 1;
    public int faceTwoLevel = 1;
    public int faceThreeLevel = 1;
    public int faceFourLevel = 1;
    public int faceFiveLevel = 1;
    public int faceSixLevel = 1;

    //�� Ÿ�� �� ���ǵ�
    public Dictionary<Define.BallType, float> ballSpeeds = new Dictionary<Define.BallType, float>();

    //�÷��̾� ��ȭ, ĳ�� ���� Ȯ�� ����
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

    //���� ���� �� üũ
    public void CheckMonsterCount()
    {
        if (Define.NeedLevelUpCount - currentKillMonsterCount < Define.MinMonsterCount)
        {
            Debug.Log($"���������� ���� ���� �� {Define.NeedLevelUpCount - currentKillMonsterCount}");

            return;
        }

        if (Managers.Object.monsters.Count < Define.MinMonsterCount)
        {
            while(Managers.Object.monsters.Count < Define.MinMonsterCount)
                SpawnRandomMonster();
        }

        Debug.Log($"���������� ���� ���� �� {Define.NeedLevelUpCount - currentKillMonsterCount}");
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


