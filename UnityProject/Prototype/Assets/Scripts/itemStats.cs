using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]

public class itemStats : ScriptableObject
{
    public string itemName;

    [Header("item Info")]
    public Sprite icon;
    public GameObject itemModel;

    [Header("Sound")]
    public AudioClip itemSound;
    [Range(0, 1)] public float vol;

    [Header("Potions")]
    public int healthAmt;
    public int fearAmt;
    public bool isStackable;
    public int maxStackSize;

    [Header("Weapon")]
    [Range(1, 10)] public int weaponDmg;
    [Range(1, 500)] public int weaponDistance;
    [Range(.01f, 3)] public float weaponSpeed;
    public int ammoCur;
    public int ammoMax;
    public ParticleSystem hitEffect;

    [Header("Key")]
    public bool isKey;
    public int keyID;
}
