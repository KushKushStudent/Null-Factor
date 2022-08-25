using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDespawner : MonoBehaviour
{
    public float despawnTime;
    public int bounces=2;
    public int currentBounces = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawnProjectile());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator despawnProjectile() 
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
        
    }
    private void OnCollisionEnter(Collision collision)
    {   currentBounces++;
        if (currentBounces>=bounces) 
        {
            Destroy(gameObject, 1f);
        }
        
    }
}
