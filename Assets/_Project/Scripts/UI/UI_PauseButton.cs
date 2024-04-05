using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PauseButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Pause()
    {
        if (Time.timeScale == Mathf.Epsilon)
        {
            Time.timeScale = 1;
            text.text = "Pause";
        }
        else
        {
            Time.timeScale = Mathf.Epsilon;
            text.text = "Play";
        }
    }
}
