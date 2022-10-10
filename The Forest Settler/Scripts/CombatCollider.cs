using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    private bool weapon_hand;
    PlayerStatusController atk = PlayerStatusController.instance;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.tag == "Bear")
        {
            //punch
            if (weapon_hand && PlayerStatusController.instance.attack) {
                GameObject enmy = other.gameObject;
                Bear bear = enmy.GetComponent<Bear>();
                bear.takeDamage();

            } 

            //sword
            else if (!weapon_hand && PlayerStatusController.instance.attack)
            {
                GameObject enmy = other.gameObject;
                Bear bear = enmy.GetComponent<Bear>();
                bear.takeDamageWeapon();
            }
        }
        if(other.gameObject.tag == "Wolf")
        {
            //punch
            if (weapon_hand && PlayerStatusController.instance.attack) {
                GameObject enmy = other.gameObject;
                Wolf bear = enmy.GetComponent<Wolf>();
                bear.takeDamage();
            } 

            //sword
            else if (!weapon_hand && PlayerStatusController.instance.attack)
            {
                GameObject enmy = other.gameObject;
                Wolf bear = enmy.GetComponent<Wolf>();
                bear.takeDamageWeapon();
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        weapon_hand = PlayerPickupController.getWeaponStatus();
    }
}
