
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class TargetController : MonoBehaviour
{
    public ParticleSystem explosionFX;
    public AudioSource enemyAudio;
    public AudioClip deathAudio;
    public bool isDecoy;
    // Start is called before the first frame update
    public float health = 100f;

    public float maxHealth=100f;
    public GameObject hpCanvas;
   
    public Slider slider;
    private Camera cam;
    public ParticleSystem tempExplosion;

    public GameManager gameManager;
 
    private void Start()
    {cam=Camera.main;
        gameManager=FindObjectOfType<GameManager>();
        slider.value = CalculateHealth();
       maxHealth=health;
        
    }
    private void Update()
    {
        hpCanvas.transform.rotation = Quaternion.LookRotation(transform.position-cam.transform.position);
        slider.value = CalculateHealth();
        if (health<maxHealth) 
        { 
        hpCanvas.SetActive(true);
        
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile") 
        {
            TakeDamage(collision.gameObject.GetComponent<BulletDamageController>().bulletDamage);
        }
    }
    public void TakeDamage(float amt) 
    {
        health -= amt;
       
        if (health<=0f) 
        {
            Die();
        }
    }
    
   
    void Die() 
    {
     
       tempExplosion =Instantiate(explosionFX, transform.position,Quaternion.identity);
        enemyAudio.clip = deathAudio;
        enemyAudio.Play();

        StartCoroutine(ExplosionDeath());
   
    }
    float CalculateHealth() 
    {

        return health / maxHealth;
    }

    IEnumerator ExplosionDeath()
    {
        yield return new WaitForSeconds(0.3f);
        if (isDecoy == true)
        {
            gameManager.updateDecoyUI();


        }
        else
        {
            gameManager.updateSpawnerUI();
        }
        Destroy(tempExplosion);
        Destroy(transform.gameObject);

    }

}
