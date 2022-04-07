using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform  target;
    [SerializeField] private Vector3    offset;
    [SerializeField] private float      speed = 1.0f;
    [SerializeField] private Rect       cameraLimits;

    void Update()
    {
        if (target != null)
        {
            Vector3 newPos = target.position + offset;
            newPos.z = transform.position.z;

            if (newPos.x > cameraLimits.xMax) newPos.x = cameraLimits.xMax;
            else if (newPos.x < cameraLimits.xMin) newPos.x = cameraLimits.xMin;
            if (newPos.y > cameraLimits.yMax) newPos.y = cameraLimits.yMax;
            else if (newPos.y < cameraLimits.yMin) newPos.y = cameraLimits.yMin;

            Vector3 delta = newPos - transform.position;
            transform.position = transform.position + delta * speed * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Camera camera = GetComponent<Camera>();
        float  cameraHeight = camera.orthographicSize;
        float  cameraWidth = cameraHeight * camera.aspect;

        var p1 = new Vector3(cameraLimits.xMin - cameraWidth, cameraLimits.yMin - cameraHeight, 0.0f);
        var p2 = new Vector3(cameraLimits.xMax + cameraWidth, cameraLimits.yMin - cameraHeight, 0.0f);
        var p3 = new Vector3(cameraLimits.xMax + cameraWidth, cameraLimits.yMax + cameraHeight, 0.0f);
        var p4 = new Vector3(cameraLimits.xMin - cameraWidth, cameraLimits.yMax + cameraHeight, 0.0f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
