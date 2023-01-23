using UnityEngine;

public static class MouseUtils
{
    public static GameObject GetMouseClickedObject(Camera cam)
    {
        var ray = GetMouseRay(cam);
        return Physics.Raycast(ray, out var hitInfo) ? hitInfo.collider.gameObject : null;
    }
    
    public static Vector3 GetMouseClickedPoint(Camera cam)
    {
        var ray = GetMouseRay(cam);
        return Physics.Raycast(ray, out var hitInfo) ? hitInfo.point : default;
    }

    public static Ray GetMouseRay(Camera cam) => cam.ScreenPointToRay(Input.mousePosition);
}