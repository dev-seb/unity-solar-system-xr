using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class SolarSystem : MonoBehaviour
{	
	// 3D
	public Camera mainCamera;
	public Button button3D;
	
	// VR
	public Camera vrCamera;
	public Button buttonVR;
	public Text vrSupportText;
	
	// AR
	public Camera arCamera;
	public Button buttonAR;
	public ARSession arSession;
	public ARSessionOrigin arSessionOrigin;
	public GameObject arScale;
	public Text arSupportText;
	
	public Canvas canvas;
	
	private bool arSupported = true;
	private bool vrSupported = true;
	
	// Device
	private bool isDesktop = false;
	private bool isTouch = false;
	private bool isVrHmd = false;
	private bool isArHmd = false;
	
	private string currentCamera = "3D";
	
	void Start () 
	{
		Debug.Log("SolarSystem.Start()");
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		// Detect device types
		isDesktop = SystemInfo.deviceType == DeviceType.Desktop;
		isVrHmd = SystemInfo.deviceName.Substring(0, 6) == "Oculus";
		isTouch = SystemInfo.deviceType == DeviceType.Handheld && !isVrHmd;
		// Show default view
		if(isVrHmd) {
			switchToVRCamera();
		}
		else if(isArHmd) {
			switchToARCamera();
		}
		else {
		    switchTo3DCamera();
		}
	}	
	
	public void setARSupported(bool supported) 
	{
		Debug.Log("SolarSystem.setARSupported(" + supported + ")");
		arSupported = supported;
	}
	
	public void setVRSupported(bool supported) 
	{
		Debug.Log("SolarSystem.setVRSupported(" + supported + ")");
		vrSupported = supported;
	}
	
	public void switchTo3DCamera() 
	{
		Debug.Log("SolarSystem.switchTo3DCamera()");
		// Disable AR session and camera
		arCamera.gameObject.SetActive(false);
		arSession.gameObject.SetActive(false);
		arSessionOrigin.gameObject.SetActive(false);
		//arScale.gameObject.SetActive(false);
		arSupportText.gameObject.SetActive(false);
		// Disable VR main camrea
		vrCamera.gameObject.SetActive(false);
		// Disable VR gyro rotation
		vrCamera.GetComponent<CameraMotionGyro>().enabled = false;
		vrCamera.GetComponent<CameraMotionTouch>().enabled = false;
		// Enable 3D main camera
		mainCamera.gameObject.SetActive(true);
		// Enable mouse / touch interactions
		if(isDesktop) {
			mainCamera.GetComponent<CameraMotionDesktop>().enabled = true;
		}
		if(isTouch) {
			mainCamera.GetComponent<CameraMotionTouch>().enabled = true;
		}
		// Change Canvas render camera
		canvas.GetComponent<Canvas>().enabled = true;
		canvas.GetComponent<Canvas>().worldCamera = mainCamera;
		// Update current camera
		setCurrentCamera("3D");
	}
	
	public void switchToVRCamera() 
	{
		Debug.Log("SolarSystem.switchToVRCamera()");	
		if(vrSupported) {
			// Disable AR session and camera
			arCamera.gameObject.SetActive(false);
			arSession.gameObject.SetActive(false);
			arSessionOrigin.gameObject.SetActive(false);
			//arScale.gameObject.SetActive(false);
			arSupportText.gameObject.SetActive(false);
			// Disable 3D main camera
			mainCamera.gameObject.SetActive(false);
			// Disable mouse / touch interactions
			mainCamera.GetComponent<CameraMotionDesktop>().enabled = false;
			mainCamera.GetComponent<CameraMotionTouch>().enabled = false;		
			// Enable VR main camera
			vrCamera.gameObject.SetActive(true);			
			// Enabled VR gyro rotation 
			if(isTouch || isDesktop) {
				if(isTouch) {
					vrCamera.GetComponent<CameraMotionGyro>().enabled = true;
					vrCamera.GetComponent<CameraMotionTouch>().enabled = true;
				}
				// Change Canvas render camera
				canvas.GetComponent<Canvas>().enabled = true;
				canvas.GetComponent<Canvas>().worldCamera = vrCamera;	
			}	
			else {
				vrCamera.GetComponent<CameraMotionGyro>().enabled = false;
				vrCamera.GetComponent<CameraMotionTouch>().enabled = false;
				canvas.GetComponent<Canvas>().enabled = false;
			}
		}
		else {
			vrSupportText.gameObject.SetActive(true);	
		}
		// Update current camera
		setCurrentCamera("VR");		
	}
	
	public void switchToARCamera() 
	{
		Debug.Log("SolarSystem.switchToARCamera()");
		if(arSupported) {
			// Disable AR session and camera
			arCamera.gameObject.SetActive(true);
			arSession.gameObject.SetActive(true);
			arSessionOrigin.gameObject.SetActive(true);
			//arScale.gameObject.SetActive(true);
			// Disable VR main camera
			vrCamera.gameObject.SetActive(false);
			// Disable VR gyro rotation
			vrCamera.GetComponent<CameraMotionGyro>().enabled = false;			
			vrCamera.GetComponent<CameraMotionTouch>().enabled = false;
			// Disable 3D main camera
			mainCamera.gameObject.SetActive(false);
			// Disable mouse / touch interactions
			mainCamera.GetComponent<CameraMotionDesktop>().enabled = false;
			mainCamera.GetComponent<CameraMotionTouch>().enabled = false;		
			// Change Canvas render camera
			canvas.GetComponent<Canvas>().worldCamera = arCamera;
		}	
		else {
			arSupportText.gameObject.SetActive(true);
		}
		// Update current camera
		setCurrentCamera("AR");
	}
	
	
	private void setCurrentCamera(string cameraName) 
	{
		currentCamera = cameraName;
		highlightButton();
	}	
	
	private void highlightButton() 
	{
		Debug.Log("SolarSystem.highlightButton()");		
		/*
		// Reset all button colors
		button3D.colors = ColorBlock.defaultColorBlock;
		buttonVR.colors = ColorBlock.defaultColorBlock;
		buttonAR.colors = ColorBlock.defaultColorBlock;
		// highlight selected button
		if(currentCamera.Equals("3D")) {
			button3D.colors.normalColor = new Color(75, 95, 140);
		}
		*/
	}	
}
