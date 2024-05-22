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
            Debug.LogError("LevelBaseHP가 0 입니다. 값을 넣어주세요");
            return;
        }

        Managers.Grid.EnGridObjectInScene();
        levelData = Managers.Grid.CreateLevelData(levelBaseHP);
        levelData.index = levelIndex;

        if(!Managers.Data.levelDatas.TryAdd(levelIndex, levelData))
        {
            Debug.LogError("레벨 저장에 실패했습니다. 다른 인덱스를 사용해주세요");
            return;
        }
        Managers.Data.SaveLevelData();
        Debug.Log("레벨 저장에 성공했습니다.");

    }


}

