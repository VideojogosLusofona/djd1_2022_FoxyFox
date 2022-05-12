using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private int        score = 0;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private IntValue   scoreValue;

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player == null) return;

        scoreValue.ChangeValue(score);

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
