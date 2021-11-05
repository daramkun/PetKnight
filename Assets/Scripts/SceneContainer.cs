using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SceneContainer : MonoBehaviour
{
    private Size lastWindowSize;

    void Awake()
    {
        UpdateContainerSize();
        lastWindowSize = new Size(Screen.width, Screen.height);
    }

    void Update()
    {
        var currentWindowSize = new Size(Screen.width, Screen.height);
        if (lastWindowSize != currentWindowSize)
        {
            UpdateContainerSize();
            lastWindowSize = currentWindowSize;
        }
    }

    private void UpdateContainerSize()
    {
        var mainCamera = Camera.main;
        var rectTransform = transform as RectTransform;

        rectTransform.sizeDelta = new Vector2(3.38f, Screen.height * (3.38f / Screen.width));
    }
}
