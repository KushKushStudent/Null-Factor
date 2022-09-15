using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    public Animator floorAnim;
    public AnimationClip openClip;
    public ButtonController buttonController;
    public SetNewWaypoints setNewWaypoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonController.isActivated == true)
        {
            setNewWaypoints.setNewWaypoints();
            floorAnim.SetBool("isActivated",true);
            Destroy(this.gameObject.GetComponent<FloorMove>());           
        }

    }

   
}
