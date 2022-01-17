using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 


public class EdgeScrolling : MonoBehaviour
{
    public float panSpeed = 300f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minY = 10f;   
    public float maxY = 80f;
    public Vector3 panLimit;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        
        // checks a button press or if mouse hits a border. Each if is for a different direction
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        pos.z -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);
        pos.z = Mathf.Clamp(pos.z, -panLimit.z, panLimit.z);
        
        transform.position = pos;
                
    }
}