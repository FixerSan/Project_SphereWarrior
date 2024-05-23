using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Face 
{
    public FaceData data;

    public Face(int _level)
    {
        data = new FaceData(_level);
    }


    public virtual void CollisionFaceEffect()
    {
        Managers.Game.GetGold(data.addGoldValue);
    }
}

namespace Faces
{
    public class Default : Face
    {
        public Default(int _level) : base(_level)
        {

        }

        public override void CollisionFaceEffect()
        {
            base.CollisionFaceEffect();
        }
    }

    public class TestFace : Face
    {
        public TestFace(int _level) : base(_level)
        {

        }

        public override void CollisionFaceEffect()
        {
            base.CollisionFaceEffect();
            Debug.Log("WallEffect Test");
        }
    }
}
