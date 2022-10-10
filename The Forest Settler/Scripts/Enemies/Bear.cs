using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : Animal
{

    private float timer = 0;
    private bool check = false;

    public override void attack()
    {
        playerStatus.setHp(playerStatus.getCurrHP() - damage);
    }

    public override void die()
    {
        Animator banimate = GetComponent<Animator>();
        NavMeshAgent obj = GetComponent<NavMeshAgent>();
        obj.isStopped = true;
        banimate.SetBool("Idle", false);
        banimate.SetBool("Run Forward", false);
        banimate.SetBool("Death", true);
        if (timer >= 4f)
        {
            check = true;
            timer = 0;
            gameObject.SetActive(false);
            playerStatus.setXP(playerStatus.getCurrXP() + xpAmount);
        }
    }

    public override void takeDamage()
    {
        health -= PlayerStatusController.instance.getCurrentDMG();
    }

    public override void takeDamageWeapon()
    {
        health -= (PlayerStatusController.instance.getCurrentDMG() * PlayerStatusController.instance.weaponDamage);
    
    }

    // Start is called before the first frame update
    void Start()
    {
        this.maxHp = 1000;
        this.health = maxHp;
        this.xpAmount = 14;
        this.damage = 90;
        this.isAlive = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hb.setMaxHealth(maxHp);
        hb.setHealth(health);


        checkDeath();
        //debug purpose
        //if (Input.GetKeyDown(KeyCode.T)) health -= 100;


    }

    void checkDeath()
    {
  
        if(health <= 0)
        {
            isAlive = false;
            check = false;
            if (!check)
            {
                timer += Time.deltaTime;
            }
            if(!isAlive && !check)
            {
                die();                
                
            }
        }
    }

    public override void revive()
    {
        health = maxHp;
        isAlive = true;
        check = false;
        Animator banimate = GetComponent<Animator>();
        NavMeshAgent obj = GetComponent<NavMeshAgent>();
        obj.isStopped = false;
        banimate.SetBool("Death", false);
    }
}
