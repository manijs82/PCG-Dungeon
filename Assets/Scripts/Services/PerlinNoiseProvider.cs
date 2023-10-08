using UnityEngine;

public class PerlinNoiseProvider : Service
{
    [SerializeField] private float perlinSnapInterval = 0.25f;
    [SerializeField] private float perlinScale = 0.1f;

    private float perlinOffset;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.PerlinNoiseProvider = this;
    }

    public void SetPerlinOffset(float offset)
    {
        perlinOffset = offset;
    }

    public virtual float GetNoise(int x, int y)
    {
        float noise = Mathf.PerlinNoise((x + perlinOffset) * perlinScale,
            (y + perlinOffset) * perlinScale);
        return Mathf.Round(noise / perlinSnapInterval) * perlinSnapInterval;
    }

    protected override void OnDestroy()
    {
        ServiceLocator.PerlinNoiseProvider = null;
        base.OnDestroy();
    }
}