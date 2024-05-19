using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Monster monster;
    public float HP 
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
            if (hp <= 0)
                Dead();
        }
    }
    private float hp;

    public void Init(int _hp)
    {
        hp = _hp;
    }

    private void Dead()
    {
        hp = 0;
        Managers.Resource.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            HP -= Managers.Game.player.attackForce;
        }
    }
}

public class Monster
{

}
