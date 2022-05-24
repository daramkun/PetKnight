using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldInspector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private float _speed = 2;
    private long _currentAmount = 0;

    private Coroutine _animationCoroutine;

    void Awake()
    {

    }

    public void SetAmount(long gold)
    {
        if (_currentAmount == gold)
            return;

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(AmountChangeAnimation(gold));
    }

    private IEnumerator AmountChangeAnimation(long gold)
    {
        var startTime = Time.realtimeSinceStartup;
        var startingAmount = _currentAmount;
        while ((Time.realtimeSinceStartup - startTime) * _speed < 1)
        {
            var temp = (double)(Time.realtimeSinceStartup - startTime) * _speed;
            var animationAmount = (startingAmount - gold) * temp;

            _currentAmount = (long)(startingAmount - animationAmount);
            _text.text = $"{_currentAmount:n0}";

            yield return null;
        }

        _currentAmount = gold;
        _text.text = $"{gold:n0}";

        _animationCoroutine = null;
    }
}
