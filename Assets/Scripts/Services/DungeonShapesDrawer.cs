using System;
using System.Collections.Generic;

public class DungeonShapesDrawer : Service
{
    private Dictionary<string, Action> shapes;
        
    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.dungeonShapesDrawer = this;

        shapes = new Dictionary<string, Action>();
    }

    public void AddShape(string key, Action drawAction)
    {
        if (shapes.ContainsKey(key))
            shapes[key] = drawAction;
        else
            shapes.Add(key, drawAction);
    }

    private void OnDrawGizmos()
    {
        if (shapes == null) return;
        
        foreach (var shape in shapes)
        {
            shape.Value();
        }
    }

    protected override void OnDestroy()
    {
        ServiceLocator.dungeonShapesDrawer = null;
        base.OnDestroy();
    }
}