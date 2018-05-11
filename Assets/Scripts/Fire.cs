using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public GameObject _fireSample = null;
	private List<GameObject> _fires = null;
	private int i=0;
	// Use this for initialization
	void Start () {
		_fires = new List<GameObject> ();
		_fireSample.transform.position = new Vector3 (-5, 10, 5);
		_fires.Add( GameObject.Instantiate (_fireSample));

	}
	
	// Update is called once per frame
	void Update () {
		
		if( Input.GetKeyDown(KeyCode.Space)){
			i++;
			_fireSample.transform.position = new Vector3 (-5+14*i, 10, 5+14*i);
			_fires.Add( GameObject.Instantiate (_fireSample));
		}
	}
}
