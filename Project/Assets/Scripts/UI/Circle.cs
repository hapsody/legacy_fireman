using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

	public int segments = 200;
	public float xradius = 1;
	public float yradius = 1;
	LineRenderer line;


	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.positionCount = segments + 1;
		line.useWorldSpace = false;
		Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
		line.material = whiteDiffuseMat;
		CreatePoints ();


	}
		
	
	void CreatePoints() {
		float x;
		float y;
		float z = -0.1f;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Cos (Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Sin (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i, new Vector3 (x, y, z));
			angle += (360f / segments);
		}
	}
}
