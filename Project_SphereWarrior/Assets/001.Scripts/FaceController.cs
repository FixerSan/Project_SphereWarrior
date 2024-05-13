using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    public Define.FaceType faceType;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Debug.Log("����");
            Managers.Game.CollisionFace(faceType);
        }
    }
}
