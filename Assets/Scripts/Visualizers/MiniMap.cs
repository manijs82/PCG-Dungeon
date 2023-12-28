using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class MiniMap : DungeonVisualizer
{
    [SerializeField] private Transform playerMinimap;
    [SerializeField] private Color backColor;
    [SerializeField] private Color forColor;

    private float width = 0;
    private float height = 0;
    
    private void Start()
    {
        width = GetComponent<RectTransform>().rect.width;
        height = GetComponent<RectTransform>().rect.height;
    }

    private void Update()
    {
        var player = GameManager.Instance.player;
        if (player == null) return;
        
        var playerPos = player.GetCurrentPositionOnDungeon();
        playerMinimap.localPosition = new Vector3(playerPos.x * width, playerPos.y * height) - new Vector3(150, 150);
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