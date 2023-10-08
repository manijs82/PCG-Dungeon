using UnityEngine;

public abstract class Service : MonoBehaviour
{
    protected virtual void Awake()
    {
        ServiceLocator.AddService(this);
    }

    protected virtual void OnDestroy()
    {
        ServiceLocator.RemoveService(this);
    }
}