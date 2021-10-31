using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldInspector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private float speed = 2;
    private long currentAmount = 0;

    private Coroutine animationCoroutine;

    void Awake()
    {

    }

    public void SetAmount(long gold)
    {
        if (currentAmount == gold)
            return;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AmountChangeAnimation(gold));
    }

    private IEnumerator AmountChangeAnimation(long gold)
    {
        var startTime = Time.realtimeSinceStartup;
        var startingAmount = currentAmount;
        while ((Time.realtimeSinceStartup - startTime) * speed < 1)
        {
            var temp = (double)(Time.realtimeSinceStartup - startTime) * speed;
            var animationAmount = (startingAmount - gold) * temp;

            currentAmount = (long)(startingAmount - animationAmount);
            text.text = $"{currentAmount:n0}";

            yield return null;
        }

        currentAmount = gold;
        text.text = $"{gold:n0}";

        animationCoroutine = null;
    }
}
