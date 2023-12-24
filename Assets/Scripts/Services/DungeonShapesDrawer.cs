using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ShapeCategory
{
    public string categoryName;
    public bool shouldDraw;
    
    public List<Action> drawActions;
}

public class DungeonShapesDrawer : Service
{
    [SerializeField] private List<ShapeCategory> categories = new();
    
        
    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.dungeonShapesDrawer = this;
    }

    public void AddShape(Action drawAction, string category)
    {
        var shapeCategory = categories.FirstOrDefault(c => c.categoryName == category);
        if (shapeCategory == null)
        {
            categories.Add(new ShapeCategory
            {
                categoryName = category,
                shouldDraw = true,
                drawActions = new List<Action> {drawAction}
            });
        }
        else
        {
            shapeCategory.drawActions ??= new List<Action>();
            shapeCategory.drawActions.Add(drawAction);
        }
    }

    private void OnDrawGizmos()
    {
        if (categories == null) return;
        
        foreach (var category in categories)
        {
            if(!category.shouldDraw)
                continue;

            foreach (var drawAction in category.drawActions)
                drawAction();
        }
    }

    protected override void OnDestroy()
    {
        ServiceLocator.dungeonShapesDrawer = null;
        base.OnDestroy();
    }
}