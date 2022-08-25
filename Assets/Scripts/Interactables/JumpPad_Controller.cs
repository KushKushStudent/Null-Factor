using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad_Controller : MonoBehaviour
{
    public float jumpMultiplier=1000f;
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
        if (collision.gameObject.tag=="Player") 
        {
            collision.rigidbody.AddForce((Vector3.up * jumpMultiplier));
        
        }
        if (collision.gameObject.layer==5) 
        {
           collision.rigidbody.AddForce((Vector3.up * jumpMultiplier));
        }
    }
}
