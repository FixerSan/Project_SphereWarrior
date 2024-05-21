using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO::나중에 추상으로 만들거임
public class Face 
{
    public FaceData data;

    public Face(int _level)
    {
        data = new FaceData(_level);
    }


    //TODO::추상 함수 할 거임
    public void CollisionFaceEffect()
    {
        Managers.Game.GetGold(data.addGoldValue);
    }
}

public class FaceData
{
    public FaceData(int _level)
    {
        level = _level;
        addGoldValue = _level * 1.5f;
    }

    public int level;
    public float addGoldValue;
}
