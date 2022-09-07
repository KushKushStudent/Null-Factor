using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{public NavMeshAgent agent;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public bool attacking;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject bulletSpawnPoint;
    public float bulletForce = 100f;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //cloneDecoyDeplou
    public GameObject decoyPrefab;
    public GameObject decoy1;
    public GameObject decoy2;   
    public GameObject decoy3;
    public float deployTime=0f;
    public float timeBetweenSpawns=10f;
    public bool canDeploy = false;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Fps player").transform;
        agent=GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange=Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) { 
            AttackPlayer();
            

        }

    }

    private void Patroling() 
    { 
        if(!walkPointSet)SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint=transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint() 
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint,-transform.up,2f,whatIsGround)) 
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer() 
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }
    private void AttackPlayer() 
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.position+(2.3f*Vector3.up));

        if (!alreadyAttacked) 
        {
          
            if (Time.time>deployTime+timeBetweenSpawns&&canDeploy==true) 
            {deployTime=Time.time;
                Instantiate(decoyPrefab, decoy1.transform.position, Quaternion.identity);
                Instantiate(decoyPrefab, decoy2.transform.position, Quaternion.identity);
                Instantiate(decoyPrefab, decoy3.transform.position, Quaternion.identity);
            
            }
            Rigidbody rb=Instantiate(projectile,bulletSpawnPoint.transform.position,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletForce , ForceMode.Impulse);
            rb.AddForce(transform.up * 1f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    
    }
    private void ResetAttack() 
    { 
    alreadyAttacked=false;  
    }
}
