using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _bar;

    [SerializeField]
    private float _speed = 2;
    private float _currentAmount = 1;

    private Coroutine _animationCoroutine;

    void Awake()
    {

    }

    public void SetAmount(float amount)
    {
        if (Math.Abs(_currentAmount - amount) < float.Epsilon)
            return;

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(AmountChangeAnimation(amount));
    }

    private IEnumerator AmountChangeAnimation(float amount)
    {
        var startTime = Time.realtimeSinceStartup;
        var startingAmount = _currentAmount;
        while ((Time.realtimeSinceStartup - startTime) * _speed < 1)
        {
            var temp = (Time.realtimeSinceStartup - startTime) * _speed;
            var animationAmount = (startingAmount - amount) * temp;

            _currentAmount = startingAmount - animationAmount;
            _bar.rectTransform.sizeDelta =
                new Vector2(_background.rectTransform.rect.width * _currentAmount,
                            _bar.rectTransform.sizeDelta.y);

            yield return null;
        }

        _currentAmount = amount;
        _bar.rectTransform.sizeDelta =
            new Vector2(_background.rectTransform.rect.width * amount, _bar.rectTransform.sizeDelta.y);

        _animationCoroutine = null;
    }
}
