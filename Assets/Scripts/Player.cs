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

    private Rigidbody2D rb;
    private Animator    anim;
    private float       jumpTime;

    private void Start()
    {
        //Debug.Log($"{name}: Started Player");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.Log($"{name}: Updated Player");

        float hAxis = Input.GetAxis("Horizontal");

        //Debug.Log($"Horizontal Axis = {hAxis}");
        //transform.position = transform.position + hAxis * Vector3.right * hSpeed * Time.deltaTime;

        bool        onGround = IsOnGround();
        bool        onGroundSecondary = IsOnGroundSecondary();
        bool        canJump = onGround || onGroundSecondary;
        Vector3     currentVelocity = rb.velocity;
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
        anim.SetFloat("SpeedY", currentVelocity.y);
        anim.SetBool("onGround", canJump);

        if ((currentVelocity.x > 0) && (transform.right.x < 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if ((currentVelocity.x < 0) && (transform.right.x > 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
}
