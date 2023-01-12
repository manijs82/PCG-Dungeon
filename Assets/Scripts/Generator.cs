using UnityEditor;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private GameObject block;

        private Dungeon candidateDungeon;
        
        private void Start()
        {
            Evolution<Dungeon> e = new Evolution<Dungeon>();

            Dungeon d = (Dungeon) e.samples[0];
            foreach (var room in d.rooms)
            {
                room.SetCells();
                GenerateRoom(room);
            }

            candidateDungeon = d;
        }

        private void GenerateRoom(Room room)
        {
            for (int y = 0; y < room.cells.GetLength(1); y++)
            {
                for (int x = 0; x < room.cells.GetLength(0); x++)
                {
                    var go = Instantiate(block, room.startPoint + new Vector2(x, y), 
                        Quaternion.identity, transform);
                    SpriteRenderer sprite = go.GetComponentInChildren<SpriteRenderer>();
                    sprite.color = GetColor(room.cells[x, y]);
                }
            }
        }

        private Color GetColor(CellType roomCell)
        {
            switch (roomCell)
            {
                case CellType.Empty:
                    return Color.white;
                case CellType.Wall:
                    return Color.black;
            }
            return Color.white;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(Vector3.zero, new Vector3(50, 0));
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, 50));
            Gizmos.DrawLine(new Vector3(50, 0), new Vector3(50, 50));
            Gizmos.DrawLine(new Vector3(0, 50), new Vector3(50, 50));
            Gizmos.DrawSphere(new Vector3(25, 25), .5f);

            if(candidateDungeon != null)
                DrawDungeon(candidateDungeon);
        }

        #if UNITY_EDITOR
        private void DrawDungeon(Dungeon dungeon)
        {
            foreach (var room in dungeon.rooms)
            {
                Vector2 center = new Vector2(25f, 25f);

                Handles.color = Color.LerpUnclamped(Color.red, Color.black, Vector2.Distance(center, room.Center) / 25f);
                Handles.DrawAAPolyLine(Vector2.Distance(center, room.Center) / 25f * 10, center, room.Center);
            }
        }
        #endif
    }
}