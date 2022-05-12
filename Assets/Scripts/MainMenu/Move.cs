using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Vector3 moveSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + moveSpeed * Time.deltaTime;        
    }
}
