using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreater : MonoBehaviour
{
    private LevelData levelData;
    public int levelIndex;
    public int levelBaseHP;

    [ContextMenu("Create")]
    public void CreateLevel()
    {
        if(levelBaseHP == 0)
        {
            Debug.LogError("LevelBaseHP�� 0 �Դϴ�. ���� �־��ּ���");
            return;
        }

        Managers.Grid.EnGridObjectInScene();
        levelData = Managers.Grid.CreateLevelData(levelBaseHP);
        levelData.index = levelIndex;

        if(!Managers.Data.levelDatas.TryAdd(levelIndex, levelData))
        {
            Debug.LogError("���� ���忡 �����߽��ϴ�. �ٸ� �ε����� ������ּ���");
            return;
        }
        Managers.Data.SaveLevelData();
        Debug.Log("���� ���忡 �����߽��ϴ�.");

    }


}

