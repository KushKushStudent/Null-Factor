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
    public Text decoysDestroyedUI;
    public Text spawnersDestroyedUI;
    public Text highScoreText;
    public Text currentScoreText;
    public int currentScore;
    public int highScore=0;
    
   
    public bool paused = false;
    public GameObject pauseCanvas;

    public int decoysDestroyed=0;
    public int spawnersDestroyed=0;
    public int decoyPointValue = 40;
    public int spawnerPointValue = 200;
    // Start is called before the first frame update
    void Start()
    
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text="HIGH SCORE: "+ highScore.ToString();
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
        Time.timeScale = 1; 
        pauseCanvas.SetActive(false);
        coinText.text = "0/" +maxCoins;
    }

    // Update is called once per frame
    void Update()
    {
      
        //Debug.Log(decoysDestroyed);
        if (Input.GetKeyDown(KeyCode.Escape)&& paused==false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("paused");
            
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
else if (Input.GetKeyDown(KeyCode.Escape)&& paused==true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
        CalculateScore();

    }
    public void updateCoinUi() 
    { 
        coinText.text=coinsCollected.ToString()+"/"+maxCoins.ToString();
    
    }
    public void updateDecoyUI() 
    { 
        decoysDestroyed++;
        decoysDestroyedUI.text="DECOYS DESTROYED: "+decoysDestroyed.ToString();
        CalculateScore();
       
    }
    public void updateSpawnerUI()
    {
        spawnersDestroyed++;
        spawnersDestroyedUI.text = "SPAWNERS DESTROYED: " + spawnersDestroyed.ToString();
        CalculateScore();
    }
    public void CalculateScore() 
    {
        currentScore = spawnersDestroyed * spawnerPointValue + (decoyPointValue * decoysDestroyed)+(coinsCollected*100);
        currentScoreText.text = "CURRENT SCORE: " + currentScore.ToString();
        if (currentScore >= highScore)
        {
            highScoreText.text = "HIGH SCORE: " + currentScore.ToString();
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
    
}
