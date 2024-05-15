using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    public Vector3 SwipeDirection { get { return swipeDirection; } }
    private Vector2 swipeDirection;
    public float SwipeForce { get { return swipeForce; } }
    private float swipeForce;

    public float swipeForceDownForce = 0.75f;


    public void SetSwipeDirection(Vector3 _swipeDir)
    {
        swipeDirection = _swipeDir;
    }

    public void SetSwipeForce(float _swipeForce)
    {
        swipeForce = _swipeForce;
    }

    //스와이프 힘을 천천히 줄이는 코드
    public void DownSwipeForce()
    {
        if (Managers.Input.SwipeForce != 0f)
        {
            Managers.Input.SetSwipeForce(Managers.Input.SwipeForce * swipeForceDownForce);
            if (Managers.Input.SwipeForce <= 0.1f)
                Managers.Input.SetSwipeForce(0f);
        }
    }
}
