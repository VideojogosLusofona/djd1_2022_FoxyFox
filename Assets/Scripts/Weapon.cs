using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/RPG/Weapon")]
public class Weapon : ScriptableObject
{
    public enum Type { Ice, Fire, Earth, Water, Avatar };

    [SerializeField] private int    damage;
    [SerializeField] private Type   damageType;
    [SerializeField] private int    protection;
    [SerializeField] private Sprite image;
    
}
