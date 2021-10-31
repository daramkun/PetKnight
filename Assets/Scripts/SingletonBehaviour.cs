using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Object
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = Object.FindObjectOfType<T>();
            return instance;
        }
    }
}