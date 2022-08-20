using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallRunTime, maxWallSpeed;
   public  bool isWallRight, isWallLeft;
   public bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    public Transform orientation;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WallRunInput() 
    {
        if (Input.GetKey(KeyCode.D)&& isWallRight) { StartWallRun(); }
        if (Input.GetKey(KeyCode.A)&& isWallLeft) { StartWallRun(); }
    }
    public void StartWallRun() 
    { 
        rb.useGravity = false;
        isWallRunning = true;
        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallRunForce  * Time.deltaTime);
            if (isWallRight)
            {
                rb.AddForce(orientation.right * wallRunForce / 5 * Time.deltaTime);
            }
            else 
            {
                rb.AddForce(-orientation.right * wallRunForce / 5 * Time.deltaTime);
            }
        }
    
    }
    public void StopWallRun() 
    {
        rb.useGravity = true;
        isWallRunning = false;
    }
    public void CheckForWall() 
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        if (!isWallLeft && !isWallRight) { StopWallRun(); }
    
    }
}
