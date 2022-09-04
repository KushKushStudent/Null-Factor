using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwapController : MonoBehaviour
{
    public int gunNumber;
    public GameObject[] guns;
    public GameObject[] gunVisuals;
    // Start is called before the first frame update
    void Start()
    {
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
            gunVisuals[0].SetActive(true);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2)) //gun 2
        {
            DeactivateGunVisuals();
            guns[1].SetActive(true);
            gunVisuals[1].SetActive(true);
        } 
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) //gun 3
        {
            DeactivateGunVisuals();
            guns[2].SetActive(true);
            gunVisuals[2].SetActive(true);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha4)) //gun 4
        {
            DeactivateGunVisuals();
            guns[3].SetActive(true);
            gunVisuals[3].SetActive(true);
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
}
