using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    [SerializeField]
    private int maxHP, currHP, damage;

    private float maxXP, currXP, level;
    private Animator animate;

    public bool playerAlive,aliveAnimate, attack;
    private bool weaponHand;
    private float timer = 0f;

    private float str, agi, vit;

    public int weaponDamage;

    public HealthBar hb;
    public ExperienceBar expb;


    public static PlayerStatusController instance;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    void Start()
    {

        level = 1;

        maxHP = 1000;
        currHP = maxHP;

        maxXP = Mathf.Pow(level / 0.3f, 2);
        currXP = 0;

        animate = GetComponent<Animator>();
        playerAlive = true;
        aliveAnimate = true;
        weaponHand = PlayerPickupController.getWeaponStatus();
        damage = 63;
        weaponDamage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        hb.setMaxHealth(maxHP);
        hb.setHealth(currHP);

        expb.setMaxXP(maxXP);
        expb.setCurrentXP(currXP);
        expb.setLevel(level);
        checkPlayerDeath();
        weaponHand = PlayerPickupController.getWeaponStatus();
        if (weaponHand) weaponDamage = 0;
        else if (!weaponHand) weaponDamage = 2;

        if(currXP >= maxXP)
        {
            level++;
            maxXP = Mathf.Pow(level / 0.3f, 2);
        }

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    currHP += 20;
        //    hb.setHealth(currHP);
        //}

        //if (Input.GetKey(KeyCode.K))
        //{
        //    currXP += 20;
        //    expb.setCurrentXP(currXP);
        //}
    }

    void checkPlayerDeath()
    {
        if (currHP <= 0) {
            currHP = 0;
            timer += Time.deltaTime;
            if (aliveAnimate)
            {
                animate.SetTrigger("isDeath");
                aliveAnimate = false;
                PlayerController.playerSpeed = 0;
                StartCoroutine(delay());
            }
            if (timer >= 2f) { 
                PlayerController.playerSpeed = 0;
                playerAlive = false;
            }
        }
        
    }

    public int getCurrHP()
    {
        return currHP;
    }
    public float getMaxHP()
    {
        return maxHP;
    }
    public float getCurrXP()
    {
        return currXP;
    }
    public float getMaxXP()
    {
        return maxXP;
    }

    public float getLevel()
    {
        return level;
    }

    public int getCurrentDMG()
    {
        return damage;
    }

    public void setHp(int hp)
    {
        currHP = hp;
    }

    public void setXP(float xp)
    {
        currXP = xp;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
    }
}
