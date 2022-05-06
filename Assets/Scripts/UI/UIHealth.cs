using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private Image[]  hearts;

    private Player player;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player == null)
            {
                return;
            }
        }

        int health = player.GetHealth();
        for (int i = 0; i < health; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
        
        for (int i = health; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }
}
