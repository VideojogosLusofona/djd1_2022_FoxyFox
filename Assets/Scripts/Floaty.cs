using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{
    [SerializeField] private float amplitude = 10.0f;
    [SerializeField] private float frequency = 1.0f;
    [SerializeField] private float angularOffset = 0.0f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = initialPosition + Vector3.up * amplitude * Mathf.Sin(frequency * 2 * Mathf.PI * Time.time + angularOffset);
    }
}
