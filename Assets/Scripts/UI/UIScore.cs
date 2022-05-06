using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private IntValue   scoreValue;

    private TextMeshProUGUI             textObject;

    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();        
    }

    void Update()
    {        
        int score = scoreValue.GetValue();

        textObject.text = $"Score: {score}";
    }
}
