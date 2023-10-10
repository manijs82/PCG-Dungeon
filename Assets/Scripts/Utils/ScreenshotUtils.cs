using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class ScreenshotUtils
    {
        public static void TakeScreenShot(Dungeon dungeon, Tilemap tilemap, int pixelPerUnit, string path)
        {
            int dungeonWidth = dungeon.dungeonParameters.width;
            int dungeonHeight = dungeon.dungeonParameters.width;

            var texture = GetTilemapTexture(tilemap, pixelPerUnit, dungeonWidth, dungeonHeight);

            byte[] bytes = texture.EncodeToPNG();
            if (Directory.Exists(path))
            {
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                File.WriteAllBytes(path + $"Screenshot_{dateTime}.png", bytes);
            }
        }

        public static Texture2D GetTilemapTexture(Tilemap tilemap, int pixelPerUnit, int width, int height)
        {
            Texture2D texture = new Texture2D(width * pixelPerUnit, height * pixelPerUnit);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Sprite sprite = tilemap.GetSprite(new Vector3Int(x, y));
                    Texture2D spriteTexture = sprite.texture;
                    Rect spriteRect = sprite.textureRect;
                    int atlasX = (int)spriteRect.xMin;
                    int atlasY = (int)spriteRect.yMin;

                    for (int i = 0; i < pixelPerUnit; i++)
                    {
                        for (int j = 0; j < pixelPerUnit; j++)
                        {
                            Vector2Int scaledPixelCoordinate = GetNearestPixelCoordinate(new Vector2Int(i, j),
                                sprite.pixelsPerUnit / pixelPerUnit);
                            Color pixelColor =
                                spriteTexture.GetPixel(atlasX + scaledPixelCoordinate.x, atlasY + scaledPixelCoordinate.y) *
                                tilemap.GetColor(new Vector3Int(x, y));
                            texture.SetPixel(x * pixelPerUnit + i, y * pixelPerUnit + j, pixelColor);
                        }
                    }
                }
            }

            return texture;
        }

        private static Vector2Int GetNearestPixelCoordinate(Vector2Int coordinate, float scale)
        {
            int projectedX = Mathf.FloorToInt(coordinate.x * scale);
            int projectedY = Mathf.FloorToInt(coordinate.y * scale);
            return new Vector2Int(projectedX, projectedY);
        }
    }
}