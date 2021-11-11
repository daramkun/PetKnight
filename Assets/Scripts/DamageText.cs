using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;

    private ObjectPool objectPool;
    private Coroutine animationCoroutine;

    void Awake()
    {
        objectPool = GetComponentInParent<ObjectPool>();
    }

    public void StartAnimation(GameObject target, string i, Color color)
    {
        text.text = i;
        text.color = color;

        transform.position = target.transform.position + new Vector3(0, 1, 0);

        animationCoroutine = StartCoroutine(DamageTextAnimation());
    }

    void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
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
        objectPool.Push(gameObject);
    }
}
