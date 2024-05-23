using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BallManager
{
    public Dictionary<Define.BallType, HashSet<Ball>> balls = new Dictionary<Define.BallType, HashSet<Ball>>();

    private Transform ballTrans;
    public Transform BallTrans
    {
        get
        {
            if(ballTrans == null)
            {
                GameObject go = GameObject.Find("@Ball");
                if (go == null)
                    go = new GameObject("@Ball");
                ballTrans = go.transform;
            }
            return ballTrans;
        }
    }

    public void MoveToEmptyGrid()
    {
        foreach (var ballhash in balls.Values)
        {
            foreach (var ball in ballhash)
            {
                ball.transform.position = Managers.Grid.FindNearEmptyGridPos(ball.transform.position);
                ball.SetRandomDirection();
            }
        }
    }

    public void CreateBall(Define.BallType _ballType)
    {
        Ball ball = Managers.Resource.Instantiate($"Ball_{_ballType}", BallTrans, true).GetComponent<Ball>();
        if (!balls.ContainsKey(_ballType))
            balls.Add(_ballType, new HashSet<Ball>());
        balls[_ballType].Add(ball);
        ball.Init(Managers.Game.player.ballSpeeds[_ballType]);
    }
}
