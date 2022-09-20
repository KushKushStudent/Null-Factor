using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public Vector3 startPos;
    [Header("HealthBar")]
    public float health;
    private float lerpTimer;
    public int maxHealth = 100;
    public float chipSpeed;
    public Image frontHealthBar;
    public Image backHealthBar;
 

    [Header("Damage overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    public float durationTimer;

    [Header("DamageNumbers")]
    public float enemyRangedDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {startPos = transform.position; 
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
        if (overlay.color.a>0) 
        {
            durationTimer += Time.deltaTime;
            if (durationTimer>duration) 
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color=new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

    }
    public void UpdateHealthUI() 
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB=backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB>hFraction) 
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer +=Time.deltaTime ;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB,hFraction,percentComplete);
        
        }

        if (fillF<hFraction) 
        { 
        backHealthBar.color=Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF,backHealthBar.fillAmount,percentComplete);
        }
        if (health<=0) 
        {
            SceneManager.LoadScene(1);
        }
    
    }
    public void TakeDamage(float damage) 
    { 
    health -= damage;
        lerpTimer = 0f;
        health = Mathf.Clamp(health, 0, maxHealth);
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f);
        UpdateHealthUI();

    }
    private void OnCollisionEnter(Collision collision)
    {
    


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="FallDeath") 
        {
            TakeDamage(10);
            transform.position = startPos;
        }

        if (other.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage(enemyRangedDamage);

        }
        
        if (other.gameObject.tag == "BossProjectile")
        {
            TakeDamage(30f);

        }
        

        if (other.gameObject.tag == "TouchDamage")
        {
            TakeDamage(2);

        }
    }
    public void RestoreHealth(float healAmount) 
    {
        health += healAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }
}
