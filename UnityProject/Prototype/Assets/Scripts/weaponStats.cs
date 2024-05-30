using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]

public class weaponStats : ScriptableObject
{
    public GameObject weaponModel;
    [Range(1,10)] public int weaponDmg;
    [Range(1,500)]public int weaponDistance;
    [Range(.01f,3)] public float weaponSpeed;
    public int ammoCur;
    public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip weaponSound;
    [Range(0, 1)] public float weaponVol;
    public Image icon;
}
