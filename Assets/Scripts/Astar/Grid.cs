using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public float CellSize => cellSize;

    public Grid(int width, int height, float cellSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject,
        Vector3 origin = default, bool shouldDebug = false)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new TGridObject[width, height];

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
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + cellSize, y), Color.white,
                    Mathf.Infinity);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + cellSize), Color.white,
                    Mathf.Infinity);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(height, width), Color.white, Mathf.Infinity);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(height, width), Color.white, Mathf.Infinity);
    }

    public Vector3 GetWorldPosition(float x, float y) => new Vector3(x, y) * cellSize + origin;

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
    
    public List<TGridObject> GetNeighbors(TGridObject center)
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
            if (p != null)
            {
                if (!p.IsBlocked)
                    output.Add(p);
            }

        return output;
    }
    
    public List<TGridObject> Get9Neighbors(TGridObject center)
    {
        var output = new List<TGridObject>();
        var temp = new List<TGridObject>();

        
        temp.Add(GetValue(center.x, center.y + 1));
        temp.Add(GetValue(center.x, center.y - 1));
        temp.Add(GetValue(center.x + 1, center.y));
        temp.Add(GetValue(center.x - 1, center.y));
        temp.Add(GetValue(center.x + 1, center.y + 1));
        temp.Add(GetValue(center.x - 1, center.y - 1));
        temp.Add(GetValue(center.x + 1, center.y - 1));
        temp.Add(GetValue(center.x - 1, center.y + 1));

        foreach (var p in temp)
            if (p != null)
            {
                if (!p.IsBlocked)
                    output.Add(p);
            }

        return output;
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
}