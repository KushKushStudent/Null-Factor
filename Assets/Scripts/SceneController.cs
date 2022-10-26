using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int sceneNum=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame() 
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void LoadMenu() 
    {
        SceneManager.LoadScene(0);

    }
    public void LoadCredits() 
    {
        SceneManager.LoadScene(2);
    }
    public void Quit() 
    { 
    Application.Quit();
    }
}
