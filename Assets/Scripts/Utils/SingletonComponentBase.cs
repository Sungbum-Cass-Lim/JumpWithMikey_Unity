using Unity.VisualScripting;
using UnityEngine;

public abstract class SingletonComponentBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _lockObj = new();
    private static bool _isQuitApplication = false;

    public static T Instance
    {
        get
        {
            if (_isQuitApplication)
                return null;
            lock (_lockObj)
            {
                if (null == _instance)
                {
                    var componentName = typeof(T).ToString();
                    var findGameObject = FindObjectOfType(typeof(T));
                    if (null != findGameObject)
                    {
                        _instance = findGameObject.GetComponent<T>();
                        return _instance;
                    }
                    GameObject singleton = new()
                    {
                        name = "(SingletonComponent)" + componentName
                    };
                    _instance = singleton.AddComponent<T>();
                    //DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
    }

    private void Awake()
    {
        _isQuitApplication = false;
        InitializeSingleton();
    }

    protected abstract void InitializeSingleton();

    public abstract void ResetSingleton();

    private void OnApplicationQuit()
    {
        _isQuitApplication = true;
    }
}
