using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    //public void SetMaxValue(float value)
    //{
    //	slider.maxValue = value;
    //	slider.value = value;
    //}

    //public void SetHealth(float valueCurrent)
    //{
    //	slider.value = valueCurrent;
    //}

    public void SetValue(float value, float valueMax)
    {
        slider.maxValue = valueMax;
        slider.value = value;
    }
}
