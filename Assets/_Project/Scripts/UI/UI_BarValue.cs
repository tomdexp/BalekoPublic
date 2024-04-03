using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BarValue : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void SetBarValue(float currentValue, float maxValue)
    {
        RectTransform rect = (RectTransform) _image.transform;
        rect.anchorMax = new Vector2(currentValue / maxValue, 1);
        Debug.Log(currentValue / maxValue);
    }
}
