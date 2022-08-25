using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float defenseRange = 1;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject bulletSpawnPoint;
    public GameObject BossSpikes;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Fps player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        if (playerInSightRange && playerInAttackRange && Vector3.Distance(player.position, transform.position) <= defenseRange){ Defense(); }
        if (playerInSightRange && playerInAttackRange) { AttackPlayer(); }

    }

    private void Patroling()
    {
        BossSpikes.SetActive(false);
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        BossSpikes.SetActive(false);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        BossSpikes.SetActive(false);
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, bulletSpawnPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);
            rb.AddForce(transform.up * 1f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }


    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Defense() {

     BossSpikes.SetActive(true); BossSpikes.SetActive(false);
    }
}
