using Freya;
using MeshGen;
using UnityEngine;

public class RoomMeshGenerator : DungeonMeshGenerator
{
    private Material groundMaterial;
    
    public RoomMeshGenerator(Dungeon dungeon, NoiseMap heightMap, Material groundMaterial) : base(dungeon, heightMap)
    {
        this.groundMaterial = groundMaterial;
    }

    public override void Generate()
    {
        foreach (var roomNode in dungeon.roomGraph.Nodes)
        {
            if (roomNode.Value.environmentType != EnvironmentType.Forest)
                continue;
            
            MeshData meshData = new MeshData();
            var roomBound = roomNode.Value.bound;
            var height = heightMap.GerHighestValueWithinBound(roomBound);
            
            var quadRight = (roomBound.BottomRight - roomBound.Start) / 2f;
            var quadUp = (roomBound.TopLeft - roomBound.Start) / 2f;
            meshData.AddQuad(Vector3.zero, quadRight.XZtoXYZ(), quadUp.XZtoXYZ());
            meshData.AddCube(Vector3.up * height / 2f, roomBound.Extents.XZtoXYZ(height / 2f));

            Mesh mesh = meshData.CreateMesh(false, true);
            
            var go = new GameObject("room ground");
            go.transform.position = roomBound.Center.XZtoXYZ();
            var meshFilter = go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = groundMaterial;
            meshFilter.mesh = mesh;
            var meshCollider = go.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
    }
}