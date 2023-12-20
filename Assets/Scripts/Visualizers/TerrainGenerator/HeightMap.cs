using UnityEngine;

public class HeightGridObject : GridObject
{
    public float height;
    
    public HeightGridObject(int x, int y, float height) : base(x, y)
    {
        this.height = height;
    }
}

public class HeightMap
{
    public readonly Grid<HeightGridObject> heights;

    private HeightMapData structure;
    private Vector2[] octaveOffsets;
    
    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;
    
    public float this[int x, int y]
    {
        get
        {
            if (heights == null)
                return 0;
            if (heights.GetValue(x, y) == null)
                return 0;
            return heights.GetValue(x, y).height;
        }
    }

    public HeightMap(HeightMapData structure)
    {
        this.structure = structure;

        heights = new Grid<HeightGridObject>(structure.width, structure.width, 1, (_, x, y) => new HeightGridObject(x, y, 0));
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
        for (int y = 0; y < heights.Height; y++)
        {
            for (int x = 0; x < heights.Width; x++)
            {
                float height = GetNoiseAt(x, y);

                if (height > maxNoiseHeight)
                    maxNoiseHeight = height;
                else if (height < minNoiseHeight) 
                    minNoiseHeight = height;

                heights.GetValue(x, y).height = height;
            }
        }

        for (int y = 0; y < heights.Height; y++)
        {
            for (int x = 0; x < heights.Width; x++)
            {
                heights.GetValue(x, y).height = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights.GetValue(x, y).height);
                heights.GetValue(x, y).height = structure.elevationCurve.Evaluate(heights.GetValue(x, y).height) * structure.elevation;
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

    public Vector3 GetNormalAt(float x, float y, float accuracy = 0.1f)
    {
        Vector3 zero = new Vector3(-accuracy, GetHeightAt(x - accuracy, y - accuracy), -accuracy);
        Vector3 up = new Vector3(-accuracy, GetHeightAt(x - accuracy, y + accuracy), accuracy);
        Vector3 right = new Vector3(accuracy, GetHeightAt(x + accuracy, y - accuracy), -accuracy);
        Vector3 one = new Vector3(accuracy, GetHeightAt(x + accuracy, y + accuracy), accuracy);

        return Vector3.Cross((one - up).normalized, (zero - up).normalized).normalized;
    }
}