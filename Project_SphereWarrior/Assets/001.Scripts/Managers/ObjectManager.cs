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

    public MonsterController SpawnMonster(int _index, float _hp,  Vector3 _position)
    {
        MonsterController monster = Managers.Resource.Instantiate($"Monster_{_index}", MonsterTrans, true).GetComponent<MonsterController>();
        monster.transform.position = _position;
        monster.Init(_hp);
        monsters.Add(monster);
        return monster;
    }
}
