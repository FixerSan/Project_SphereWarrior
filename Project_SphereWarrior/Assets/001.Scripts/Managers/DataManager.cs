using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager 
{
    public Dictionary<int, LevelData> levelDatas = new Dictionary<int, LevelData>();

    public void LoadLevelData()
    {
        TextAsset text = Managers.Resource.Load<TextAsset>("LevelData");
        LevelDatas datas = JsonUtility.FromJson<LevelDatas>(text.text);
        for (int i = 0; i < datas.datas.Length; i++)
            levelDatas.Add(datas.datas[i].index, datas.datas[i]);
    }

    public void SaveLevelData()
    {
        LevelDatas datas = new LevelDatas();
        datas.datas = new LevelData[levelDatas.Count];


        int loopCount = 0;
        foreach (var levelData in levelDatas.Values)
        {
            datas.datas[loopCount] = levelData;
            loopCount++;
        }

        string dataJson = JsonUtility.ToJson(datas, true);
        string path = Path.Combine(Application.dataPath, "008.Datas/LevelData.json");

        File.WriteAllText(path, dataJson);
    }

    public LevelData GetLevelData(int _index)
    {
        if (levelDatas.TryGetValue(_index, out LevelData data))
            return data;
        return null;
    }
}

[Serializable]
public class LevelDatas
{
    public LevelData[] datas;
}
