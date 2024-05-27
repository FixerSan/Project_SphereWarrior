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
            player = new Player(); //Todo::나중에 저장된 녀석 불러오는 코드로 수정
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
        SetIngameGold();

        Managers.UI.SceneUI?.RedrawUI();
    }

    //돈 얻는 코드
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
    //플레이어 골드
    public float gold;
    public string inGameGoldString;
    public float cash;

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

    //볼 레벨
    public int ballDamageLevel = 1;
    public int ballSpeedLevel = 1;
    public int ballCountLevel = 1;
    public int ballCriticalLevel = 1;

    //볼 데미지
    public float beforeBallDamage;
    public float currentBallDamage;
    public float afterBallDamage;

    //볼 스피드
    public float beforeBallSpeed;
    public float currentBallSpeed;
    public float afterBallSpeed;

    //볼 크리티컬
    public float ballCritical;

    //볼 갯수
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

public class UpgradeSystem
{
    private float tempFloat = 0f;

    //볼 데미지
    private float beforeBallDamageUpgradeCost = 10f;
    public float currentBallDamageUpgradeCost = 20f;

    //볼 스피드
    private float beforeBallSpeedUpgradeCost = 10f;
    public float currentBallSpeedUpgradeCost = 20f;

    //볼 갯수
    private float beforeBallCountUpgradeCost = 10f;
    public float currentBallCountUpgradeCost = 20f;


    //볼 크리티컬
    private float beforeBallCiriticalUpgradeCost = 10f;
    public float currentBallCriticalUpgradeCost = 20f;

    //볼 데미지 업그레이드
    public void BallDamageUpgrade()
    {
        if(Managers.Game.player.gold > currentBallDamageUpgradeCost)
        {
            //가격 차감
            Managers.Game.RemoveGold(currentBallDamageUpgradeCost);

            //피보나치 가격 수정
            tempFloat = beforeBallDamageUpgradeCost + currentBallDamageUpgradeCost;
            beforeBallDamageUpgradeCost = currentBallDamageUpgradeCost;
            currentBallDamageUpgradeCost = tempFloat;

            //업그레이드
            Managers.Game.player.ballDamageLevel++;

            Managers.Game.player.beforeBallDamage = Managers.Game.player.currentBallDamage;
            Managers.Game.player.currentBallDamage = Managers.Game.player.afterBallDamage;
            Managers.Game.player.afterBallDamage = Managers.Game.player.beforeBallDamage + Managers.Game.player.currentBallDamage;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //볼 스피드 업그레이드
    public void BallSpeedUpgrade()
    {
        if (Managers.Game.player.gold > currentBallSpeedUpgradeCost)
        {
            //가격 차감
            Managers.Game.RemoveGold(currentBallSpeedUpgradeCost);

            //피보나치 가격 수정
            tempFloat = beforeBallSpeedUpgradeCost + currentBallSpeedUpgradeCost;
            beforeBallSpeedUpgradeCost = currentBallSpeedUpgradeCost;
            currentBallSpeedUpgradeCost = tempFloat;

            //업그레이드
            Managers.Game.player.ballSpeedLevel++;

            Managers.Game.player.beforeBallSpeed = Managers.Game.player.currentBallSpeed;
            Managers.Game.player.currentBallSpeed = Managers.Game.player.afterBallSpeed;
            Managers.Game.player.afterBallSpeed = Managers.Game.player.beforeBallSpeed + Managers.Game.player.currentBallSpeed;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //볼 크리티컬 업그레이드
    public void BallCriticalUpgrade()
    {
        if (Managers.Game.player.gold > currentBallCriticalUpgradeCost)
        {
            //가격 차감
            Managers.Game.RemoveGold(currentBallCriticalUpgradeCost);

            //피보나치 가격 수정
            tempFloat = beforeBallCiriticalUpgradeCost + currentBallCriticalUpgradeCost;
            beforeBallCiriticalUpgradeCost = currentBallCriticalUpgradeCost;
            currentBallCriticalUpgradeCost = tempFloat;

            //업그레이드
            Managers.Game.player.ballCriticalLevel++;
            Managers.Game.player.ballCritical += 0.05f;

            Managers.UI.SceneUI?.RedrawUI();
        }
    }

    //볼 갯수 업그레이드
    public void BallCountUpgrade()
    {
        if (Managers.Game.player.gold > currentBallCountUpgradeCost)
        {
            //가격 차감
            Managers.Game.RemoveGold(currentBallCountUpgradeCost);

            //피보나치 가격 수정
            tempFloat = beforeBallCountUpgradeCost + currentBallCountUpgradeCost;
            beforeBallCountUpgradeCost = currentBallCountUpgradeCost;
            currentBallCountUpgradeCost = tempFloat;

            //업그레이드
            Managers.Game.player.ballCountLevel++;
            Managers.Game.player.ballCount += 1;

            Managers.Game.world.CheckBallCount();
            Managers.UI.SceneUI?.RedrawUI();
        }
    }
}


