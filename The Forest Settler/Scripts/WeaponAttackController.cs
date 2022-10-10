using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackController : MonoBehaviour
{
    public Bear bear;
    public Wolf wolf;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bear")
        {
            bear = other.gameObject.GetComponent<Bear>();
            bear.health -= PlayerStatusController.instance.getCurrentDMG();
        }

        else if(other.gameObject.tag == "Bear")
        {
            wolf = other.gameObject.GetComponent<Wolf>();
            wolf.health -= PlayerStatusController.instance.getCurrentDMG();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
