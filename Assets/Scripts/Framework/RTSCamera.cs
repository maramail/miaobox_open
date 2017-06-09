using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

    public int edgePixel = 10;
    public int moveSpeed = 15;

    private float xpos;
    private float ypos;

    private Vector3 movement = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void LateUpdate() {
        //return  ;
        movement = new Vector3(0, 0, 0);
        xpos = Input.mousePosition.x;
        ypos = Input.mousePosition.y;

        if (xpos >= 0 && xpos < edgePixel)
        {
                movement.x -= moveSpeed * Time.deltaTime;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - edgePixel)
        {
            movement.x += moveSpeed * Time.deltaTime;
        }

        if (ypos >= 0 && ypos < edgePixel)
        {
                movement.z -= moveSpeed * Time.deltaTime;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - edgePixel)
        {
                movement.z += moveSpeed * Time.deltaTime;
        }

        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        gameObject.transform.position += movement;
    }
}
