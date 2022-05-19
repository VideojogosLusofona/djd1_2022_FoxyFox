using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private Camera         mainCamera;
    [SerializeField] private LineRenderer   line;
    [SerializeField] private float          maxLength = 200;
    [SerializeField] private float          growSpeed = 100.0f;
    [SerializeField] private float          animSpeedU = 1.0f;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private LayerMask      obstacleMask;
    [SerializeField] private float          damageRadius = 5.0f;
    [SerializeField] private LayerMask      damageMask;

    private float       length;
    private Material    material;
    private float       materialU;

    void Start()
    {
        material = new Material(line.material);
        line.material = material;
        materialU = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            length = 0;
        }
        else if (Input.GetButton("Fire1"))
        {
            Vector2 mousePos = Input.mousePosition;

            mousePos = mainCamera.ScreenToWorldPoint(mousePos);

            Vector2 startPos = transform.position;
            Vector2 delta = (mousePos - startPos);
            float   maxDistance = delta.magnitude;

            length = Mathf.Clamp(length + Time.deltaTime * growSpeed, 0, Mathf.Min(maxLength, maxDistance));

            delta.Normalize();
            Vector2 targetPos = startPos + delta * length;

            RaycastHit2D hitInfo;
            hitInfo = Physics2D.Raycast(startPos, delta, maxDistance, obstacleMask);
            if (hitInfo)
            {
                targetPos = hitInfo.point;
            }

            line.enabled = true;
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetPos);

            line.material.SetTextureOffset("_MainTex", new Vector2(materialU, 0));
            materialU = materialU - Time.deltaTime * animSpeedU;

            var emission = particleSystem.emission;
            emission.enabled = true;
            particleSystem.transform.position = targetPos;

            var collisions = Physics2D.OverlapCircleAll(targetPos, damageRadius, damageMask);
            foreach (var collision in collisions)
            {
                var patrolAnt = collision.GetComponentInParent<AntPatrol>();
                if (patrolAnt)
                {
                    patrolAnt.Freeze();
                }
            }
        }
        else
        {
            var emission = particleSystem.emission;
            emission.enabled = false;
            line.enabled = false;            
        }
    }
}
