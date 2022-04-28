using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float          horizontalSpeed = 200.0f;
    [SerializeField] private float          jumpSpeed = 200.0f;
    [SerializeField] private Transform[]    groundProbe;
    [SerializeField] private float          groundProbeRadius = 5.0f;
    [SerializeField] private LayerMask      groundMask;
    [SerializeField] private float          maxJumpTime = 0.1f;
    [SerializeField] private float          fallGravityScale = 5.0f;
    [SerializeField] private Collider2D     groundCollider;
    [SerializeField] private Collider2D     airCollider;
    [SerializeField] private int            maxHealth = 3;
    [SerializeField] private float          invulnerabilityTime = 2.0f;
    [SerializeField] private float          blinkTime = 0.1f;
    [SerializeField] private Vector2        knockbackIntensity = new Vector2(200.0f, 200.0f);
    [SerializeField] private float          knockbackTime = 0.25f;
    [SerializeField] private float          deadTime = 5.0f;

    private Rigidbody2D     rb;
    private Animator        anim;
    private SpriteRenderer  spriteRenderer;
    private float           jumpTime;
    private int             health;
    private float           invulnerabilityTimer = 0.0f;
    private float           blinkTimer = 0.0f;
    private float           lockInputTimer = 0.0f;
    private float           deadTimer = 0.0f;

    public bool isInvulnerable
    {
        get { return invulnerabilityTimer > 0.0f; }
        set { if (value) invulnerabilityTimer = invulnerabilityTime; else invulnerabilityTimer = 0.0f; }
    }

    public bool isInputLocked => (lockInputTimer > 0) || (deadTimer > 0);

    private void Start()
    {
        //Debug.Log($"{name}: Started Player");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    private void Update()
    {
        bool    onGround = IsOnGround();
        bool    onGroundSecondary = IsOnGroundSecondary();
        bool    canJump = onGround || onGroundSecondary;
        Vector3 currentVelocity = rb.velocity;

        if (deadTimer > 0)
        {
            deadTimer -= Time.deltaTime;

            if (deadTimer <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }

        if (isInputLocked)
        {
            if (lockInputTimer > 0)
            {
                lockInputTimer -= Time.deltaTime;
            }
        }
        else
        {
            //Debug.Log($"{name}: Updated Player");

            float hAxis = Input.GetAxis("Horizontal");

            //Debug.Log($"Horizontal Axis = {hAxis}");
            //transform.position = transform.position + hAxis * Vector3.right * hSpeed * Time.deltaTime;

            currentVelocity.x = hAxis * horizontalSpeed;

            groundCollider.enabled = onGround;
            airCollider.enabled = !onGround;

            if (Input.GetButtonDown("Jump"))
            {
                if (canJump)
                {
                    rb.gravityScale = 1.0f;
                    currentVelocity.y = jumpSpeed;
                    jumpTime = Time.time;
                }
            }
            else if (Input.GetButton("Jump"))
            {
                float elapsedTime = Time.time - jumpTime;
                if (elapsedTime > maxJumpTime)
                {
                    rb.gravityScale = fallGravityScale;
                }
            }
            else
            {
                rb.gravityScale = fallGravityScale;
            }

            rb.velocity = currentVelocity;

            anim.SetFloat("AbsSpeedX", Mathf.Abs(currentVelocity.x));

            if ((currentVelocity.x > 0) && (transform.right.x < 0))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if ((currentVelocity.x < 0) && (transform.right.x > 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        anim.SetFloat("SpeedY", currentVelocity.y);
        anim.SetBool("onGround", canJump);        

        if (isInvulnerable)
        {
            invulnerabilityTimer = invulnerabilityTimer - Time.deltaTime;

            if (invulnerabilityTimer <= 0)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                blinkTimer = blinkTimer - Time.deltaTime;

                if (blinkTimer < 0)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    blinkTimer = blinkTime;
                }
            }
        }
    }

    private bool IsOnGround()
    {
        var collider = Physics2D.OverlapCircle(groundProbe[0].position, groundProbeRadius, groundMask);

        return (collider != null);
    }

    private bool IsOnGroundSecondary()
    {
        var collider = Physics2D.OverlapCircle(groundProbe[1].position, groundProbeRadius, groundMask);

        if (collider != null) return true;

        collider = Physics2D.OverlapCircle(groundProbe[2].position, groundProbeRadius, groundMask);

        if (collider != null) return true;

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundProbe != null)
        {
            foreach (var probe in groundProbe)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(probe.position, groundProbeRadius);
            }
        }
    }

    public void DealDamage(int damage, Transform sourceDamageTransform)
    {
        if (isInvulnerable) return;

        health = health - damage;

        if (health <= 0)
        {
            anim.SetTrigger("Dead");

            deadTimer = deadTime;

            groundCollider.enabled = false;
            airCollider.enabled = false;

            rb.velocity = new Vector2(0.0f, jumpSpeed);
        }
        else
        {
            anim.SetTrigger("Hurt");

            isInvulnerable = true;

            if (sourceDamageTransform != null)
            {
                Vector2 direction = new Vector2(0.0f, 1.0f);

                //if (sourceDamageTransform.position.x > transform.position.x) direction.x = -1.0f;
                //else direction.x = 1.0f;
                // Next code does the same as previous commented code
                direction.x = Mathf.Sign(transform.position.x - sourceDamageTransform.position.x);

                Knockback(direction.normalized);
            }
        }
    }

    public void Knockback(Vector2 direction)
    {
        rb.velocity = direction * knockbackIntensity;

        lockInputTimer = knockbackTime;
    }
}
