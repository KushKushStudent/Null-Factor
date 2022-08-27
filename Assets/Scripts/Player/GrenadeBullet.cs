using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour
{
    [Header("GroundSlam")]
    public bool groundSlamExplosion = false;
    public float groundSlamRadius = 10f;
    public float groundSlamDamage = 30f;
    public LayerMask enemies;
    public ParticleSystem groundSlamEffect;
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
        Collider[] enemyArray = Physics.OverlapSphere(gameObject.transform.position, groundSlamRadius, enemies);

        if (enemyArray != null)
        {
            for (int i = 0; i < enemyArray.Length; i++)
            {

                enemyArray[i].gameObject.GetComponent<TargetController>().TakeDamage(groundSlamDamage);

                
            }
            ParticleSystem effect = Instantiate(groundSlamEffect, gameObject.transform.position - new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(effect, 2);
            Destroy(gameObject, 0);
           
        }
    }

}
