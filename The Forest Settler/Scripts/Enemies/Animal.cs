using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    // Start is called before the first frame update

    public int health, maxHp;
    public float xpAmount;
    public int damage;
    public bool isAlive = true;
    public PlayerStatusController playerStatus = PlayerStatusController.instance;
    public HealthBar hb;

    public abstract void attack();
    public abstract void die();

    public abstract void takeDamage();
    public abstract void takeDamageWeapon();

    public abstract void revive();

}
