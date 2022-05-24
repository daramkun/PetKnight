using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraWidthFixer : MonoBehaviour
{
    private Size _lastWindowSize;

    void Awake()
    {
        UpdateCamera();
        _lastWindowSize = new Size(Screen.width, Screen.height);
    }

    void Update()
    {
        var currentWindowSize = new Size(Screen.width, Screen.height);
        if (_lastWindowSize != currentWindowSize)
        {
            UpdateCamera();
            _lastWindowSize = currentWindowSize;
        }
    }

    private void UpdateCamera()
    {
        var mainCamera = Camera.main;

        var cameraHeight = 3f;
        var desiredAspect = 9 / 16f;
        var aspect = mainCamera.aspect;
        var ratio = desiredAspect / aspect;

        mainCamera.orthographicSize = cameraHeight * ratio;
    }
}
