using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PCG_SearchBased_Dungeon
{
    public class Dungeon : Sample
    {
        public Vector2Int RoomCountRange = new(6, 13);
        public Vector2Int RoomWidthRange = new(5, 8);

        private CellType[,] cells;
        private int width = 50;
        public List<Room> rooms;
        public List<Triangle> triangles;

        public Dungeon()
        {
            rooms = new List<Room>();
            SetRooms();
        }

        public Dungeon(Dungeon d)
        {
            width = d.width;
            rooms = new List<Room>(d.rooms);
        }

        private void SetRooms()
        {
            int roomCount = Random.Range(RoomCountRange.x, RoomCountRange.y);
            for (int i = 0; i < roomCount; i++)
            {
                Vector2 point = new Vector2(Random.Range(0, width), Random.Range(0, width));
                Room room = new Room(point, Random.Range(RoomWidthRange.x, RoomWidthRange.y),
                    Random.Range(RoomWidthRange.x, RoomWidthRange.y));

                rooms.Add(room);
            }
        }

        public override void Mutate()
        {
            for (var i = 0; i < rooms.Count; i++)
            {
                Vector2 startPointOffset = new Vector2(Random.Range(-2, 3), Random.Range(-2, 3));
                Room newRoom = new Room(rooms[i]);
                newRoom.startPoint += startPointOffset;
                rooms[i] = newRoom;
            }
        }

        public override string ToString()
        {
            string o = String.Empty;

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    o += GetChar(cells[x, y]);
                }

                o += "\n";
            }

            return o;
        }

        public static string GetChar(CellType cell)
        {
            switch (cell)
            {
                case CellType.Empty:
                    return ". ";
                case CellType.Wall:
                    return "H ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(cell), cell, null);
            }
        }
    }
}