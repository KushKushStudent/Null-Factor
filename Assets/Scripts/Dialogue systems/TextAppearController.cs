using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class TextAppearController : MonoBehaviour
{
    public string[] dialogue;
    public TextMeshPro dialogueBox;
    public int counter = 0;
    private bool isActivated=false;

    public float typingSpeed=0.04f;
    public float timeBetweenLines=3f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.text = "";
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ContinueStory() 
    {
        StartCoroutine(DisplayLine(dialogue[counter]));
    }
    public IEnumerator DisplayLine(string line) 
    {
        dialogueBox.text = "";
        foreach (char letter in line.ToCharArray()) 
        {
            dialogueBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        counter++;
        yield return new WaitForSeconds(timeBetweenLines);
       
        if (counter>dialogue.Length-1)
        {
            animator.Play("Fade");
        }
        else { ContinueStory(); }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated==false&& other.tag=="Player") 
        { 
            isActivated=true;
            ContinueStory();
        }
    }
}
