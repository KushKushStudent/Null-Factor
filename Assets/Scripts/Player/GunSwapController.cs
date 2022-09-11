using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSwapController : MonoBehaviour
{
    public int gunNumber;
    public GameObject normalCrosshair;

    public GameObject[] guns;
    public GameObject[] gunVisuals;
    public bool isScoped = false;

    public Animator aRAnimator;
    public Animator shotgunAnimator;
    public Animator GrenadeAnimator;


    public Animator sniperAnimator; public GameObject sniperScopeOverlay;

    public GameObject shotgunScopeOverlay;
  
    public GameObject aRScopeOverlay;
    public GameObject GrenadescopeOverlay;
   

    public Camera mainCamera;
    public float normalFOV;
    public float sniperScopedFOV=15f;
    public float aDSFOV = 30f;

    public Image sniperSil;
    public Image shotgunSil;
    public Image arSil;  
    public Image nadeSil;

    // Start is called before the first frame update
    void Start()
    {
        normalCrosshair.SetActive(true) ;
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
            isScoped = false;
            DeactivateGunVisuals();
            guns[0].SetActive(true);
            gunNumber = 0;

            gunVisuals[0].SetActive(true);
            switchSil(arSil);

        } 
        if (Input.GetKeyDown(KeyCode.Alpha2)) //gun 2
        {
            isScoped = false;
            DeactivateGunVisuals();
            guns[1].SetActive(true);
            gunVisuals[1].SetActive(true);
            gunNumber = 1;
            switchSil(shotgunSil);
        } 
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) //gun 3
        {
            isScoped = false;
            DeactivateGunVisuals();
            guns[2].SetActive(true);
            gunVisuals[2].SetActive(true);
            gunNumber = 2;
            switchSil(sniperSil);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha4)) //gun 4
        {
            isScoped = false;
            gunNumber = 3;
            DeactivateGunVisuals();
            guns[3].SetActive(true);
            gunVisuals[3].SetActive(true);
            switchSil(nadeSil);
        }

        if (Input.GetButtonDown("Fire2")&& gunNumber==2) 
        {
            isScoped = !isScoped;
          
            sniperAnimator.SetBool("isScoped",isScoped);

            normalCrosshair.SetActive(false);
            StartCoroutine(OnSniperScOped());
        }


        if (Input.GetButtonDown("Fire2") && gunNumber == 0)
        {
            isScoped = !isScoped;

            aRAnimator.SetBool("isScoped", isScoped);

            normalCrosshair.SetActive(false);
            StartCoroutine(OnOtherScoped(aRScopeOverlay));
        }


        if (Input.GetButtonDown("Fire2") && gunNumber == 1)
        {
            isScoped = !isScoped;

            shotgunAnimator.SetBool("isScoped", isScoped);
            normalCrosshair.SetActive(false);

            StartCoroutine(OnOtherScoped(shotgunScopeOverlay));
        }


        if (Input.GetButtonDown("Fire2") && gunNumber == 3)
        {
            isScoped = !isScoped;

           GrenadeAnimator.SetBool("isScoped", isScoped);

            normalCrosshair.SetActive(false);
            StartCoroutine(OnOtherScoped(GrenadescopeOverlay));
        }



        if (Input.GetButtonUp("Fire2") && gunNumber == 0)
        {
            isScoped = false;
            aRAnimator.SetBool("isScoped", isScoped);
            OnOtherUnScoped(aRScopeOverlay);
            normalCrosshair.SetActive(true);
        }
        if (Input.GetButtonUp("Fire2") && gunNumber ==1)
        {
            isScoped = false;
           shotgunAnimator.SetBool("isScoped", isScoped);
            OnOtherUnScoped(shotgunScopeOverlay);
            normalCrosshair.SetActive(true);

        } 
        if (Input.GetButtonUp("Fire2") && gunNumber ==3)
        {
            isScoped = false;
            GrenadeAnimator.SetBool("isScoped", isScoped);
            OnOtherUnScoped(GrenadescopeOverlay);
            normalCrosshair.SetActive(true);

        } 
        
        if (Input.GetButtonUp("Fire2") && gunNumber == 2)
        {
            isScoped = false;
            sniperAnimator.SetBool("isScoped", isScoped);
            OnSniperUnScOped();
            normalCrosshair.SetActive(true);
        }
    }
    public void DeactivateGunVisuals() 
    {
        isScoped = false;
         sniperScopeOverlay.SetActive(false);

    shotgunScopeOverlay.SetActive(false);

        aRScopeOverlay.SetActive(false);
        GrenadescopeOverlay.SetActive(false);


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
    IEnumerator OnOtherScoped(GameObject overlay) 
    {
        yield return new WaitForSeconds(.15f);
        overlay.SetActive(true);
        mainCamera.fieldOfView = aDSFOV;
        //gunVisuals[gunNumber].SetActive(false);

    
    }
    void OnOtherUnScoped(GameObject overlay) 
    {
        mainCamera.fieldOfView = normalFOV;
        overlay.SetActive(false);
     //   gunVisuals[gunNumber].SetActive(true);

    }
    public void SniperScope() { }
    public void switchSil(Image thisImage) 
    { 
        arSil.gameObject.SetActive(false);
        sniperSil.gameObject.SetActive(false);
        shotgunSil.gameObject.SetActive(false);
        thisImage.gameObject.SetActive(true);

    
    }
}
