using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image bar;

    [SerializeField]
    private float speed = 2;
    private float currentAmount = 1;

    private Coroutine animationCoroutine;

    void Awake()
    {

    }

    public void SetAmount(float amount)
    {
        if (Math.Abs(currentAmount - amount) < float.Epsilon)
            return;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AmountChangeAnimation(amount));
    }

    private IEnumerator AmountChangeAnimation(float amount)
    {
        var startTime = Time.realtimeSinceStartup;
        var startingAmount = currentAmount;
        while ((Time.realtimeSinceStartup - startTime) * speed < 1)
        {
            var temp = (Time.realtimeSinceStartup - startTime) * speed;
            var animationAmount = (startingAmount - amount) * temp;

            currentAmount = startingAmount - animationAmount;
            bar.rectTransform.sizeDelta =
                new Vector2(background.rectTransform.rect.width * currentAmount,
                            bar.rectTransform.sizeDelta.y);

            yield return null;
        }

        currentAmount = amount;
        bar.rectTransform.sizeDelta =
            new Vector2(background.rectTransform.rect.width * amount, bar.rectTransform.sizeDelta.y);

        animationCoroutine = null;
    }
}
