using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    public int index;
    public int gridIndexX;
    public int gridIndexY;
    public int gridIndexZ;


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
    protected float hp;

    public virtual void Init(float _hp)
    {
        hp = _hp;
    }

    public void SetGridIndex(int _x, int _y, int _z)
    {
        gridIndexX = _x;
        gridIndexY = _y;    
        gridIndexZ = _z;
    }

    public virtual void Destroy()
    {
        Managers.Grid.DeGrid(this);
    }

    public abstract void Dead();

    public abstract void Hit(float _damage);

}
