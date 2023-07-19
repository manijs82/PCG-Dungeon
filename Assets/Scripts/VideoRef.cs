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
        var superPointUp = new Point(0, 5);
        var superPointLeft = new Point(-7, -5);
        var superPointRight = new Point(7, -5);
        var vDownLeft = new Point(-2, -3.5f);
        var vDownRight = new Point(2, -1.5f);
        var vUpLeft = new Point(-2, -0.5f);
        var vUpRight = new Point(1, 1.5f);
        
        Triangle superTriangle = new Triangle(
            superPointUp,
            superPointLeft,
            superPointRight);

        Triangle triangle1 = new Triangle(
            vDownLeft,
            superPointRight,
            vUpRight);
        triangle1.DrawGizmos();
        triangle1.CircumCircle.DrawGizmos(true);
        
        //print(triangle1.CircumCircle.Center);
        //print(triangle1.CircumCircle.Radius);
        
        Handles.matrix = Matrix4x4.identity;
    }
    
#endif
}
