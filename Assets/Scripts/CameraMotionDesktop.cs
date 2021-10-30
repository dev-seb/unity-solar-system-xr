using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Migration guide to New Input System:
// https://docs.unity3d.com/Packages/com.unity.inputsystem@0.9/manual/Migration.html

public class CameraMotionDesktop : MonoBehaviour {
	      
    public float keyboardSpeed = 5.0f;
    public float mouseSpeed = 0.05f; 
	public float scrollSpeed = 5.0f;
	
    private Vector3 lastPosition = new Vector3();
	
	void Start () 
	{
		//cameraMotionEnabled = (Application.platform == RuntimePlatform.WindowsEditor);
		
		lastPosition = Input.mousePosition;
		
	}	
     
    void Update () 
	{		
		if(SystemInfo.deviceType != DeviceType.Desktop) {
			return;
		}
		
		// Quit game on escape
		if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
		
		// Mouse
		if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            return;
        }
		if(Input.GetMouseButton(0)) {
			lastPosition = Input.mousePosition - lastPosition ;
			lastPosition = new Vector3(-lastPosition.y * mouseSpeed, lastPosition.x * mouseSpeed, 0 );
			lastPosition = new Vector3(transform.eulerAngles.x + lastPosition.x , transform.eulerAngles.y + lastPosition.y, 0);
			transform.eulerAngles = lastPosition;
			lastPosition =  Input.mousePosition;
		}
		
		// Mouse Scroll
		var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheel != 0) {
			transform.Translate(new Vector3(0, 0, 1 * scrollWheel * scrollSpeed));
		}

		// Keyboard
        Vector3 translation = new Vector3();
        if (Input.GetKey (KeyCode.UpArrow)){
            translation += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.DownArrow)){
            translation += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.LeftArrow)){
            translation += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.RightArrow)){
            translation += new Vector3(1, 0, 0);
        }
        translation = translation * keyboardSpeed * Time.deltaTime;
        transform.Translate(translation);
       
    }
}