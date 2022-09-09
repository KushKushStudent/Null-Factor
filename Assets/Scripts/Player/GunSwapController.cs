using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwapController : MonoBehaviour
{
    public int gunNumber;
    public GameObject[] guns;
    public GameObject[] gunVisuals;

    public Animator sniperAnimator;
    private bool isScoped = false;
    public GameObject sniperScopeOverlay;

    public Camera mainCamera;
    public float normalFOV;
    public float sniperScopedFOV=15f;

    // Start is called before the first frame update
    void Start()
    {
        normalFOV = mainCamera.fieldOfView;
        gunNumber = 0;
        
        guns[0].SetActive(true); 
        gunVisuals[0].SetActive(true); 
        guns[1].SetActive(false); 
        guns[2].SetActive(false); 
        guns[3].SetActive(false);
        gunVisuals[1].SetActive(false); 
        gunVisuals [2].SetActive(false); 
        gunVisuals[3].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //gun 1
        {
            DeactivateGunVisuals();
            guns[0].SetActive(true);
            gunNumber = 0;

            gunVisuals[0].SetActive(true);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2)) //gun 2
        {
            DeactivateGunVisuals();
            guns[1].SetActive(true);
            gunVisuals[1].SetActive(true);
            gunNumber = 1;
        } 
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) //gun 3
        {
            DeactivateGunVisuals();
            guns[2].SetActive(true);
            gunVisuals[2].SetActive(true);
            gunNumber = 2;
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha4)) //gun 4
        {
            gunNumber = 3;
            DeactivateGunVisuals();
            guns[3].SetActive(true);
            gunVisuals[3].SetActive(true);
        }

        if (Input.GetButtonDown("Fire2")&& gunNumber==2) 
        {
            isScoped = !isScoped;
          
            sniperAnimator.SetBool("isScoped",isScoped);

       
            StartCoroutine(OnSniperScOped());
        }
        if (Input.GetButtonUp("Fire2") && gunNumber == 2)
        {
            isScoped = !isScoped;
            sniperAnimator.SetBool("isScoped", isScoped);
            OnSniperUnScOped();

        }
    }
    public void DeactivateGunVisuals() 
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
            gunVisuals[i].SetActive(false);

        }

    }
    IEnumerator OnSniperScOped() 
    {
        yield return new WaitForSeconds(.15f);
        sniperScopeOverlay.SetActive(true);
        mainCamera.fieldOfView = sniperScopedFOV;
        gunVisuals[gunNumber].SetActive(false);

    
    }
    void OnSniperUnScOped() 
    {
        mainCamera.fieldOfView = normalFOV;
        sniperScopeOverlay.SetActive(false);
        gunVisuals[gunNumber].SetActive(true);

    }
    public void SniperScope() { }
}
