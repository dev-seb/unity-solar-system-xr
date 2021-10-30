using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotionGyro : MonoBehaviour
{		
    private float X = 0;
    private float Y = 0;
    private float Z = 0;

    private float[] xx = new float[10];
    private float[] zz = new float[10];

    void Start()
    {		
		Debug.Log("CameraMotionGyro.Start()");
		
		if(SystemInfo.deviceType != DeviceType.Handheld) {
			Debug.Log("device type is not handled");
			return;
		}
				
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.005f;
    }

    void Update()
    {	
		Debug.Log("CameraMotionGyro.Update()");
	
		if(SystemInfo.deviceType != DeviceType.Handheld) {
			Debug.Log("device type is not handled");
			return;
		}
		
		/*		
		// Set camera rotation according to gyro rotation
		transform.Rotate(
			-Input.gyro.rotationRateUnbiased.x, 
			-Input.gyro.rotationRateUnbiased.y, 
			0
		);
		*/
		
		// Set camera rotation according to gyro rotation + accelerometer
        float accX = Input.acceleration.x;
        float accY = Input.acceleration.y;
        float accZ = Input.acceleration.z;

        float x = 180 / Mathf.PI * Mathf.Atan (accZ / accY);
        float z = 180 / Mathf.PI * Mathf.Atan (accX / accY);

        xx [0] = xx [1];
        xx [1] = xx [2];
        xx [2] = xx [3];
        xx [3] = xx [4];
        xx [4] = xx [5];
        xx [5] = xx [6];
        xx [6] = xx [7];
        xx [7] = xx [8];
        xx [8] = xx [9];
        xx [9] = x;

        x = (xx [0] + xx [1] + xx [2] + xx [3] + xx [4] + xx [5] + xx [6] + xx [7] + xx [8] + xx [9]) / 10;

        zz [0] = zz [1];
        zz [1] = zz [2];
        zz [2] = zz [3];
        zz [3] = zz [4];
        zz [4] = zz [5];
        zz [5] = zz [6];
        zz [6] = zz [7];
        zz [7] = zz [8];
        zz [8] = zz [9];
        zz [9] = z;

        z = (zz [0] + zz [1] + zz [2] + zz [3] + zz [4] + zz [5] + zz [6] + zz [7] + zz [8] + zz [9]) / 10;

        //X += -Input.gyro.rotationRate.x;
        //Y += -Input.gyro.rotationRate.y;
        //Z += Input.gyro.rotationRate.z;
		
		X += -Input.gyro.rotationRateUnbiased.x;
        Y += -Input.gyro.rotationRateUnbiased.y;
        Z += Input.gyro.rotationRateUnbiased.z;

        X = (X + x) / 2;
        Z = (Z + z) / 2;

        transform.eulerAngles = new Vector3 (X, Y, Z);		
    }
}
