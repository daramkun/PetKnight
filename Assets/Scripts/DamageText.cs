using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;

    private ObjectPool _objectPool;
    private Coroutine _animationCoroutine;

    void Awake()
    {
        _objectPool = GetComponentInParent<ObjectPool>();
    }

    public void StartAnimation(GameObject target, string i, Color color)
    {
        _text.text = i;
        _text.color = color;

        transform.position = target.transform.position + new Vector3(0, 1, 0);

        _animationCoroutine = StartCoroutine(DamageTextAnimation());
    }

    void OnDisable()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }
    }

    IEnumerator DamageTextAnimation()
    {
        var startTime = Time.realtimeSinceStartup;
        var startPosition = transform.position;
        while ((Time.realtimeSinceStartup - startTime) < 1)
        {
            var temp = (Time.realtimeSinceStartup - startTime) / 2;
            transform.position = startPosition + new Vector3(0, temp, 0);
            yield return null;
        }
        _objectPool.Push(gameObject);
    }
}
