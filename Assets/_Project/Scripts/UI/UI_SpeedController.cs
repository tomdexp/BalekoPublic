using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SpeedController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int maxNum;
    public void DoubleSpeed()
    {
        Time.timeScale *= 2;
        if (Time.timeScale == maxNum) Time.timeScale = 1;
        text.text = "Speed : x" + Time.timeScale;
    }
}
