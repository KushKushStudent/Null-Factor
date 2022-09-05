
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

   public GameManager gm;
    [Header("Assignables")]
    public Transform playerCam;
    public Transform orientation;
    
    //Other
    public Rigidbody rb;

    [Header("Look and rotation")]
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    [Header("Movement")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [Header("Crouch and Slider")]
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    public float crouchSlamForce = 1000f;
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    [Header("Jumping")]
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    
    [Header("Input")]
    float x, y;
    bool jumping, sprinting, crouching;

    
    public WallRun wallRun;

    [Header("Sliding")]
    
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;


    [Header ("GroundSlam")]
    public bool groundSlamExplosion=false;
    public float groundSlamRadius = 10f;
    public float groundSlamDamage = 30f;
    public LayerMask enemies;
    public ParticleSystem groundSlamEffect;

    [Header("TimeRewind")]
    public bool isRewinding = false;
    List<TimeController> pointsInTime;
    public int timeToRewind = 5;

    public int updateCounter = 0;
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
        pointsInTime = new List<TimeController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void FixedUpdate() {
        Movement();
        if (isRewinding)
        {
            Rewind();
        }
        else { Record(); }

    }

    private void Update() {
        MyInput();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopRewind();
        }
        Look();
        wallRun.CheckForWall();
        wallRun.WallRunInput();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);
      
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
         
        }
        if (!grounded) {
            groundSlamExplosion =true;
            rb.AddForce(Vector3.down * crouchSlamForce);
            Debug.Log(groundSlamExplosion); ; 
        }
    }

    private void StopCrouch() {
        groundSlamExplosion = false;
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement() {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);
        
        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;
        
        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        // Movement in air
        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
           /* if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
           */
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (wallRun.isWallRunning)
        {
            readyToJump = false;
            if (wallRun.isWallLeft && !Input.GetKey(KeyCode.D) || wallRun.isWallRight && !Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);

            }
            if (wallRun.isWallRight || wallRun.isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) rb.AddForce(-orientation.up * jumpForce * 1f);
            if (wallRun.isWallRight && Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * jumpForce * 3f); 
            if (wallRun.isWallLeft && Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * jumpForce * 3f);

            rb.AddForce(orientation.forward * jumpForce * 1f);
          //  rb.velocity = Vector3.zero;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
    
    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRun.wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        if (Math.Abs(wallRun.wallRunCameraTilt)<wallRun.maxWallRunCameraTilt&&wallRun.isWallRunning&&wallRun.isWallRight) 
        {
            wallRun.wallRunCameraTilt += Time.deltaTime * wallRun.maxWallRunCameraTilt * 2;
        }   
        if (Math.Abs(wallRun.wallRunCameraTilt)<wallRun.maxWallRunCameraTilt&&wallRun.isWallRunning&&wallRun.isWallLeft) 
        {
            wallRun.wallRunCameraTilt -= Time.deltaTime * wallRun.maxWallRunCameraTilt * 2;
        }

        if (wallRun.wallRunCameraTilt>0 &&!wallRun.isWallRight && !wallRun.isWallLeft) 
        { 
            wallRun.wallRunCameraTilt-=Time.deltaTime * wallRun.maxWallRunCameraTilt * 2;
        }  
        if (wallRun.wallRunCameraTilt<0 &&!wallRun.isWallRight && !wallRun.isWallLeft) 
        { 
            wallRun.wallRunCameraTilt+=Time.deltaTime * wallRun.maxWallRunCameraTilt * 2;
        }
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
   
    private void OnCollisionStay(Collision other) {
       
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

      
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                
                
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }

    public void GroundSlam() 
    {
        Collider[] enemyArray = Physics.OverlapSphere(rb.position,groundSlamRadius,enemies );

        if (enemyArray!=null)
        {
            for (int i = 0; i < enemyArray.Length; i++)
            {
              
                enemyArray[i].gameObject.GetComponent<TargetController>().TakeDamage(groundSlamDamage);
               
                Debug.Log(enemyArray[i].gameObject.name);
            }
          ParticleSystem effect= Instantiate(groundSlamEffect, rb.transform.position-new Vector3(0,1,0), Quaternion.identity);
            Destroy(effect,3);
            groundSlamExplosion = false;
        }

       
           
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
   
         Gizmos.DrawWireSphere(rb.position, groundSlamRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        Debug.Log(groundSlamExplosion);

        if (collision.gameObject.layer==3 && groundSlamExplosion==true)
        {
            GroundSlam();
            
        }   
    }
    void StartRewind()
    {

        isRewinding = true;
        rb.isKinematic = true;
    }
    void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }
    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(timeToRewind / Time.fixedDeltaTime / 3))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);

        }
        if (updateCounter >= 3)
        {
            pointsInTime.Insert(0, new TimeController(transform.position, transform.rotation));
            updateCounter = 0;
        }
        else { updateCounter++; }
    }
    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            TimeController pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else { StopRewind(); }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            gm.collectCoin();

        }
    }

}
