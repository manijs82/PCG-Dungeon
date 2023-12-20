using System;
using System.Collections.Generic;

public class DungeonShapesDrawer : Service
{
    private List<Action> shapes;
        
    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.dungeonShapesDrawer = this;

        shapes = new List<Action>();
    }

    public void AddShape(Action drawAction)
    {
        shapes.Add(drawAction);
    }

    private void OnDrawGizmos()
    {
        if (shapes == null) return;
        
        foreach (var shape in shapes)
        {
            shape();
        }
    }

    protected override void OnDestroy()
    {
        ServiceLocator.dungeonShapesDrawer = null;
        base.OnDestroy();
    }
}