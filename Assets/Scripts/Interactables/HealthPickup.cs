using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public Vector3 rotation;
  public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation *rotationSpeed* Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player") 
        {
            other.GetComponent<PlayerHealthController>().RestoreHealth(40);
        
            Destroy(gameObject,0.3f);
        }
    }
}
