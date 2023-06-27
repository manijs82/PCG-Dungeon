using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid<TGridObject> where TGridObject : GridObject
{
    public delegate void GridValueChangeHandler(int x, int y);

    public event GridValueChangeHandler OnGridValueChange;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private TGridObject[,] gridArray;
    private TextMeshPro[,] debugObj;

    public int Height => height;
    public int Width => width;
    public float OriginX => origin.x;
    public float OriginY => origin.y;
    public float CellSize => cellSize;

    public TGridObject[,] GridObjects => gridArray;

    public Vector3 Origin => origin;

    public Grid(int width, int height, float cellSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject,
        Vector3 origin = default, bool shouldDebug = false)
    {
        this.width = (int)(width / cellSize);
        this.height = (int)(height / cellSize);
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new TGridObject[this.width, this.height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        if (shouldDebug)
            EnableDebug();
    }

    private void EnableDebug()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white,
                    Mathf.Infinity);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white,
                    Mathf.Infinity);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, Mathf.Infinity);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, Mathf.Infinity);
    }

    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize + origin;

    private void GetXY(Vector3 pos, out int x, out int y)
    {
        x = Mathf.FloorToInt((pos - origin).x / cellSize);
        y = Mathf.FloorToInt((pos - origin).y / cellSize);
    }
    
    private Vector3 GetXY(Vector3 pos)
    {
        float x = Mathf.FloorToInt((pos - origin).x / cellSize);
        float y = Mathf.FloorToInt((pos - origin).y / cellSize);

        return new Vector3(x, y);
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            OnGridValueChange?.Invoke(x, y);
        }
    }

    public void SetValue(Vector3 worldPos, TGridObject value)
    {
        GetXY(worldPos, out var x, out var y);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
            return default(TGridObject);
    }

    public TGridObject GetValue(Vector3 worldPos)
    {
        GetXY(worldPos, out var x, out var y);
        return GetValue(x, y);
    }

    public bool HasValue(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool HasValue(int x, int y, out TGridObject value)
    {
        if (HasValue(x, y))
        {
            value = gridArray[x, y];
            return true;
        }

        value = null;
        return false;
    }

    public TGridObject[] GetAll()
    {
        var o = new TGridObject[height * width];
        int i = 0;
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                o[i] = gridArray[x, y];
                i++;
            }
        }

        return o;
    }

    private bool neighborAlter;

    public List<TGridObject> Get4Neighbors(int x, int y) =>
        Get4Neighbors(gridArray[x, y]);

    public List<TGridObject> Get4Neighbors(TGridObject center)
    {
        var output = new List<TGridObject>();
        var temp = new List<TGridObject>();

        neighborAlter = !neighborAlter;

        if(neighborAlter)
        {
            temp.Add(GetValue(center.x, center.y + 1));
            temp.Add(GetValue(center.x, center.y - 1));
            temp.Add(GetValue(center.x + 1, center.y));
            temp.Add(GetValue(center.x - 1, center.y));
        }
        else
        {
            temp.Add(GetValue(center.x, center.y - 1));
            temp.Add(GetValue(center.x, center.y + 1));
            temp.Add(GetValue(center.x - 1, center.y));
            temp.Add(GetValue(center.x + 1, center.y));
        }

        foreach (var p in temp)
        {
            if (p == null) continue;
            if (!p.IsBlocked)
                output.Add(p);
        }

        return output;
    }

    public List<TGridObject> Get8Neighbors(TGridObject center) => 
        Get8Neighbors(center.x, center.y);

    public List<TGridObject> Get8Neighbors(int x, int y)
    {
        return GetNeighbors(x, y, new Bound(1, 1, 1, 1), true);
    }
    
    public List<TGridObject> GetNeighbors(int x, int y, Bound extents, bool checkBlocked = false)
    {
        var output = new List<TGridObject>();
        
        for (int i = x - extents.x; i <= x + extents.w; i++)
        {
            for (int j = y - extents.y; j <= y + extents.h; j++)
            {
                if(i == x && j == y) continue;
                if (!HasValue(i, j, out TGridObject gridObject)) continue;
                if(checkBlocked && gridObject.IsBlocked) continue;
                output.Add(gridObject);
            }
        }

        return output;
    }

    public List<TGridObject> GetGridObjectsWithCondition(Func<Grid<TGridObject>, int, int, bool> conditionFunction)
    {
        List<TGridObject> list = new List<TGridObject>();

        foreach (var gridObject in gridArray)
        {
            if(conditionFunction(this, gridObject.x, gridObject.y))
                list.Add(gridObject);
        }

        return list;
    }
    
    public TGridObject GetRandomGridObjectWithCondition(Func<Grid<TGridObject>, int, int, bool> conditionFunction)
    {
        var list = GetGridObjectsWithCondition(conditionFunction);
        if (list.Count == 0) return null;
        
        return list[Random.Range(0, list.Count)];
    }

    public void PlaceGridOnGrid(int startX, int startY, Grid<TGridObject> grid)
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                SetValue(startX + x, startY + y, grid.GetValue(x, y));
            }
        }
    }

    public Bound GetBound() => 
        new Bound((int)origin.x, (int)origin.y, width, height);
    
#if UNITY_EDITOR
    public void DrawGizmos(Func<TGridObject, Color> colorFunc)
    {
        Handles.color = Color.white;
        Color color = Color.black;
        
        foreach (var obj in GridObjects)
        {
            color = colorFunc(obj);
            
            Handles.DrawSolidRectangleWithOutline(new Rect(Origin.x + obj.x, Origin.y + obj.y, CellSize, CellSize), color, color);
        }
    }
#endif
}