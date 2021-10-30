using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
	private bool cameraTrackingEnabled = false;
	private bool cameraTrackingStarted = false;

    void Start()
    {
		//cameraTrackingEnabled = (Application.platform == RuntimePlatform.WindowsEditor);   
    }

    void Update()
    {	
		if(!cameraTrackingEnabled) {
			return;
		}
		if(cameraTrackingStarted) {
			// Change position
			Vector3 cameraPosition = new Vector3(0,0.3f,-0.6f);
			Camera.main.transform.position = gameObject.transform.position + cameraPosition;
			// Change rotation
			Vector3 cameraRotationVector = new Vector3(20, 0, 0);
			Quaternion cameraRotation = Quaternion.Euler(cameraRotationVector);
			Camera.main.transform.rotation = cameraRotation;
			// Exit tracking
			if (Input.GetAxis("Mouse ScrollWheel") != 0
				|| Input.GetKey (KeyCode.UpArrow)
				||Input.GetKey (KeyCode.DownArrow)
				|| Input.GetKey (KeyCode.LeftArrow)
				|| Input.GetKey (KeyCode.RightArrow)
			) {
				toggleCameraTracking();
			}
		}
    }
	
	void OnMouseDown() {
		toggleCameraTracking();
	}
	
	private void toggleCameraTracking() {
		cameraTrackingStarted = !cameraTrackingStarted;	
	}
	
}
