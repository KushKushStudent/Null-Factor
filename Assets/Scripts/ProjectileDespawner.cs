using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDespawner : MonoBehaviour
{
    public float despawnTime;
    public int bounces=0;
    public int currentBounces = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawnProjectile());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBounces > bounces)
        {
            Destroy(gameObject, 0f);
        }

    }
    IEnumerator despawnProjectile() 
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
        
    }
    private void OnCollisionEnter(Collision collision)
    {   if(collision.gameObject.tag!="Enemy")
            currentBounces++;
    else Destroy(gameObject);
       
    }
}
