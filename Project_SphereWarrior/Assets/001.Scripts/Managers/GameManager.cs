using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public WorldSystem world = new WorldSystem();
    public UpgradeSystem upgrade = new UpgradeSystem();
    public float swipeForceDownForce = 0.75f;
    public int nowEvent = 0;


    public void Awake()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("default", _completeCallback: () => 
        {
            player = new Player(); //Todo::���߿� ����� �༮ �ҷ����� �ڵ�� ����
            Managers.Data.LoadData();
            Managers.Face.InitFace();

            Managers.UI.ShowSceneUI<UIScene_Main>();

            world.CheckBallCount();
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
        SetIngameGold();

        Managers.UI.SceneUI?.RedrawUI();
    }

    //�� ��� �ڵ�
    public void RemoveGold(float _removeGoldValue)
    {
        player.gold -= _removeGoldValue;
        SetIngameGold();

        Managers.UI.SceneUI?.RedrawUI();
    }

    private void SetIngameGold()
    {
        player.inGameGoldString = Util.FloatToSymbolString(player.gold);
    }
}

[System.Serializable]
public class Player
{
    //�÷��̾� ���
    public float gold;
    public string inGameGoldString;
    public float cash;

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

    //�� ����
    public int ballDamageLevel = 1;
    public int ballSpeedLevel = 1;
    public int ballCountLevel = 1;
    public int ballCriticalLevel = 1;

    //�� ������
    public float beforeBallDamage;
    public float currentBallDamage;
    public float afterBallDamage;

    //�� ���ǵ�
    public float beforeBallSpeed;
    public float currentBallSpeed;
    public float afterBallSpeed;

    //�� ũ��Ƽ��
    public float ballCritical;

    //�� ����
    public int ballCount;

    public Player()
    {
        gold = 0f;

        beforeBallDamage = 0;
        currentBallDamage = 1;
        afterBallDamage = 2;

        beforeBallSpeed = 0;
        currentBallSpeed = 1;
        afterBallSpeed = 2;

        ballCritical = 1.5f;
        ballCount = 1;

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
        worldLevel++;
    }

    public void CheckLevelUp()
    {
        if(currentKillMonsterCount == Define.NeedLevelUpCount)
        {
            currentKillMonsterCount = 0;
            Managers.Game.LevelUpEvent();
        }
    }

    public void CheckBallCount()
    {
        if (Managers.Game.player.ballCount < Managers.Ball.GetBallCount(Define.BallType.Default))
            return;
        while (Managers.Game.player.ballCount != Managers.Ball.GetBallCount(Define.BallType.Default))
            Managers.Ball.CreateBall(Define.BallType.Default);
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

public class UpgradeSystem
{
    private float tempFloat = 0f;

    //�� ������
    private float beforeBallDamageUpgradeCost = 10f;
    public float currentBallDamageUpgradeCost = 20f;

    //�� ���ǵ�
    private float beforeBallSpeedUpgradeCost = 10f;
    public float currentBallSpeedUpgradeCost = 20f;

    //�� ����
    private float beforeBallCountUpgradeCost = 10f;
    public float currentBallCountUpgradeCost = 20f;


    //�� ũ��Ƽ��
    private float beforeBallCiriticalUpgradeCost = 10f;
    public float currentBallCriticalUpgradeCost = 20f;

    //�� ������ ���׷��̵�
    public void BallDamageUpgrade()
    {
        if(Managers.Game.player.gold > currentBallDamageUpgradeCost)
        {
            //���� ����
            Managers.Game.RemoveGold(currentBallDamageUpgradeCost);

            //�Ǻ���ġ ���� ����
            tempFloat = beforeBallDamageUpgradeCost + currentBallDamageUpgradeCost;
            beforeBallDamageUpgradeCost = currentBallDamageUpgradeCost;
            currentBallDamageUpgradeCost = tempFloat;

            //���׷��̵�
            Managers.Game.player.ballDamageLevel++;

            Managers.Game.player.beforeBallDamage = Managers.Game.player.currentBallDamage;
            Managers.Game.player.currentBallDamage = Managers.Game.player.afterBallDamage;
            Managers.Game.player.afterBallDamage = Managers.Game.player.beforeBallDamage + Managers.Game.player.currentBallDamage;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //�� ���ǵ� ���׷��̵�
    public void BallSpeedUpgrade()
    {
        if (Managers.Game.player.gold > currentBallSpeedUpgradeCost)
        {
            //���� ����
            Managers.Game.RemoveGold(currentBallSpeedUpgradeCost);

            //�Ǻ���ġ ���� ����
            tempFloat = beforeBallSpeedUpgradeCost + currentBallSpeedUpgradeCost;
            beforeBallSpeedUpgradeCost = currentBallSpeedUpgradeCost;
            currentBallSpeedUpgradeCost = tempFloat;

            //���׷��̵�
            Managers.Game.player.ballSpeedLevel++;

            Managers.Game.player.beforeBallSpeed = Managers.Game.player.currentBallSpeed;
            Managers.Game.player.currentBallSpeed = Managers.Game.player.afterBallSpeed;
            Managers.Game.player.afterBallSpeed = Managers.Game.player.beforeBallSpeed + Managers.Game.player.currentBallSpeed;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //�� ũ��Ƽ�� ���׷��̵�
    public void BallCriticalUpgrade()
    {
        if (Managers.Game.player.gold > currentBallCriticalUpgradeCost)
        {
            //���� ����
            Managers.Game.RemoveGold(currentBallCriticalUpgradeCost);

            //�Ǻ���ġ ���� ����
            tempFloat = beforeBallCiriticalUpgradeCost + currentBallCriticalUpgradeCost;
            beforeBallCiriticalUpgradeCost = currentBallCriticalUpgradeCost;
            currentBallCriticalUpgradeCost = tempFloat;

            //���׷��̵�
            Managers.Game.player.ballCriticalLevel++;
            Managers.Game.player.ballCritical += 0.05f;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //�� ���� ���׷��̵�
    public void BallCountUpgrade()
    {
        if (Managers.Game.player.gold > currentBallCountUpgradeCost)
        {
            //���� ����
            Managers.Game.RemoveGold(currentBallCountUpgradeCost);

            //�Ǻ���ġ ���� ����
            tempFloat = beforeBallCountUpgradeCost + currentBallCountUpgradeCost;
            beforeBallCountUpgradeCost = currentBallCountUpgradeCost;
            currentBallCountUpgradeCost = tempFloat;

            //���׷��̵�
            Managers.Game.player.ballCountLevel++;
            Managers.Game.player.ballCount += 1;

            Managers.Game.world.CheckBallCount();
            Managers.UI.SceneUI?.RedrawUI();
        }
    }
}


