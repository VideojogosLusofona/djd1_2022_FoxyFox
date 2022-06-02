using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float          horizontalSpeed = 200.0f;
    [SerializeField] private float          jumpSpeed = 200.0f;
    [SerializeField] private Transform      groundProbe;
    [SerializeField] private float          groundProbeRadius = 5.0f;
    [SerializeField] private LayerMask      groundMask;
    [SerializeField] private float          maxJumpTime = 0.1f;
    [SerializeField] private float          fallGravityScale = 5.0f;
    [SerializeField] private int            maxHealth = 3;
    [SerializeField] private float          invulnerabilityTime = 2.0f;
    [SerializeField] private float          blinkTime = 0.1f;
    [SerializeField] private float          knockbackIntensity = 100.0f;
    [SerializeField] private float          knockbackDuration = 0.5f;
    [SerializeField] private float          deadTime = 3.0f;
    [SerializeField] private TimeUpdater    timeUpdater;
    [SerializeField] private Projectile     projectilePrefab;
    [SerializeField] private Transform      shootPoint;
    [SerializeField] private float          cooldownTime;
    [SerializeField] private float          projectileDamageMultiplier = 1.0f;
    [SerializeField] private AudioClip      hurtSound;

    private Rigidbody2D     rb;
    private Animator        anim;
    private SpriteRenderer  spriteRenderer;
    private float           jumpTime;
    private int             health;
    private float           invulnerabilityTimer = 0;
    private float           blinkTimer = 0;
    private float           inputLockTimer = 0;
    private float           deadTimer = 0;
    private float           cooldownTimer = 0;

    private bool isInvulnerable
    {
        get { return invulnerabilityTimer > 0; }
        set { if (value) invulnerabilityTimer = invulnerabilityTime; else invulnerabilityTimer = 0; }
    }

    private bool isInputLocked => (inputLockTimer > 0) || (deadTimer > 0);

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
        Vector3 currentVelocity = rb.velocity;
        bool    onGround = IsOnGround();

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
            inputLockTimer -= Time.deltaTime;
            if (timeUpdater)
            {
                timeUpdater.SetScale(1.0f);
            }
        }
        else
        {
            float hAxis = Input.GetAxis("Horizontal");
            currentVelocity.x = hAxis * horizontalSpeed;

            if (timeUpdater != null)
            {
                if (Mathf.Abs(hAxis) > 0) timeUpdater.SetScale(1.0f);
                else timeUpdater.SetScale(0.0f);
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (onGround)
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

            if ((currentVelocity.x > 0) && (transform.right.x < 0))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if ((currentVelocity.x < 0) && (transform.right.x > 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        anim.SetFloat("AbsSpeedX", Mathf.Abs(currentVelocity.x));
        anim.SetFloat("SpeedY", currentVelocity.y);
        anim.SetBool("onGround", onGround);

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer > 0)
            {
                blinkTimer -= Time.deltaTime;

                if (blinkTimer < 0)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    blinkTimer = blinkTime;
                }
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }

        if (cooldownTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                projectile.Damage = Mathf.FloorToInt(projectile.Damage * projectileDamageMultiplier);

                cooldownTimer = cooldownTime;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private bool IsOnGround()
    {
        var collider = Physics2D.OverlapCircle(groundProbe.position, groundProbeRadius, groundMask);

        return (collider != null);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundProbe != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundProbe.position, groundProbeRadius);
        }
    }

    public void DealDamage(int damage, Transform sourceDamageTransform)
    {
        if (isInvulnerable) return;

        health = health - damage;

        SoundManager.Get().Play(hurtSound);

        if (health <= 0)
        {
            anim.SetTrigger("Dead");

            rb.velocity = new Vector2(0, jumpSpeed * 2);
            spriteRenderer.sortingOrder = 2;

            deadTimer = deadTime;

            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            isInvulnerable = true;

            if (sourceDamageTransform != null)
            {
                anim.SetTrigger("Hurt");

                Vector2 direction = new Vector2(0.0f, 1.0f);

                //if (transform.position.x < sourceDamageTransform.position.x) direction.x = -1.0f;
                //else direction.x = 1.0f;

                direction.x = Mathf.Sign(transform.position.x - sourceDamageTransform.position.x);

                Knockback(direction);
            }
        }
    }

    public float GetJumpSpeed()
    {
        return jumpSpeed;
    }

    private void Knockback(Vector2 direction)
    {
        rb.velocity = direction * knockbackIntensity;

        inputLockTimer = knockbackDuration;
    }

    public int GetHealth()
    {
        return health;
    }

}
