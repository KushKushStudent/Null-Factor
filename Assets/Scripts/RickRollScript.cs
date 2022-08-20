using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RickRollScript : MonoBehaviour
{
    public VideoClip RickRollVideo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<VideoPlayer>()==null)
        {
            collision.gameObject.AddComponent<VideoPlayer>();
            collision.gameObject.GetComponent<VideoPlayer>().clip = RickRollVideo;
        }
       
       
    }
}
