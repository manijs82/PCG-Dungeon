using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class Grid<TGridObject> where TGridObject : GridObject
{
    public delegate void GridValueChangeHandler(int x, int y);

    public event GridValueChangeHandler OnGridValueChange;

    protected int width;
    protected int height;
    protected float cellSize;
    protected Vector3 origin;
    protected Vector2Int originInt;
    protected TGridObject[,] gridArray;
    protected TextMeshPro[,] debugObj;

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
        originInt = new Vector2Int((int) origin.x, (int) origin.y);

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

    public void GetGridPosition(Vector3 pos, out int x, out int y)
    {
        x = Mathf.FloorToInt((pos - origin).x / cellSize);
        y = Mathf.FloorToInt((pos - origin).y / cellSize);
    }
    
    public Vector2 GetGridPosition(Vector3 pos)
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
        GetGridPosition(worldPos, out var x, out var y);
        SetValue(x, y, value);
    }
    
    public void SetValue(int x, int y, Bound extents, Func<int, int, TGridObject> createGridObject)
    {
        for (int i = x - extents.x; i <= x + extents.w; i++)
        {
            for (int j = y - extents.y; j <= y + extents.h; j++)
            {
                if (!HasValue(i, j)) continue;
                SetValue(i, j, createGridObject(i, j));
            }
        }
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
        GetGridPosition(worldPos, out var x, out var y);
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

    public IEnumerable<TGridObject> GetAll()
    {
        int i = 0;
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                yield return gridArray[x, y];
                i++;
            }
        }
    }

    private bool neighborAlter;

    public List<TGridObject> Get4Neighbors(int x, int y, bool ignoreBlocked = false) =>
        Get4Neighbors(gridArray[x, y], ignoreBlocked);

    public List<TGridObject> Get4Neighbors(TGridObject center, bool ignoreBlocked = false)
    {
        var output = new List<TGridObject>();
        var temp = new List<TGridObject>();

        neighborAlter = !neighborAlter;

        int x = center.x - originInt.x;
        int y = center.y - originInt.y;

        if(neighborAlter)
        {
            temp.Add(GetValue(x, y + 1));
            temp.Add(GetValue(x, y - 1));
            temp.Add(GetValue(x + 1, y));
            temp.Add(GetValue(x - 1, y));
        }
        else
        {
            temp.Add(GetValue(x, y - 1));
            temp.Add(GetValue(x, y + 1));
            temp.Add(GetValue(x - 1, y));
            temp.Add(GetValue(x + 1, y));
        }

        foreach (var p in temp)
        {
            if (p == null) continue;
            if (p.IsBlocked && !ignoreBlocked) continue;
            output.Add(p);
        }

        return output;
    }

    public IEnumerable<TGridObject> Get8Neighbors(TGridObject center) => 
        Get8Neighbors(center.x, center.y);

    public IEnumerable<TGridObject> Get8Neighbors(int x, int y)
    {
        return GetNeighbors(x, y, new Bound(1, 1, 1, 1), true);
    }
    
    public IEnumerable<TGridObject> GetNeighbors(int x, int y, Bound extents, bool checkBlocked = false)
    {
        for (int i = x - extents.x; i <= x + extents.w; i++)
        {
            for (int j = y - extents.y; j <= y + extents.h; j++)
            {
                if(i == x && j == y) continue;
                if (!HasValue(i, j, out TGridObject gridObject)) continue;
                if(checkBlocked && gridObject.IsBlocked) continue;
                yield return gridObject;
            }
        }
    }
    
    public IEnumerable<TGridObject> GetNeighbors(int x, int y, int radius, bool checkBlocked = false)
    {
        var output = GetNeighbors(x, y, new Bound(radius, radius, radius, radius), checkBlocked);

        output = output.Where(go => GetManhattanDistance(x, y, go) <= radius).ToList();

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

    public TGridObject GetRandomGridObject(bool checkBlocked = false)
    {
        if (checkBlocked)
        {
            var nonBlockGridObjects = GetAll().Where(go => !go.IsBlocked).ToList();
            return nonBlockGridObjects.GetRandomElement();
        }

        return gridArray.GetRandomElement();
    }
    
    public TGridObject GetRandomGridObjectWithCondition(Func<TGridObject, bool> conditionFunc)
    {
        var nonBlockGridObjects = GetAll().Where(conditionFunc).ToList();
        return nonBlockGridObjects.GetRandomElement();
    }
    
    public TGridObject GetRandomGridObjectWithCondition(Func<Grid<TGridObject>, int, int, bool> conditionFunction)
    {
        var list = GetGridObjectsWithCondition(conditionFunction);
        if (list.Count == 0) return null;
        
        return list[Random.Range(0, list.Count)];
    }

    public IEnumerable<TGridObject> GetSection(int sectionXIndex, int sectionYIndex, int sectionWidth,
        int sectionHeight)
    {
        for (int x = 0; x < sectionWidth; x++)
        {
            for (int y = 0; y < sectionHeight; y++)
            {
                var gridObject = GetValue(sectionWidth * sectionXIndex + x, sectionHeight * sectionYIndex + y);
                if (gridObject != null)
                    yield return gridObject;
            }
        }
    }
    
    public IEnumerable<TGridObject> GetGridObjectsInBound(Bound bound)
    {
        for (int x = bound.x; x <= bound.XPW; x++)
        {
            for (int y = bound.y; y <= bound.YPH; y++)
            {
                var gridObject = GetValue(x, y);
                if (gridObject != null)
                    yield return gridObject;
            }
        }
    }

    public int GetManhattanDistance(TGridObject g1, TGridObject g2)
    {
        return Mathf.Abs(g1.x - g2.x) + Mathf.Abs(g1.y - g2.y);
    }
    
    public int GetManhattanDistance(int x, int y, TGridObject g2)
    {
        return Mathf.Abs(x - g2.x) + Mathf.Abs(y - g2.y);
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