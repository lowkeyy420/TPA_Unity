using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private float initialPlayerSpeed = 5f, rollSpeed;
    public static float playerSpeed;

    [SerializeField]
    private float sprintSpeed = 10f;

    [SerializeField]
    private float maxRotationSpeed = 0.1f;

    [SerializeField]
    private Transform playerCamera;

    private float curr_velocity;

    private float maxCombo, maxComboSword;
    private float timerRoll = 0, timerCombo = 0, comboDuration, comboDurationSword;
    private bool roll, inCombat, firstAttack;

    [SerializeField]
    private Animator animate;

    private Vector3 mov_dir, dir;

    private PlayerStatusController player;
    private AudioController sound;



    // Start is called before the first frame update
    void Start()
    {
        player = PlayerStatusController.instance;
        sound = AudioController.instance;
        controller = GetComponent<CharacterController>();
        // set cursor locked && gone
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //initialize var
        maxCombo = 0;
        maxComboSword = 0;
        timerRoll = 0;
        timerCombo = 0;
        comboDuration = 0;
        comboDurationSword = 0;
        roll = false;
        sprintSpeed = 10f;
        rollSpeed = sprintSpeed + 2;
        initialPlayerSpeed = 7f;
        playerSpeed = initialPlayerSpeed;
        inCombat = false;
        sound.startBGM();
        sound.startBGM2();
}

    // Update is called once per frame
    void Update()
    {

        playerDeath();
        float horizontal_inp = Input.GetAxisRaw("Horizontal");  //x
        float vertical_inp = Input.GetAxisRaw("Vertical"); //z
        float stiffAngle;
        float smoothAngle;
        
        dir = new Vector3(horizontal_inp, 0, vertical_inp).normalized; //normalized set all dir have the same max value (no diagonal speed increase)
        Vector3 dir_h = new Vector3(horizontal_inp, 0, 0).normalized;
        Vector3 dir_v = new Vector3(0, 0, vertical_inp).normalized;
        mov_dir = new Vector3();

        checkCursor();

        /* Movement logic */

        if (player.aliveAnimate)
        {
            checkRun();
            checkAttack();
        }
        if (dir.magnitude > 0.2f && player.aliveAnimate) // If player is moving
        {

            //stiff player rotation
            stiffAngle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y);
            //smooth player rotation
            smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, stiffAngle, ref curr_velocity, maxRotationSpeed);
     
            animate.SetFloat("X", horizontal_inp);
            animate.SetFloat("Z", vertical_inp);

            if( dir_v.magnitude > 0.2f)   //if player is moving W / S
            {
                animate.SetBool("isStrafing", false);
                animate.SetBool("isRunning", true);
                //rotate player based on direction
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }

            //if player is moving A / D
            if ( dir_h.magnitude > 0.2f && animate.GetBool("isRunning") == true)
            {
                animate.SetBool("isStrafing", false);
                animate.SetBool("isRunning", true);
                //rotate player based on direction
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            }
            if( dir_h.magnitude > 0.2f && animate.GetBool("isRunning") == false)
            {
                animate.SetBool("isStrafing", true);
                animate.SetBool("isRunning", false);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animate.SetBool("isStrafing", false);
                    animate.SetBool("isRunning", true);
                }
            }
            

            //real player movement direction
            mov_dir = (Quaternion.Euler(0, stiffAngle, 0) * Vector3.forward).normalized;
            // set speed
            mov_dir.x *= playerSpeed;
            mov_dir.z *= playerSpeed;

            checkRoll();

        }
        else
        {
            animate.SetBool("isRunning", false);
            animate.SetBool("isStrafing", false);
        }

        if (dir.magnitude <= 0.2f && !Input.GetKeyDown(KeyCode.LeftShift)) playerSpeed = 5f;


        // add gravity
        mov_dir.y -= 9.8f;

        //move player
        controller.Move(mov_dir * Time.deltaTime);
            
    }

    void checkRun()
    {
        // Running
        if (Input.GetKey(KeyCode.LeftShift)) playerSpeed = sprintSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftShift)) playerSpeed = initialPlayerSpeed;
    }

    void checkCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (Input.GetKeyDown(KeyCode.LeftAlt) && Cursor.visible == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void checkAttack()
    {
        bool weapon_hand = PlayerPickupController.getWeaponStatus();
        firstAttack = true;

        //check if player hit once / in combat state
        if (inCombat)
        {
            firstAttack = false;
            timerCombo += Time.deltaTime;
            
            if(weapon_hand) comboDuration += Time.deltaTime;
            else if ( !weapon_hand) comboDurationSword += Time.deltaTime;

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift) && !roll)
        {
                // hand
            // hand first time punch
            if (maxCombo == 0 && weapon_hand && !inCombat && timerCombo == 0 && firstAttack)
            {
                animate.SetTrigger("Punch1");
                maxCombo = 1;
                inCombat = true;
                firstAttack = false;
                playerSpeed = 0f;
                sound.hit1();
            }

            // hand combat in combo duration
            if (inCombat && maxCombo > 0 && comboDuration <= 2.4f && weapon_hand)
            {
                if (maxCombo == 0 && timerCombo >= 0.8f)
                {
                    animate.SetTrigger("Punch1");
                    maxCombo = 1;
                    timerCombo = 0;
                    playerSpeed = 0f;
                    sound.punch1();
                }
                else if (maxCombo == 1 && timerCombo >= 0.8f)
                {
                    if (!PlayerStatusController.instance.attack) PlayerStatusController.instance.attack = true;
                    animate.SetTrigger("Punch2");
                    maxCombo = 2;
                    timerCombo = 0;
                    playerSpeed = 0f;
                    sound.punch2();
                }

                else if (maxCombo == 2 && timerCombo >= 0.8f)
                {
                    if (!PlayerStatusController.instance.attack) PlayerStatusController.instance.attack = true;
                    animate.SetTrigger("Punch3");
                    maxCombo = 0;
                    timerCombo = 0;
                    comboDuration = 0;
                    playerSpeed = initialPlayerSpeed;
                    sound.punch3();
                }

            }

            // sword
            // sword first time hitting
            if (!weapon_hand)
            {
                if (maxComboSword == 0 && comboDurationSword == 0 && !inCombat)
                {
                    animate.SetTrigger("SwordAttack1");
                    inCombat = true;
                    maxComboSword = 1;
                    playerSpeed = 0f;
                    sound.swordHit1();
                }

                else if (maxComboSword == 1 && inCombat && timerCombo >= 0.8f && comboDurationSword <= 1.9f)
                {
                    if (!PlayerStatusController.instance.attack) PlayerStatusController.instance.attack = true;
                    animate.SetTrigger("SwordAttack2");
                    playerSpeed = initialPlayerSpeed;
                    maxComboSword = 0;
                    timerCombo = 0;
                    sound.swordHit2();
                }


            }
            
        }

        if (comboDuration > 3f || comboDurationSword > 3f) resetInCombat(firstAttack);

    }

    void resetInCombat(bool firstAttack)
    {
        comboDuration = 0;
        comboDurationSword = 0;
        firstAttack = true;
        maxCombo = 0;
        maxComboSword = 0;
        timerCombo = 0;
        inCombat = false;
        playerSpeed = initialPlayerSpeed;
    }

    void checkRoll()
    {


        if(roll)
        {
            timerRoll += Time.deltaTime;
            //mov_dir.x *= playerSpeed + rollSpeed;
            //mov_dir.z *= playerSpeed  + rollSpeed;
            //mov_dir = (Quaternion.Euler(0, (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y), 0) * Vector3.forward).normalized;
            //Debug.Log(timerRoll);
            playerSpeed = rollSpeed;
        }
        if (roll && timerRoll >= 1.2f)
        {
            timerRoll = 0;
            roll = false;
            playerSpeed = initialPlayerSpeed;
        }

        if (Input.GetMouseButtonDown(1) && !roll && timerRoll == 0)
        {
            animate.SetTrigger("RollForward");
            roll = true;
        }

    }

    void playerDeath()
    {
        if (!player.aliveAnimate)
        {
            playerSpeed = 0;
        }
        if (!player.playerAlive)
        {
            SceneManager.LoadScene(3);
        }
    }

}
