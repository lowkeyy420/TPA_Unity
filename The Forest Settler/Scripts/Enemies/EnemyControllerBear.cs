using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerBear : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent obj;

    [SerializeField]
    private Animator eAnimator;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float detectionRange = 15f, hitRange = 3f;

    [SerializeField]
    private Transform monster; 

    public List<Transform> waypoints;

    private float timer = 0f;

    [SerializeField]
    private float delay;

    private bool hit1, hit2, hit3, lastDelay;
    public static bool inCombat;

    private Bear bear;


    void Start()
    {
        obj = GetComponent<NavMeshAgent>();
        timer = 0f;
        hit1 = false; hit2 = false; hit3 = false; lastDelay = false;
        bear = GetComponent<Bear>();
        inCombat = false;
        obj.speed = 1.5f;
        obj.SetDestination(waypoints[Random.Range(1,waypoints.Count)].position);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(monster.position, player.position);

        //patrol lol
        if(obj.remainingDistance <= 1f && distance > detectionRange)
        {
            obj.speed = 1.5f;
            obj.SetDestination(waypoints[Random.Range(1, waypoints.Count)].position);
        }

        checkEnemyMoving();

        if (distance < detectionRange && bear.isAlive)
        {
            inCombat = true;
            obj.speed = 3.5f;
            obj.SetDestination(player.position);
            bear = obj.GetComponent<Bear>();
            if (bear.isAlive == false) eAnimator.SetBool("Idle", false);
            if (distance <= hitRange)
            {
                obj.isStopped = true;
                eAnimator.SetBool("Run Forward", false);
                if(inCombat)
                {
                    // First time meeting
                    if (timer <= 0.8f && !hit1 && !lastDelay)
                    {
                        timer += Time.deltaTime;
                        if (!hit1 && timer >= 0.8)
                        {
                            eAnimator.SetTrigger("Attack1");
                            hit1 = true;
                            timer = 0;
                            bear.attack();
                        }
                    }

                    // end of cycle, back to first hit with delay
                    else if (timer <= delay && !hit1 && lastDelay)
                    {
                        timer += Time.deltaTime;
                        if (!hit1 && timer >= delay)
                        {
                            eAnimator.SetTrigger("Attack1");
                            hit1 = true;
                            timer = 0;
                            bear.attack();
                        }
                    } 
                    // 2nd hit
                    else if(timer <= delay && hit1 && !hit2)
                    {
                        timer += Time.deltaTime;
                        if (timer >= delay)
                        {
                            eAnimator.SetTrigger("Attack2");
                            hit2 = true;
                            timer = 0;
                            bear.attack();
                        }
                    }
                    // 3rd hit
                    else if (timer <= delay && hit1 && hit2 && !hit3)
                    {
                        timer += Time.deltaTime;
                        if (timer >= delay)
                        {
                            eAnimator.SetTrigger("Attack3");
                            hit3 = true;
                            lastDelay = false;
                            timer = 0;
                            bear.attack();
                        }
                    }
                    // last hit
                    else if (timer <= delay && hit1 && hit2 && hit3 && !lastDelay)
                    {
                        timer += Time.deltaTime;
                        if (timer >= delay)
                        {
                            eAnimator.SetTrigger("Attack5");
                            lastDelay = true;
                            timer = 0;
                            hit1 = false; hit2 = false; hit3 = false;
                            bear.attack();
                        }
                    }

                }
            }
            // if out of range
            else if (distance > hitRange)
            {
                obj.isStopped = false;
                eAnimator.ResetTrigger("Attack1");
                eAnimator.ResetTrigger("Attack2");
                eAnimator.ResetTrigger("Attack3");
                eAnimator.ResetTrigger("Attack5");
                timer = 0;
                hit1 = false; hit2 = false; hit3 = false; lastDelay = false;
                inCombat = false;
            }
        }

    }

    void checkEnemyMoving()
    {
        //Debug.Log(obj.velocity.magnitude);
        Debug.Log(obj.velocity.magnitude );
        if (bear.isAlive)
        {
            if (obj.velocity.magnitude >= 1f && obj.velocity.magnitude < 3f) {
                eAnimator.SetBool("WalkForward", true);
                eAnimator.SetBool("Run Forward", false);
                eAnimator.SetBool("Idle", false);
            }
            else if (obj.velocity.magnitude > 3f && bear.isAlive ) { 
                eAnimator.SetBool("Run Forward", true);
                eAnimator.SetBool("WalkForward", false); 
                eAnimator.SetBool("Idle", false);
            }
            else if (obj.velocity.magnitude < 0.15f) 
            {

                eAnimator.SetBool("WalkForward", false);
                eAnimator.SetBool("Run Forward", false);
                eAnimator.SetBool("Idle", true);
            }

        }
        if (!bear.isAlive)
        {
            eAnimator.SetBool("WalkForward", false);
            eAnimator.SetBool("Run Forward", false);
            eAnimator.SetBool("Idle", false);
        }
    }

}
