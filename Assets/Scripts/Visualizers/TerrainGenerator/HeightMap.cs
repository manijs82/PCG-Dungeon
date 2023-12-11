using UnityEngine;

public class HeightMap
{
    public float[,] heights;

    private HeightMapData structure;
    private Vector2[] octaveOffsets;

    public HeightMap(HeightMapData structure)
    {
        this.structure = structure;

        heights = new float[structure.width + 1, structure.width + 1];
        SetOffset();
        SetHeightMap();
    }

    private void SetOffset()
    {
        octaveOffsets = new Vector2[structure.octaves];
        for (int i = 0; i < structure.octaves; i++)
        {
            float offsetX = Generator.dungeonRnd.Next(-100000, 100000);
            float offsetY = Generator.dungeonRnd.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
    }

    private void SetHeightMap()
    {
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int z = 0; z < structure.width; z++)
        {
            for (int x = 0; x < structure.width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float height = 0;

                for (int i = 0; i < structure.octaves; i++)
                {
                    float xSample = (x - structure.width / 2f) / structure.scale * frequency + octaveOffsets[i].x;
                    float zSample = (z - structure.width / 2f) / structure.scale * frequency + octaveOffsets[i].y;

                    float sample = Mathf.PerlinNoise(xSample, zSample) * 2 - 1;

                    frequency *= structure.lacunarity;
                    amplitude *= structure.persistence;
                    height += sample * amplitude;
                }

                if (height > maxNoiseHeight)
                    maxNoiseHeight = height;
                else if (height < minNoiseHeight) 
                    minNoiseHeight = height;

                heights[x, z] = height;
            }
        }

        for (int y = 0; y < structure.width; y++)
        {
            for (int x = 0; x < structure.width; x++)
            {
                heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);
                heights[x, y] = structure.elevationCurve.Evaluate(heights[x, y]) * structure.elevation;
            }
        }
    }
}