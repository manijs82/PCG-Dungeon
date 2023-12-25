using UnityEngine;

public class NoiseGridObject : GridObject
{
    public float height;
    
    public NoiseGridObject(int x, int y, float height) : base(x, y)
    {
        this.height = height;
    }
}

public class NoiseMap
{
    public readonly Grid<NoiseGridObject> noiseGrid;

    private NoiseMapData structure;
    private Vector2[] octaveOffsets;
    
    float maxNoiseValue = float.MinValue;
    float minNoiseValue = float.MaxValue;
    
    public float this[int x, int y]
    {
        get
        {
            if (noiseGrid == null)
                return 0;
            var value = noiseGrid.GetValue(x, y);
            if (value == null)
                return 0;
            return value.height;
        }
    }

    public NoiseMap(NoiseMapData structure)
    {
        this.structure = structure;

        noiseGrid = new Grid<NoiseGridObject>(structure.width, structure.width, 1, (_, x, y) => new NoiseGridObject(x, y, 0));
        SetOffset();
        SetNoiseMap();
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

    private void SetNoiseMap()
    {
        for (int y = 0; y < noiseGrid.Height; y++)
        {
            for (int x = 0; x < noiseGrid.Width; x++)
            {
                float height = GetNoiseAt(x, y);

                if (height > maxNoiseValue)
                    maxNoiseValue = height;
                else if (height < minNoiseValue) 
                    minNoiseValue = height;

                noiseGrid.GetValue(x, y).height = height;
            }
        }

        for (int y = 0; y < noiseGrid.Height; y++)
        {
            for (int x = 0; x < noiseGrid.Width; x++)
            {
                noiseGrid.GetValue(x, y).height = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, noiseGrid.GetValue(x, y).height);
                noiseGrid.GetValue(x, y).height = structure.elevationCurve.Evaluate(noiseGrid.GetValue(x, y).height) * structure.elevation;
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

    public float GetValueAt(float x, float y)
    {
        float height = GetNoiseAt(x, y);

        height = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, height);
        height = structure.elevationCurve.Evaluate(height) * structure.elevation;

        return height;
    }

    public Vector3 GetNormalAt(float x, float y, float accuracy = 0.1f)
    {
        Vector3 zero = new Vector3(-accuracy, GetValueAt(x - accuracy, y - accuracy), -accuracy);
        Vector3 up = new Vector3(-accuracy, GetValueAt(x - accuracy, y + accuracy), accuracy);
        Vector3 one = new Vector3(accuracy, GetValueAt(x + accuracy, y + accuracy), accuracy);

        return Vector3.Cross((one - up).normalized, (zero - up).normalized).normalized;
    }
}