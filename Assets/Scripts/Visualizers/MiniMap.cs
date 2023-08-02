using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [RequireComponent(typeof(RawImage))]
    public class MiniMap : DungeonVisualizer
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform playerMinimap;
        
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
                    Color color = tile.Type == CellType.Empty ? Color.cyan : Color.blue;
                    minimap.SetPixel(x, y, color);  
                }
            }
            minimap.Apply();

            GetComponent<RawImage>().texture = minimap;
        }
    }
}