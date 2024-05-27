using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager
{
    #region 오브젝트 참조
    public HashSet<GridObject> gridObjects = new HashSet<GridObject>();
    public HashSet<MonsterController> monsters = new HashSet<MonsterController>();
    #endregion

    #region Trans
    public Transform MonsterTrans
    {
        get
        {
            if(monsterTrans == null)
            {
                GameObject go = GameObject.Find("@Monster");
                if(go == null) go = new GameObject("@Monster");
                monsterTrans = go.transform;
            }
            return monsterTrans;
        }
    }
    private Transform monsterTrans;
    #endregion

    public Transform ParticleTrans
    {
        get
        {
            if (particleTrans == null)
            {
                GameObject go = GameObject.Find("@Particle");
                if (go == null) go = new GameObject("@Particle");
                particleTrans = go.transform;
            }
            return particleTrans;
        }
    }
    private Transform particleTrans;

    public GridObject SpawnGridObject(int _index, float _baseHP, int _gridIndexX, int _gridIndexY, int _gridIndexZ)
    {
        GridObject block = Managers.Resource.Instantiate($"Block_{_index}", MonsterTrans, true).GetComponent<GridObject>();
        block.transform.position = (new Vector3(_gridIndexX, _gridIndexY, _gridIndexZ) * Define.gridScale.x * 2) + Define.gridOffset;
        block.transform.localScale = Define.gridScale;
        block.Init(_baseHP);
        gridObjects.Add(block);
        Managers.Grid.EnGrid(block, _gridIndexX, _gridIndexY, _gridIndexZ);
        return block;
    }

    public MonsterController SpawnMonster(Define.WorldTheme _themeType, Define.MonsterType _monsterType)
    {
        MonsterController monster= Managers.Resource.Instantiate($"Monster_{_themeType}_{_monsterType}", monsterTrans, true).GetComponent<MonsterController>();
        monster.transform.position = Managers.Grid.EnGridRandomEmptyGrid(monster);
        monster.transform.localScale = Define.gridScale;
        monster.Init(Define.BaseMonsterHP * Managers.Game.world.worldLevel, _monsterType);
        monsters.Add(monster);
        return monster;
    }

    public void DespawnMonster(MonsterController _monster)
    {
        Managers.Grid.DeGrid(_monster);
        monsters.Remove(_monster);
    }
}
