using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntPatrol : MonoBehaviour
{
    [SerializeField] private float      speed = 100;
    [SerializeField] private Transform  wallProbe;
    [SerializeField] private Transform  groundProbe;
    [SerializeField] private float      probeRadius = 5;
    [SerializeField] private LayerMask  probeMask;
    [SerializeField] private float      deathAngle = 20;

    private Rigidbody2D rb;
    private float       dirX = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        Vector3 currentVelocity = rb.velocity;

        Collider2D collider = Physics2D.OverlapCircle(wallProbe.position, probeRadius, probeMask);
        if (collider != null)
        {
            currentVelocity = SwitchDirection(currentVelocity);
        }
        else
        {
            collider = Physics2D.OverlapCircle(groundProbe.position, probeRadius, probeMask);

            if (collider == null)
            {
                currentVelocity = SwitchDirection(currentVelocity);
            }
        }

        currentVelocity.x = speed * dirX;

        rb.velocity = currentVelocity;

        if ((currentVelocity.x > 0) && (transform.right.x < 0))
        {
            transform.rotation = Quaternion.identity;
        }
        else if ((currentVelocity.x < 0) && (transform.right.x > 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private Vector3 SwitchDirection(Vector3 currentVelocity)
    {
        dirX = -dirX;

        if (currentVelocity.y > 0)
        {
            currentVelocity.y = 0;
        }

        return currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            if (player.transform.position.y > transform.position.y)
            {
                Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
                float       dp = Vector3.Dot(Vector3.down, playerRB.velocity.normalized);
                float       angle = Mathf.Acos(dp) * Mathf.Rad2Deg;

                Debug.Log("Angle = " + angle);

                if (angle < deathAngle)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            Destroy(player.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (wallProbe)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
            Gizmos.DrawSphere(wallProbe.position, probeRadius);
        }

        if (groundProbe)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(groundProbe.position, probeRadius);
        }
    }
}
