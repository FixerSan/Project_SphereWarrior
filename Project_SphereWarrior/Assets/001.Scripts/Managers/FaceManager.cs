using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceManager
{
    public Face[] faces = new Face[6];

    public void InitFace()
    {
        Player player = Managers.Game.player;
        faces[0] = InitFace(player.faceOneType, player.faceOneLevel);
        faces[1] = InitFace(player.faceTwoType, player.faceTwoLevel);
        faces[2] = InitFace(player.faceThreeType, player.faceThreeLevel);
        faces[3] = InitFace(player.faceFourType, player.faceFourLevel);
        faces[4] = InitFace(player.faceFiveType, player.faceFiveLevel);
        faces[5] = InitFace(player.faceSixType, player.faceSixLevel);
    }

    private Face InitFace(Define.FaceType _type, int _level)
    {
        Face face;
        switch (_type)
        {
            case Define.FaceType.Default:
                face = new Faces.Default(_level);
                return face;

            case Define.FaceType.Test:
                face = new Faces.TestFace(_level);
                return face;
        }
        Debug.LogError($"{_type}의 타입의 초기화가 FaceManager에 존재하지 않습니다.");
        return null;
    }
}
