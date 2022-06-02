using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour
{
    [SerializeField] private float      speed;
    [SerializeField] private Vector2    direction;
    [SerializeField] private string     samplerName = "_MainTex";
    [SerializeField] private int        numberSteps = 0;


    private Material material;
    private Vector2  currentUVOffset;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            material = renderer.material;

            currentUVOffset = material.GetTextureOffset(samplerName);
        }
    }

    void Update()
    {
        if (material == null) return;

        currentUVOffset = currentUVOffset + speed * direction * Time.deltaTime;

        if (currentUVOffset.x < -1.0f) currentUVOffset.x += 1.0f;
        if (currentUVOffset.x > 1.0f) currentUVOffset.x -= 1.0f;
        if (currentUVOffset.y < -1.0f) currentUVOffset.y += 1.0f;
        if (currentUVOffset.y > 1.0f) currentUVOffset.y -= 1.0f;

        Vector2 stepOffset = currentUVOffset;

        if (numberSteps > 0)
        {
            stepOffset = stepOffset * numberSteps;
            stepOffset.x = Mathf.Floor(stepOffset.x);
            stepOffset.y = Mathf.Floor(stepOffset.y);
            stepOffset = stepOffset / numberSteps;
        }

        material.SetTextureOffset(samplerName, stepOffset);
    }
}
