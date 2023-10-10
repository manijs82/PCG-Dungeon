using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class MiniMap : DungeonVisualizer
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerMinimap;
    [SerializeField] private Color backColor;
    [SerializeField] private Color forColor;
        
    private void Update()
    {
        playerMinimap.localPosition = player.position * 2 - new Vector3(150, 150);
    }

    protected override void Visualize(Dungeon dungeon)
    {
        Texture2D minimap = new Texture2D(dungeon.grid.Width, dungeon.grid.Height);
        minimap.filterMode = FilterMode.Point;
            
        for (int y = 0; y < dungeon.grid.Height; y++)
        {
            for (int x = 0; x < dungeon.grid.Width; x++)
            {
                var tile = (TileGridObject)dungeon.grid.GetValue(x, y);
                Color color = tile.Type == CellType.Empty ? backColor : forColor;
                minimap.SetPixel(x, y, color);  
            }
        }
        minimap.Apply();

        GetComponent<RawImage>().texture = minimap;
    }
}