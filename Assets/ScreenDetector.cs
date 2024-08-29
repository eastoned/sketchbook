using System;
using UnityEngine;

public class ScreenDetector : MonoBehaviour
{
    public static event Action<Vector2> OnScreenSizeChanged;

    private void OnRectTransformDimensionsChange()
    {
        OnScreenSizeChanged?.Invoke(new Vector2(Screen.width, Screen.height));
    }
}