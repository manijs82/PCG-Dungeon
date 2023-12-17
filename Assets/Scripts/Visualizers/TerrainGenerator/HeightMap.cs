using UnityEngine;

public class HeightMap
{
    public float[,] heights;

    private HeightMapData structure;
    private Vector2[] octaveOffsets;
    
    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

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
        for (int y = 0; y < heights.GetLength(1); y++)
        {
            for (int x = 0; x < heights.GetLength(0); x++)
            {
                float height = GetNoiseAt(x, y);

                if (height > maxNoiseHeight)
                    maxNoiseHeight = height;
                else if (height < minNoiseHeight) 
                    minNoiseHeight = height;

                heights[x, y] = height;
            }
        }

        for (int y = 0; y < heights.GetLength(1); y++)
        {
            for (int x = 0; x < heights.GetLength(0); x++)
            {
                heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);
                heights[x, y] = structure.elevationCurve.Evaluate(heights[x, y]) * structure.elevation;
            }
        }
    }

    protected float GetNoiseAt(float x, float y)
    {
        float amplitude = 1;
        float frequency = 1;
        float noise = 0;

        for (int i = 0; i < structure.octaves; i++)
        {
            float xSample = (x - structure.width / 2f) / structure.scale * frequency + octaveOffsets[i].x;
            float zSample = (y - structure.width / 2f) / structure.scale * frequency + octaveOffsets[i].y;

            float sample = Mathf.PerlinNoise(xSample, zSample) * 2 - 1;

            frequency *= structure.lacunarity;
            amplitude *= structure.persistence;
            noise += sample * amplitude;
        }

        return noise;
    }

    public float GetHeightAt(float x, float y)
    {
        float height = GetNoiseAt(x, y);

        height = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, height);
        height = structure.elevationCurve.Evaluate(height) * structure.elevation;

        return height;
    }

    public Vector3 GetNormalAt(int x, int y)
    {
        float accuracy = 1f;
        
        Vector3 center = new Vector3(0, GetHeightAt(x, y), 0);
        Vector3 up = new Vector3(0, GetHeightAt(x, y + accuracy), accuracy) - center;
        Vector3 right = new Vector3(accuracy, GetHeightAt(x + accuracy, y), 0) - center;

        return Vector3.Cross(up, right).normalized;
    }
}