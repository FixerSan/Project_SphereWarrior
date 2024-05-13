using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float swipeForceDownForce = 0.75f;
    public void FixedUpdate()
    {
        Debug.Log(Managers.Input.swipeForce);
        if(Managers.Input.swipeForce != 0f)
        {
            Managers.Input.swipeForce *= swipeForceDownForce;
            if (Managers.Input.swipeForce <= 0.1f)
                Managers.Input.swipeForce = 0f;
        }
    }
}
[System.Serializable]
public class Player
{

}


