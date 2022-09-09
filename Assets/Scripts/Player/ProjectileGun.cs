using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    public GameObject bullet;
    
    [Header ("Bullet Variables") ]
    public float bulletShootForce, upwardBulletForce;

    [Header("Gun Stats")]
    public float timeBetweenShots,spread, reload, reloadTime, timeBetweenShooting;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    [Header("Gun state Booleans")]
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera playerCamera;
    public Transform attackPoint;

    [Header("bug fixes")]
    public bool allowInvoke = true;

    [Header("Graphics")]
  
    public TextMeshProUGUI ammunitionDisplay;

    public GameObject player;



    // Start is called before the first frame update
    private void Awake()
    {
        bulletsLeft = magazineSize;
        
        readyToShoot = true;
       // player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        bulletsLeft = magazineSize;

        readyToShoot = true;
      //  player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        
    }
    void Update()
    {

        PlayerInput();

        if (ammunitionDisplay != null)
        {
            if (reloading) { ammunitionDisplay.SetText("Reloading..."); } else { ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap); }

        }
    }
    private void PlayerInput() 
    {
        if (allowButtonHold)  //Checks if weapon is single fire or auto
        {

            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyUp(KeyCode.Mouse0 )) { }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload(); 
        //reloading


        if (readyToShoot && shooting &&!reloading && bulletsLeft<=0) Reload();        //reload if firing once empty


        if (readyToShoot && shooting && !reloading &&bulletsLeft>0)  //checks if there are bullets before firing
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot() 
    {
        readyToShoot = false;

       Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.6f, 0.5f, 0)); 
      //  Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.2f, 0.1f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else 
        {
            targetPoint = ray.GetPoint(300);

        }

        Vector3 directionWithoutSpeed=targetPoint - attackPoint.position;

        //bullet spread variables
        float x = Random.Range(-spread,spread);
        float y= Random.Range(-spread,spread);


        Vector3 directionWithSpread = directionWithoutSpeed + new Vector3(x, y, 0);

        GameObject ?currentBullet=Instantiate(bullet,attackPoint.position,Quaternion.identity);

        currentBullet.transform.forward = directionWithSpread.normalized+ player.GetComponent<Rigidbody>().velocity;

        currentBullet?.GetComponent<Rigidbody>().AddForce( directionWithSpread.normalized * bulletShootForce,ForceMode.Impulse);
        currentBullet?.GetComponent<Rigidbody>().AddForce(playerCamera.transform.up *upwardBulletForce,ForceMode.Impulse); //only set bullet upward force  >0 for grenades or arcing projectiles

    
        
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke) //for single projectile fire weapons
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

        }

        if (bulletsShot<bulletsPerTap &&bulletsLeft>0) //multiple projectiles fired per click, such as for shotguns
        {
            Invoke("Shoot", timeBetweenShots);

        }


    }

    private void ResetShot() 
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        ammunitionDisplay.SetText("Reloading...");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished() 
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
