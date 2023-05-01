using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSource", menuName = "DecorationObjectSource", order = 0)]
public class DecorationObjectSource : ScriptableObject
{
    public DecorationObject[] objects;
}