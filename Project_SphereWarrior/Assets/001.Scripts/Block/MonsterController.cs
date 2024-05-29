using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : GridObject
{
    public Monster monster;
    public Define.MonsterType type;
    private Animator anim;
    private Rigidbody rb;
    private Collider coll;

    public float deadMoveForce;
    public float deadTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coll = Util.FindChild<Collider>(gameObject, "Collider" , true);
    }

    public void Init(float _hp, Define.MonsterType _monster)
    {
        base.Init(_hp);
        type = _monster;
        rb.isKinematic = true;
        coll.enabled = true;
    }

    public override void Dead()
    {
        hp = 0;
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * deadMoveForce, ForceMode.Impulse);
        coll.enabled = false;
        Managers.Game.world.KillMonster(this);
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(deadTime);
        Managers.Resource.Destroy(gameObject);
    }

    public override void Hit(float _damage)
    {
        HP -= _damage;
        anim.SetTrigger("Hit");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Hit(Util.CriticalCheck(Managers.Game.player.currentBallDamage));
        }
    }

}

public class Monster
{

}
