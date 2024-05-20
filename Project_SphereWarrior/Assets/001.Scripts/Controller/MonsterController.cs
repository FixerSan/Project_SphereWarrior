using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Monster monster;
    private Animator anim;
    private Rigidbody rb;
    private Collider coll;
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

    public float deadMoveForce;
    public float deadTime;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void Init(float _hp)
    {
        hp = _hp;
        rb.isKinematic = true;
        coll.enabled = true;
    }

    private void Dead()
    {
        hp = 0;
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * deadMoveForce, ForceMode.Impulse);
        coll.enabled = false;
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(deadTime);
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
