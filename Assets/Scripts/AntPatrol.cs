using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntPatrol : MonoBehaviour
{
    [SerializeField] private float          speed = 100;
    [SerializeField] private Transform      wallProbe;
    [SerializeField] private Transform      groundProbe;
    [SerializeField] private float          probeRadius = 5;
    [SerializeField] private LayerMask      probeMask;
    [SerializeField] private float          deathAngle = 20;
    [SerializeField] private int            damage = 1;
    [SerializeField] private int            maxHealth = 1;
    [SerializeField] private GameObject     deathEffectPrefab;
    [SerializeField] private Transform      deathEffectSpawnPoint;
    [SerializeField] private IntValue       scoreValue;
    [SerializeField] private FloatValue     timeValue;
    [SerializeField] private LineRenderer   trappedFX;

    private Rigidbody2D rb;
    private float       dirX = 1;
    private int         health;

    private float       freezeTimer = 0.0f;
    private bool        frozenThisFrame;
    private float       thawTimer = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        Vector3 currentVelocity = rb.velocity;

        if (thawTimer > 0.0f)
        {
            currentVelocity.x = 0;

            thawTimer -= Time.deltaTime;
        }
        else
        {

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
        }

        if ((currentVelocity.x > 0) && (transform.right.x < 0))
        {
            transform.rotation = Quaternion.identity;
        }
        else if ((currentVelocity.x < 0) && (transform.right.x > 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        rb.velocity = currentVelocity;
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

    private void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            if (player.transform.position.y > transform.position.y)
            {
                Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
                float       dp = Vector3.Dot(Vector3.down, playerRB.velocity.normalized);
                float       angle = Mathf.Acos(dp) * Mathf.Rad2Deg;

                if (angle < deathAngle)
                {
                    DealDamage(1);

                    Vector2 currentPlayerVelocity = playerRB.velocity;
                    currentPlayerVelocity.y = player.GetJumpSpeed();
                    playerRB.velocity = currentPlayerVelocity;

                    return;
                }
            }
            player.DealDamage(damage, transform);
        }
    }

    public void DealDamage(int damage)
    {
        health = health - damage;

        Debug.Log($"Ouch Enemy, health={health}");

        if (health <= 0)
        {
            Destroy(gameObject);

            scoreValue.ChangeValue(10);

            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, deathEffectSpawnPoint.position, deathEffectSpawnPoint.rotation);
            }

            if (timeValue != null)
            {
                timeValue.ChangeValue(5.0f);
            }
        }
        else
        {

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

    public void Freeze()
    {
        freezeTimer += Time.deltaTime;
        frozenThisFrame = true;

        if (freezeTimer > 2.0f)
        {
            thawTimer = 2.0f;
        }
    }

    private void LateUpdate()
    {
        if (!frozenThisFrame)
        {
            freezeTimer = 0;
        }

        if (trappedFX)
        {
            if ((freezeTimer > 0) || (thawTimer > 0))
            {
                trappedFX.enabled = true;

                var color = trappedFX.startColor;
                color.a = Mathf.Clamp01(Mathf.Max(freezeTimer, thawTimer) / 2.0f);
                trappedFX.startColor = color;
                color = trappedFX.endColor;
                color.a = Mathf.Clamp01(Mathf.Max(freezeTimer, thawTimer) / 2.0f);
                trappedFX.endColor = color;
            }
            else
            {
                trappedFX.enabled = false;
            }
        }

        frozenThisFrame = false;
    }
}
