using Mani;
using Mani.Geometry;
using UnityEditor;
using UnityEngine;

public class VideoRef : MonoBehaviour
{
#if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        
        Triangle superTriangle = new Triangle(
            new Point(0, 5),
            new Point(-7, -5),
            new Point(7, -5));

        Triangle triangle1 = new Triangle(
            new Point(-2, -3.5f),
            new Point(0, 5),
            new Point(-7, -5));
        triangle1.DrawGizmos();
        triangle1.CircumCircle.DrawGizmos(true);
        
        print(triangle1.CircumCircle.Center);
        print(triangle1.CircumCircle.Radius);
        
        Handles.matrix = Matrix4x4.identity;
    }
    
#endif
}
