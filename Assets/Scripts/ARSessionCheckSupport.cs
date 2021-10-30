using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARSessionCheckSupport : MonoBehaviour
{
    [SerializeField] ARSession arSession;
	
	public GameObject solarSystem;

    IEnumerator Start() 
	{
		Debug.Log("ARSessionCheckSupport.Start()");
        if (
			(ARSession.state == ARSessionState.None) || 
			(ARSession.state == ARSessionState.CheckingAvailability)
		) {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported) {
            Debug.Log("AR is not supported on this device");
			// Disable AR for this device and fallback on 3D Camera
			SolarSystem solarSystemScript = solarSystem.GetComponent<SolarSystem>();
			solarSystemScript.setARSupported(false);
			//solarSystemScript.switchTo3DCamera();
        }
        else {
            Debug.Log("AR is supported on this device");
			// Start on AR by default
        }
    }
}
