using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class ScreenshotUtils
    {
        public static void TakeScreenShot(Dungeon dungeon, Tilemap tilemap, string path)
        {
            int dungeonWidth = dungeon.dungeonParameters.width;
            int dungeonHeight = dungeon.dungeonParameters.width;
            int pixelPerUnit = 12;
            
            Texture2D texture = new Texture2D(dungeonWidth * pixelPerUnit, dungeonHeight * pixelPerUnit);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (int x = 0; x < dungeonWidth; x++)
            {
                for (int y = 0; y < dungeonHeight; y++)
                {
                    Sprite sprite = tilemap.GetSprite(new Vector3Int(x, y));
                    Texture2D spriteTexture = sprite.texture;
                    Rect spriteRect = sprite.textureRect;
                    int atlasX = (int) spriteRect.xMin;
                    int atlasY = (int) spriteRect.yMin;
                    
                    for (int i = 0; i < (int) sprite.pixelsPerUnit; i++)
                    {
                        for (int j = 0; j < (int) sprite.pixelsPerUnit; j++)
                        {
                            Color pixelColor = spriteTexture.GetPixel(atlasX + i, atlasY + j) * tilemap.GetColor(new Vector3Int(x, y));
                            texture.SetPixel(x * pixelPerUnit + i, y * pixelPerUnit + j, pixelColor);
                        }
                    }
                }
            }

            byte[] bytes = texture.EncodeToPNG();
            if (Directory.Exists(path))
            {
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                File.WriteAllBytes(path + $"Screenshot_{dateTime}.png", bytes);
            }
        }
    }
}