using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField] private float      initialVelocity = 50.0f;
    [SerializeField] private float      fuseTime = 2.0f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float      explosionRadius = 100;
    [SerializeField] private LayerMask  explosionMask;

    private Rigidbody2D rb;
    private float       fuseTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 v = new Vector2(Random.Range(1.0f, 2.0f), Random.Range(0.0f, 1.0f));
        v.Normalize();
        v.x = transform.right.x;
        v = v * initialVelocity;

        rb.velocity = v;
        rb.angularVelocity = Random.Range(-90.0f, -180.0f) * transform.right.x;

        fuseTimer = fuseTime;
    }

    // Update is called once per frame
    void Update()
    {
        fuseTimer -= Time.deltaTime;

        if (fuseTimer < 0)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);
            foreach (var collider in colliders)
            {
                var enemy = collider.GetComponentInParent<AntPatrol>();
                if (enemy)
                {
                    enemy.DealDamage(_damage);
                }
                var gem = collider.GetComponent<Gem>();
                if (gem)
                {
                    Destroy(gem.gameObject);
                }
                var player = collider.GetComponent<Player>();
                if (player)
                {
                    player.DealDamage(_damage, transform);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
