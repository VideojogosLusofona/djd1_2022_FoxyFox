using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null) return;

        var enemy = collision.GetComponentInParent<AntPatrol>();
        if (enemy)
        {
            enemy.DealDamage(_damage);
        }

        Destroy(gameObject);
    }

}
