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
        if (_gridObject == null) return;
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
        grid.baseHP = data.baseHP;
        LoopGridLength(SetGrid);
    }

    private void SetGrid(int _x, int _y, int _z)
    {
        if (data.indexes[_x, _y, _z] == -1) return;
        Managers.Object.SpawnGridObject(data.indexes[_x, _y, _z], grid.baseHP, _x, _y, _z);
        Managers.Ball.MoveToEmptyGrid();
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
            Debug.LogError($"{new Vector3(_x, _y, _z) * Define.gridScale.x * 2 + Define.gridOffset} 좌표의 오브젝트가 겹칩니다. 위치를 다시 설정해주세요");
            return;
        }

        if (colliders.Length == 0) return;

        grid.gridObjectArray[_x,_y,_z] = colliders[0].GetComponent<GridObject>();
    }

    public Vector3 GridToWorldPos(int _x, int _y, int _z)
    {
        return new Vector3(_x, _y, _z) * Define.gridScale.x * 2 + Define.gridOffset;
    }

    public Vector3 FindNearEmptyGridPos (Vector3 _target)
    {
        Vector3 nearEmptyGridPos = new Vector3(1000, 1000, 1000);
        float nowDistance;

        for (int x = 0; x < grid.gridObjectArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridObjectArray.GetLength(0); y++)
            {
                for (int z = 0; z < grid.gridObjectArray.GetLength(0); z++)
                {
                    if (grid.gridObjectArray[x, y, z] == null)
                    {
                        nowDistance = Vector3.Distance(_target, GridToWorldPos(x, y, z));
                        if (Vector3.Distance(_target, nearEmptyGridPos) > nowDistance)
                            nearEmptyGridPos = GridToWorldPos(x, y, z);
                        continue;
                    }
                }
            }
        }
        return nearEmptyGridPos;
    }

    //비어있는 랜덤 그리드에 넣기
    public Vector3 EnGridRandomEmptyGrid(GridObject _monsterController)
    {
        int x = 0, y = 0, z = 0;
        bool isNull = false;

        while (!isNull)
        {
            x = UnityEngine.Random.Range(0, grid.gridObjectArray.GetLength(0));
            y = UnityEngine.Random.Range(0, grid.gridObjectArray.GetLength(1));
            z = UnityEngine.Random.Range(0, grid.gridObjectArray.GetLength(2));

            if(grid.gridObjectArray[x, y, z] == null)
            {
                Collider[] colliders = Physics.OverlapBox(GridToWorldPos(x, y, z), Define.gridScale / 2);
                if(colliders.Length == 0)
                {
                    isNull = true;
                    break;
                }
            }
        }

        EnGrid(_monsterController, x, y, z);
        return GridToWorldPos(x, y, z);
    }

    public LevelData CreateLevelData(float _baseHP)
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
                }
            }
        }
        data.baseHP = _baseHP;
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
    public float baseHP;
}

[System.Serializable]
public class LevelData
{
    public int index;
    public string stageInfo;
    public float baseHP;
    public int[,,] indexes = new int[5, 5, 5];
}
