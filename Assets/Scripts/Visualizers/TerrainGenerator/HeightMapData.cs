
using UnityEngine;

[System.Serializable]
public struct HeightMapData
{
    public int width;
    public float elevation;
    public AnimationCurve elevationCurve;
    public float scale;
    [Range(1, 10)] public int octaves;
    public float persistence;
    [Range(1, 30)] public float lacunarity;
}