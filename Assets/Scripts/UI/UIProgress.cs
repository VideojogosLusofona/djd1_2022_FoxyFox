using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : MonoBehaviour
{
    [SerializeField] private FloatValue value;
    [SerializeField] private FloatValue maxValue;
    [SerializeField] private Image      bar;
    [SerializeField] private Gradient   color;

    void Update()
    {
        bar.fillAmount = value.GetValue() / maxValue.GetValue();    
        if (color != null)
        {
            bar.color = color.Evaluate(bar.fillAmount);
        }
    }
}
