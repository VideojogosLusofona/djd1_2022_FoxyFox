using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntValue", menuName = "Custom/Int Value")]
public class IntValue : ScriptableObject
{
    [SerializeField] private int  value;

    public void ChangeValue(int deltaValue)
    {
        value += deltaValue;
    }

    public int GetValue()
    {
        return value;
    }
}
