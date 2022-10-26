using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadeBullet : MonoBehaviour
{
    [Header("GroundSlam")]
    public bool groundSlamExplosion = false;
    public float groundSlamRadius = 10f;
    public float groundSlamDamage = 30f;
    public LayerMask enemies;
    public ParticleSystem groundSlamEffect;
    public bool isGrenade=true;
    public bool isSlowGrenade;
    public bool isCollapsingGrenade;
    Collider[] enemyArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
       enemyArray = Physics.OverlapSphere(gameObject.transform.position, groundSlamRadius, enemies);

        if (enemyArray != null)
        {
            for (int i = 0; i < enemyArray.Length; i++)
            {

                enemyArray[i].gameObject.GetComponent<TargetController>().TakeDamage(groundSlamDamage);

                
            }
            if (isGrenade)
            {
                Grenade();
            }
            else if (isSlowGrenade) 
            { 
            SlowGrenade();
            }
            else if (isCollapsingGrenade)
            { 
            CollapsingStar();
            } 
            else 
            { }
           
        }
    }
    void Grenade() {
    ParticleSystem effect = Instantiate(groundSlamEffect, gameObject.transform.position - new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(effect, 2);
            Destroy(gameObject, 0);
    }
    void SlowGrenade()
    {
        ParticleSystem effect = Instantiate(groundSlamEffect, gameObject.transform.position - new Vector3(0, 1, 0), Quaternion.identity);
        for (int i = 0; i < enemyArray.Length; i++)
        {

            enemyArray[i].gameObject.GetComponent<TargetController>().TakeDamage(groundSlamDamage);
            enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed= enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed / 3;
           

        }
        StartCoroutine(slowDamage());

    }
    void CollapsingStar() 
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {

            enemyArray[i].gameObject.GetComponent<TargetController>().TakeDamage(groundSlamDamage);
            enemyArray[i].gameObject.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
            enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed = enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed / 3;


        }


    }
    IEnumerator slowDamage() 
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < enemyArray.Length; i++)
        {

            enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed = enemyArray[i].gameObject.GetComponent<NavMeshAgent>().speed *3;


        }
       

    }

}
