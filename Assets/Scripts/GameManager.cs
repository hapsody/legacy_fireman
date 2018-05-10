using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager _instance = null;
	public static GameManager Instance { get { return _instance; } set{ _instance = value; } }

	[SerializeField]
	private BoardManager _boardManager = null;

	public void GameInit()
	{
		
	}

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
