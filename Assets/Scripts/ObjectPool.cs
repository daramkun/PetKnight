using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private readonly List<GameObject> _createdObjects = new List<GameObject>();
    private readonly Queue<GameObject> _pooledObjects = new Queue<GameObject>();

    [SerializeField]
    private GameObject _prefab;

    void Awake()
    {
        if (_prefab == null)
            throw new InvalidOperationException();
    }

    public GameObject Pop()
    {
        if (_pooledObjects.Count == 0)
        {
            var createdObject = Instantiate(_prefab, transform);
            _createdObjects.Add(createdObject);
            return createdObject;
        }

        var poppedObject = _pooledObjects.Dequeue();
        poppedObject.SetActive(true);
        return poppedObject;
    }

    public void Push(GameObject gameObject)
    {
        if (!_createdObjects.Contains(gameObject))
            throw new ArgumentException(null, nameof(gameObject));

        _pooledObjects.Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
