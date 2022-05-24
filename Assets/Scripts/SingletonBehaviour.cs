using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Object
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = Object.FindObjectOfType<T>();
            return _instance;
        }
    }
}