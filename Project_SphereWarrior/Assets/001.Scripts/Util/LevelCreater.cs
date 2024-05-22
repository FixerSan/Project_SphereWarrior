using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreater : MonoBehaviour
{
    public LevelData levelData;
    public int levelIndex;
    public int levelBaseHP;

    [ContextMenu("Create")]
    public void CreateLevel()
    {
        if(levelBaseHP == 0)
        {
            Debug.Log("LevelBaseHP�� 0 �Դϴ�. ���� �־��ּ���");
            return;
        }

        Managers.Grid.EnGridObjectInScene();
        levelData = Managers.Grid.CreateLevelData(levelBaseHP);
        levelData.index = levelIndex;
        for (int x = 0; x < levelData.indexes.GetLength(0); x++)
        {
            for (int y = 0; y < levelData.indexes.GetLength(1); y++)
            {
                for (int z = 0; z < levelData.indexes.GetLength(2); z++)
                {
                    if (levelData.indexes[x, y, z] != -1)
                        Debug.Log(levelData.indexes[x, y, z]);
                }
            }
        }

        if(!Managers.Data.levelDatas.TryAdd(levelIndex, levelData))
        {
            Debug.Log("���� ���忡 �����߽��ϴ�. �ٸ� �ε����� ������ּ���");
            return;
        }
        Managers.Data.SaveLevelData();
    }


}

