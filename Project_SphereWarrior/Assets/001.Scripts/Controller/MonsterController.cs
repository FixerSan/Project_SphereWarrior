using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Monster monster;
    public Animator anim;
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
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(float _hp)
    {
        hp = _hp;
    }

    private void Dead()
    {
        hp = 0;
        Managers.Resource.Destroy(gameObject);
    }

    public void Hit(float _damage)
    {
        HP -= _damage;
        anim.SetTrigger("Hit");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Hit(Managers.Game.player.attackForce);
        }
    }

}

public class Monster
{

}
