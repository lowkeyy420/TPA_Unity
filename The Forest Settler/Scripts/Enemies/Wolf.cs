using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : Animal
{

    private float timer = 0;
    private bool check = false;

    public override void attack()
    {
        playerStatus.setHp(playerStatus.getCurrHP() - damage);
    }

    public override void die()
    {

    }

    public override void revive()
    {
        
    }

    void checkDeath()
    {

        if (health <= 0)
        {
            isAlive = false;
            check = false;
            if (!check)
            {
                timer += Time.deltaTime;
            }
            if (!isAlive && !check)
            {
                Animator banimate = GetComponent<Animator>();
                NavMeshAgent obj = GetComponent<NavMeshAgent>();
                obj.isStopped = true;
                banimate.SetBool("Idle", false);
                banimate.SetBool("Run Forward", false);
                banimate.SetBool("Death", true);
                check = true;
                timer = 0;
            }
        }
    }

    public override void takeDamage()
    {
        health -= PlayerStatusController.instance.getCurrentDMG();

    }

    public override void takeDamageWeapon()
    {
        health = maxHp;
        isAlive = true;
        check = false;
        Animator banimate = GetComponent<Animator>();
        NavMeshAgent obj = GetComponent<NavMeshAgent>();
        obj.isStopped = false;
        banimate.SetBool("Death", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.health = 1200;
        this.xpAmount = 28;
        this.damage = 200;
        this.isAlive = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hb.setMaxHealth(maxHp);
        hb.setHealth(health);
    }
}
