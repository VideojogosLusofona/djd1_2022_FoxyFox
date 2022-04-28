using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAnt : MonoBehaviour
{
    [SerializeField] private float      speed = 100.0f;
    [SerializeField] private Transform  wallProbe;
    [SerializeField] private Transform  groundProbe;
    [SerializeField] private float      probeRadius = 5.0f;
    [SerializeField] private LayerMask  probeMask;
    [SerializeField] private float      deathAngle = 20.0f;
    [SerializeField] private int        damage = 1;
    [SerializeField] private int        maxHealth = 1;

    private float dirX = 1;
    private int   health;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        Vector3 currentVelocity = rb.velocity;

        Collider2D wallCollider = Physics2D.OverlapCircle(wallProbe.position, probeRadius, probeMask);
        if (wallCollider)
        {
            currentVelocity = ReverseDirection(currentVelocity);
        }
        else
        {
            Collider2D groundCollider = Physics2D.OverlapCircle(groundProbe.position, probeRadius, probeMask);

            if (groundCollider == null)
            {
                currentVelocity = ReverseDirection(currentVelocity);
            }
        }

        currentVelocity.x = dirX * speed;

        rb.velocity = currentVelocity;

        if ((currentVelocity.x < 0) && (transform.right.x > 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if ((currentVelocity.x > 0) && (transform.right.x < 0))
        {
            transform.rotation = Quaternion.identity;
        }
    }

    Vector3 ReverseDirection(Vector3 currentVelocity)
    {
        dirX = -dirX;
        if (currentVelocity.y > 0)
        {
            currentVelocity.y = 0;
        }

        return currentVelocity;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Player player = collider.GetComponentInParent<Player>();

        if (player != null)
        {
            Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>();
            if (rbPlayer)
            {
                float dp = Vector3.Dot(Vector3.down, rbPlayer.velocity.normalized);

                if (dp > Mathf.Cos(deathAngle * Mathf.Deg2Rad))
                {
                    Vector2 currentPlayerVelocity = rbPlayer.velocity;
                    currentPlayerVelocity.y = 200;
                    rbPlayer.velocity = currentPlayerVelocity;
                    DealDamage(1);
                    return;
                }
            }

            player.DealDamage(damage, transform);
        }
    }

    public void DealDamage(int damage)
    {
        health = health - damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (wallProbe != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(wallProbe.position, probeRadius);
        }

        if (groundProbe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundProbe.position, probeRadius);
        }
    }
}
