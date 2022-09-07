using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShieldController : MonoBehaviour
{
    public GameObject shield;
    public bool shieldActivated=false;
    public float shieldRadius;
    public bool recharging = false;
    public LayerMask enemyProjectiles;
    public Material shieldMat;
    public GameObject shieldUI;
    public Text shieldText; 

    public Color projectileColour;
    // Start is called before the first frame update
    void Start()
    {
        shield.SetActive(false);
        shieldUI.SetActive(true);
        shieldText.text = "F";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&shieldActivated==false&&recharging==false)
        {
           shield.SetActive(true);
            shieldActivated = true;
            StartCoroutine(deactivateShield());
        }
        if (shieldActivated==true) 
        { 
        DeflectShield();
        }
    }
    IEnumerator deactivateShield()
    {
        shieldUI.SetActive(false);
        shieldText.text = "Active";
        Debug.Log("shield activated");
        yield return new WaitForSeconds(5f);
       
        
        StartCoroutine(recharge());
    }
    IEnumerator recharge()
    {
        shieldText.text = "Recharging";
        shieldActivated = false;
        recharging = true;
        shield.SetActive(false);
        Debug.Log("shield deactivated");
        yield return new WaitForSeconds(10f);
        shieldUI.SetActive(true);
        recharging = false;
        shieldText.text = "F";
    }
    public void DeflectShield() 
    {
        Collider[] enemyBulletArray = Physics.OverlapSphere(gameObject.transform.position, shieldRadius, enemyProjectiles);
        for (int i = 0; i < enemyBulletArray.Length; i++)
        {
            enemyBulletArray[i].gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            enemyBulletArray[i].gameObject.GetComponent<Rigidbody>().AddForce(-enemyBulletArray[i].gameObject.GetComponent<Rigidbody>().velocity);
            enemyBulletArray[i].GetComponent<MeshRenderer>().material = shieldMat; ;
            enemyBulletArray[i].GetComponent<TrailRenderer>().endColor = Color.green;
            enemyBulletArray[i].GetComponent<TrailRenderer>().colorGradient.SetKeys(new GradientColorKey[] {new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.green, 1f)}, new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }); 
            enemyBulletArray[i].GetComponent<TrailRenderer>().endColor = Color.green;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(gameObject.transform.position, shieldRadius);
    }
}
