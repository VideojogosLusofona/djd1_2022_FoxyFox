using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformRight : MonoBehaviour
{
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        transform.position = transform.position + transform.right * speed * Time.fixedDeltaTime;        
    }
}
