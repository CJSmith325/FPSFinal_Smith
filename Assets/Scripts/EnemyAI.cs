using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

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

    private void Awake()
    {
        player = GameObject.Find("FirstPersonPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
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
            
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            Chasing();
            
        }

        if (playerInSightRange && playerInAttackRange)
        {
            Attacking();
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
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // attack code inserted here


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
}
