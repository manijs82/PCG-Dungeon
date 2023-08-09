using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance => _instance;

    public static bool IsInitialized => _instance != null;

    protected virtual void Awake()
    {
        if (_instance != null)
            Debug.LogError("[Singleton] trying to instantiate a second type of a singleton class");
        else
            _instance = (T) this;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}