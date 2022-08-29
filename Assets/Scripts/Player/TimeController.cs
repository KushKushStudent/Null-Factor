
using UnityEngine;

public class TimeController 
{
    // Start is called before the first frame update

    public Vector3 position;
    public Quaternion rotation;
   
    public TimeController(Vector3 _position,Quaternion _rotation) 
    { 
        position = _position; 
        rotation = _rotation; 
    
    }
}
