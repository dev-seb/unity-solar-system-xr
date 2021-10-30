using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
	
	public float radius;
	public bool faded = true;
	
	private float lineWidth = .002f;
	private LineRenderer orbit = null;
		 
	void Start ()
	{
		int segments = (int)radius * 100;
				
		orbit = gameObject.AddComponent<LineRenderer>();
		orbit.useWorldSpace = false;
		orbit.startWidth = lineWidth;
		orbit.endWidth = lineWidth;	
		
		orbit.sortingLayerName = "Default";
		orbit.sortingOrder = 0;
			
		Material material = new Material(Shader.Find("Sprites/Default"));
		//Material material = new Material(Shader.Find("Particles/Standard Surface"));
		orbit.material = material;	
		
		var positionCount = segments + 1;
        var positions = new Vector3[positionCount];
		
        for (int i = 0; i < positionCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            positions[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

		orbit.positionCount = positionCount;
        orbit.SetPositions(positions);
	
		if(faded) {
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
			);
			orbit.colorGradient = gradient;		
		}
					
	}
	 
}
