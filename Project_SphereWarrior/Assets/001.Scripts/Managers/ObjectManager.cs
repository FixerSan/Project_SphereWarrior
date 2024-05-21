using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager
{
    #region 오브젝트 참조
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


    public MonsterController SpawnMonster(int _index, float _hp,  int _gridIndexX, int _gridIndexY, int _gridIndexZ)
    {
        MonsterController monster = Managers.Resource.Instantiate($"Monster_{_index}", MonsterTrans, true).GetComponent<MonsterController>();
        monster.transform.position = (new Vector3(_gridIndexX, _gridIndexY, _gridIndexZ) * Define.gridScale.x * 2) + Define.gridOffset;
        monster.transform.localScale = Define.gridScale;
        monster.Init(_hp);
        monsters.Add(monster);
        return monster;
    }

    public GridObject SpawnGridObject(int _index, float _hp, int _gridIndexX, int _gridIndexY, int _gridIndexZ)
    {
        GridObject block = Managers.Resource.Instantiate($"Block_{_index}", MonsterTrans, true).GetComponent<GridObject>();
        block.transform.position = (new Vector3(_gridIndexX, _gridIndexY, _gridIndexZ) * Define.gridScale.x * 2) + Define.gridOffset;
        block.transform.localScale = Define.gridScale;
        block.Init(_hp);
        Managers.Grid.EnGrid(block, _gridIndexX, _gridIndexY, _gridIndexZ);
        return block;
    }
}
