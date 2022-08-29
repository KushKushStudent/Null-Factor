using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int coinsCollected = 0;
    public int maxCoins=1;
    public Text coinText;
    public bool paused = false;
    public GameObject pauseCanvas;

    // Start is called before the first frame update
    void Start()
    
    {
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
        Time.timeScale = 1; 
        pauseCanvas.SetActive(false);
        coinText.text = "0/" +maxCoins;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& paused==false) 
        {
            Debug.Log("paused");
            
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
else if (Input.GetKeyDown(KeyCode.Escape)&& paused==true) 
        {
            
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
        if (paused==true && Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(0);
        } 
        if (paused==true && Input.GetKeyDown(KeyCode.Q)) 
        {
            Application.Quit();
        }

        
    }
    public void collectCoin()
    {
        coinsCollected++;
       updateCoinUi();

    }
    public void updateCoinUi() 
    { 
        coinText.text=coinsCollected.ToString()+"/"+maxCoins.ToString();
    
    }
    
}
