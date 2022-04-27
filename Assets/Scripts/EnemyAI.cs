using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    private HealthBar hb;

    private CameraShake cameraShake;

    private float playHealth;

    public NavMeshAgent agent;

    private Transform player;

    public LayerMask groundMask, playerMask;

    //idle variables
    [Header("Idle")]
    public float idleTime = 1;
    private float idleCount = 0;
    //patrol varaibles
    [Header("Patrol")]
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    [Header("Attack")]
    // attack variables
    public float timeBetweenAttack;
    bool alreadyAttacked;
    [Header("General")]
    // state determiners
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private Animator animator;
    public float runSpeed = 2f;
    public float health = 50;
    private AudioClip deathSound;
    private AudioSource audioSource;

    private void Awake()
    {
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        hb = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        player = GameObject.Find("FirstPersonPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        deathSound = GetComponent<AudioClip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y > 2)
        {
            if (this.gameObject.GetComponent<Rigidbody>() == true)
            {
                Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
                rb.MovePosition(new Vector3(0, -1, 0));
            }
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Idling();
            animator.SetBool("attackRange", false);
            animator.SetBool("sightRange", false);
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            Chasing();
            animator.SetBool("attackRange", false);
            animator.SetBool("sightRange", true);
        }

        if (playerInSightRange && playerInAttackRange)
        {
            Attacking();
            animator.SetBool("attackRange", true);
            animator.SetBool("sightRange", true);
        }
    }

    void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void Chasing()
    {
        agent.speed = 5.4f;
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // attack code inserted here
            animator.Play("RightHand@Attack01", -1, 0f);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    
    void Idling()
    {
        agent.speed = 3.5f;
        if (idleCount >= idleTime)
        {
            Patrolling();
            idleCount = 0;
        }
        else
        {
            idleCount += Time.deltaTime;
        }
    }

    public void DoDamage()
    {
        PlayerHealth.playerHealth -= 10;
        StartCoroutine(cameraShake.Shake(0.25f, 1.2f));
        hb.SetHealth();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            
            audioSource.Play();
            Die();
        }
    }

    void Die()
    {
        PlayDeathSound();
        Destroy(gameObject, 0.75f);
        
    }

    void PlayDeathSound()
    {

        //If you need to change the volume
        //audioSource.volume = volume;
        //Play the sound once
        audioSource.PlayOneShot(deathSound);



    }
}
