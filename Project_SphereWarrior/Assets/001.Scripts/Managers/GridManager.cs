using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager 
{
    private Grid grid = new Grid();
    private LevelData data = new LevelData();

    public void EnGrid(GridObject _gridObject, int _x, int _y, int _z)
    {
        grid.gridObjectArray[_x, _y, _z] = _gridObject;
        _gridObject.SetGridIndex(_x, _y, _z);
    }

    public void DeGrid(GridObject _gridObject)
    {
        grid.gridObjectArray[_gridObject.gridIndexX, _gridObject.gridIndexY, _gridObject.gridIndexZ] = null;
    }
    public void SetGrid(LevelData _data)
    {
        data = _data;
        LoopGridLength(SetGrid);
    }

    private void SetGrid(int _x, int _y, int _z)
    {
        Managers.Object.SpawnGridObject(data.indexes[_x, _y, _z], data.hpDatas[_x, _y, _z], _x, _y, _z);
    }


    public void EnGridObjectInScene()
    {
        LoopGridLength(EnGridObjectInScene);
    }

    private void EnGridObjectInScene(int _x, int _y, int _z)
    {
        Collider[] colliders = Physics.OverlapSphere((new Vector3(_x, _y, _z) * Define.gridScale.x * 2) + Define.gridOffset, 0.01f);

        if (colliders.Length >= 2)
        {
            Debug.LogError($"{_x} , {_y} , {_z} 좌표의 오브젝트가 겹칩니다. 위치를 다시 설정해주세요");
            return;
        }

        if (colliders.Length == 0) return;

        grid.gridObjectArray[_x,_y,_z] = colliders[0].GetComponent<GridObject>();
    }

    public LevelData CreateLevelData()
    {
        for (int x = 0; x < grid.gridObjectArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridObjectArray.GetLength(0); y++)
            {
                for (int z = 0; z < grid.gridObjectArray.GetLength(0); z++)
                {
                    if (grid.gridObjectArray[x,y,z] == null)
                    {
                        data.indexes[x, y, z] = -1;
                        continue;
                    }

                    data.indexes[x, y, z] = grid.gridObjectArray[x, y, z].index;
                    data.hpDatas[x, y, z] = grid.gridObjectArray[x, y, z].HP;
                }
            }
        }
        return data;
    }

    public void LoopGridLength(Action<int, int, int> _callback)
    {
        for (int x = 0; x < grid.gridObjectArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridObjectArray.GetLength(0); y++)
            {
                for (int z = 0; z < grid.gridObjectArray.GetLength(0); z++)
                {
                    _callback.Invoke(x, y, z);
                }
            }
        }
    }
}

public class Grid
{
    public GridObject[,,] gridObjectArray = new GridObject[5,5,5];
}

[System.Serializable]
public class LevelData
{
    public int index;
    public int[,,] indexes = new int[5, 5, 5];
    public float[,,] hpDatas = new float[5, 5, 5];
}
