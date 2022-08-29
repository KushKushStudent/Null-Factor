using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject [] switches;
    public int switchCheck = 0;
    public bool checkSwitch = false;
    public Animator switchAnimator;
    public AnimationClip doorAnimationOpen;


    public float verticalMoveDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkSwitch==false) { checkSwitches(); }
      
    }

    void checkSwitches() 
    {
        for (int i=0; i<switches.Length;i++) 
        {
            if (switches[i].GetComponent<ButtonController>().isActivated == true) 
            {
                checkSwitch = true;

            } 
            else 
            {checkSwitch = false;
                break;
            }
        }

        if (checkSwitch) 
        {
            Activate();
        }
    }
    void Activate()
    {
        switchAnimator.Play(doorAnimationOpen.name);
        this.gameObject.GetComponent<SetNewWaypoints>().setNewWaypoints();
       

        for (int i=0;i<switches.Length;i++) 
        {
            Destroy(switches[i].transform.parent.gameObject);
        }
       
        /* transform.Translate(Vector3.down*verticalMoveDistance);
         for (int i = 0; i < switches.Length; i++)
         {
             switches[i].GetComponent<ButtonController>().GetComponent<ButtonController>().enabled = false;
            }
     */
    }
}
