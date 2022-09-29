using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointController : MonoBehaviour
{
    
    public Image []imgs;  
    public Image []allImgs;

    public Vector3 offset;
    public Transform [] targets;
    
    public Text[] meters;  
    public Text[] allMeters;

    Vector3 pos;
    public Canvas waypointCanvas;
    Image[] imgarray;


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < allImgs.Length; i++)

        {
           allImgs[i].enabled = false;
            allMeters[i].enabled = false;
        }
        for (int i = 0; i < imgs.Length; i++)

        {
            imgs[i].enabled = true;
            meters[i].enabled = true;
        }

       

    }

    // Update is called once per frame
    void Update()
    {

        

   
        for (int i = 0; i < targets.Length; i++)
        {
            
            
            float minX = imgs[i].GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = imgs[i].GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;

            pos = Camera.main.WorldToScreenPoint(targets[i].position + offset);
           
            if (Vector3.Dot((targets[i].position - transform.position), transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;

                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            imgs[i].transform.position = pos;
            meters[i].text = Mathf.Round(Vector3.Distance(targets[i].position, transform.position)).ToString() + "m";

            if (Mathf.Round(Vector3.Distance(targets[i].position, transform.position))<5)
            {
                imgs[i].enabled = false;
                meters[i].enabled = false;
            }
        }


       
    }


    public void changeWaypoints(Image []newImgs,Transform [] newTargets,Text [] newMeters) 
    {
        imgs = new Image [newImgs.Length];
        imgs = newImgs;

      targets= newTargets;
        meters = newMeters;
        ResetUI();
        for (int i = 0; i < newImgs.Length; i++)

        {
            imgs[i].gameObject.SetActive(true);
            meters[i].gameObject.SetActive(true);
        }
    }

    public void ResetUI() 
    {

        for (int i = 0; i < allImgs.Length; i++)

        {
            
            allImgs[i].gameObject.SetActive(false);
            allMeters[i].gameObject.SetActive( false);
        }
    }
}

