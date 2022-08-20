
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public GameObject fpsCam;

    //primary fire 
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce=10f;
    public float fireRate=15f;
    public float nextTimeToFire = 0f;

    //secondary fire
    public ParticleSystem secondaryMuzzleFlash;
    public GameObject secondaryImpactEffect;
    public float secondaryImpactForce = 20f;
    public float secondaryFireRate = 1f;
    public float secondaryNextTimeToFire=0f;
    public float secondaryDamage = 30f;

    public AudioSource gunShotAudio;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButton("Fire1")&&Time.time>=nextTimeToFire) 
        {nextTimeToFire=Time.time + 1f / fireRate;
            Shoot();
        }  
        
        if (Input.GetButton("Fire2")&&Time.time>=secondaryNextTimeToFire) 
        {secondaryNextTimeToFire=Time.time + 1f / secondaryFireRate;
            ShootSecondary();
        }
    }

    void Shoot() 
    {
        muzzleFlash.Play();
        gunShotAudio.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) 
        {
          
            TargetController target = hit.transform.GetComponent<TargetController>();
            if (target!=null) 
            {
                target.TakeDamage(damage);
            }
          if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*impactForce);
            }
           GameObject impactGO= Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,2f);
        }
    
    
    
    }void ShootSecondary() 
    {
        secondaryMuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) 
        {
          
            TargetController target = hit.transform.GetComponent<TargetController>();
            if (target!=null) 
            {
                target.TakeDamage(secondaryDamage);
            }
          if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*secondaryImpactForce);
            }
           GameObject impactGO= Instantiate(secondaryImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,2f);
        }
    
    
    
    }
}
