using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool isActivated=false;
    public Material activatedColour;
    public Material disabledColour;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = disabledColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isActivated==false &&collision.transform.tag=="Player") 
        {
            isActivated = true;
            gameObject.GetComponent<MeshRenderer>().material = activatedColour;
        
        }
    }
    
}
