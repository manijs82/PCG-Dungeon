using UnityEngine;

[System.Serializable]
public class PerlinTileSet : TileSet
{
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float perlinSnapInterval = 0.2f;
    [SerializeField] private float perlinScale = 0.1f;
}