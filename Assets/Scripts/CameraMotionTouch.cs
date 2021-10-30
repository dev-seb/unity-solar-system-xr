using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotionTouch : MonoBehaviour {
		
    public float touchMoveSpeed = 0.5f;
    public float touchZoomSpeed = 0.05f;	
	
	private Camera cam;
    
    private float[] BoundsX = new float[]{-10f, 10f};
    private float[] BoundsZ = new float[]{-10f, 10f};
    private float[] ZoomBounds = new float[]{0.1f, 100f};

    private Vector3 lastPanPosition;
    private int panFingerId;
    
    private bool wasZoomingLastFrame;
    private Vector2[] lastZoomPositions;
	
	void Awake()
	{
		cam = GetComponent<Camera>();
	}

    void Update() 
	{
		if(SystemInfo.deviceType != DeviceType.Handheld) {
			return;
		}
		
        switch(Input.touchCount) {
			case 1: // Panning
				wasZoomingLastFrame = false;
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					lastPanPosition = touch.position;
					panFingerId = touch.fingerId;
				} else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) {
					PanCamera(touch.position);
				}
				break;
		
			case 2: // Zooming
				Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
				if (!wasZoomingLastFrame) {
					lastZoomPositions = newPositions;
					wasZoomingLastFrame = true;
				} else {
					float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
					float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
					float offset = newDistance - oldDistance;
					ZoomCamera(offset);
					lastZoomPositions = newPositions;
				}
				break;
				
			default: 
				wasZoomingLastFrame = false;
				break;
        }
    }
    
    void PanCamera(Vector3 newPanPosition) 
	{
		// Pan speed depends on zoom level
		float speed = touchMoveSpeed * cam.fieldOfView;
		if(speed > 20) {
			speed = 20;
		}
		Debug.Log("CameraMotionTouch: speed: " + speed);
		
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * speed, 0, offset.y * speed);
        
        // Perform the movement
        transform.Translate(move, Space.World);  
        
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;
    
        // Cache the position
        lastPanPosition = newPanPosition;
    }
    
    void ZoomCamera(float offset) 
	{		
        if (offset == 0) {
            return;
        }
    
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * touchZoomSpeed), ZoomBounds[0], ZoomBounds[1]);
    }
}