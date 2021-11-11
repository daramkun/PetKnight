using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private readonly List<GameObject> createdObjects = new List<GameObject>();
    private readonly Queue<GameObject> pooledObjects = new Queue<GameObject>();

    [SerializeField]
    private GameObject prefab;

    void Awake()
    {
        if (prefab == null)
            throw new InvalidOperationException();
    }

    public GameObject Pop()
    {
        if (pooledObjects.Count == 0)
        {
            var createdObject = Instantiate(prefab, transform);
            createdObjects.Add(createdObject);
            return createdObject;
        }

        var poppedObject = pooledObjects.Dequeue();
        poppedObject.SetActive(true);
        return poppedObject;
    }

    public void Push(GameObject gameObject)
    {
        if (!createdObjects.Contains(gameObject))
            throw new ArgumentException(null, nameof(gameObject));

        pooledObjects.Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
