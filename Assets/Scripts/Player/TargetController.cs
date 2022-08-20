
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 100f;

    public float maxHealth=100f;
    public GameObject hpCanvas;
   
    public Slider slider;
    private Camera cam;

 
    private void Start()
    {cam=Camera.main;
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
        Destroy(transform.gameObject);
    }
    float CalculateHealth() 
    {

        return health / maxHealth;
    }
}
