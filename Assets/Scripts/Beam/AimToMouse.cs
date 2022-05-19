using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToMouse : MonoBehaviour
{
    [SerializeField] private Camera     mainCamera;
    [SerializeField] private Transform  target;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        if (target)
        {
            target.transform.position = mousePos;
        }

        Vector2 armPos = transform.position;

        Vector2 delta = (mousePos - armPos).normalized;
        Vector2 upDirection = new Vector2(-delta.y, delta.x);

        Debug.Log($"upDirection={upDirection}");

        transform.rotation = Quaternion.LookRotation(Vector3.forward, upDirection);
    }
}
