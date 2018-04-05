using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	private int attribute;
	public int Attribute { get {return attribute;} set{attribute = value;}}
}


public class Pathfind : MonoBehaviour {

	private Node[,] _arrMap = new Node[20, 20];

	void Start(){

		for (int i = 0; i < _arrMap.GetLength (0); i++)
			for (int j = 0; j < _arrMap.GetLength (1); j++) {
				_arrMap [i, j] = new Node ();
				_arrMap [i, j].Attribute = 0;
			}

		_arrMap [10, 8].Attribute = 5;  // start
		_arrMap [9, 10].Attribute = _arrMap [10, 10].Attribute = _arrMap [11, 10].Attribute = 1; //obstacles
		_arrMap [10, 12].Attribute = 6; // end
	


		string output = null;

		for (int i = 0; i < _arrMap.GetLength (0); i++) {
			output += string.Format ("[{0:00}]: ", i);
			for (int j = 0; j < _arrMap.GetLength (1); j++)
				output += " " + _arrMap [i, j].Attribute.ToString ();

			output += '\n';
		}
	}
	/*
	// start : 5, end : 6, obstacle : 1

	int[,] _arrMap = new int[20,20];
	// Use this for initialization
	void Start () {
		for (int i = 0; i < _arrMap.GetLength(0); i++)
			for (int j = 0; j < _arrMap.GetLength(1); j++)
				_arrMap [i, j] = 0;


		_arrMap [10, 8] = 5;  // start
		_arrMap [9, 10] = _arrMap [10, 10] = _arrMap [11, 10] = 1; //obstacles
		_arrMap [10, 12] = 6; // end

		string output = null;

		for (int i = 0; i < _arrMap.GetLength (0); i++) {
			output += string.Format ("[{0:00}]: ", i);
			for (int j = 0; j < _arrMap.GetLength (1); j++)
				output += " " + _arrMap [i, j].ToString ();
			
			output += '\n';
		}



		Debug.Log (output);

	}
	
	*/
}
