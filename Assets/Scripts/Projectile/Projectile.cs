using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int _damage = 1;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
}
