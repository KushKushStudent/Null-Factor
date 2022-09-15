using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalObjectiveController : MonoBehaviour
{
   

    public Image[] imgs;

    public Transform[] targets;

    public WaypointController wp;

    public Text[] meters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        setNewWaypoints();
    }
    public void setNewWaypoints()
    {
        wp.ResetUI();
        wp.changeWaypoints(imgs, targets, meters);

    }

}
