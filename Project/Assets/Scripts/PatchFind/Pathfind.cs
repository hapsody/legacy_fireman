using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pos{
	private int i;
	private int j;
	public int I { get { return i; } set { i = value; } }
	public int J { get { return j; } set { j = value; } }

	public Pos(int a, int b)
	{
		i = a;
		j = b;
	}
}

public class Node {

	private int f=0, g=0, h=0;
	public int F { get {return f;} set{f = value;}}
	public int G { get {return g;} set{g = value;}}
	public int H { get {return h;} set{h = value;}}

	private int attribute;
	public int Attribute { get {return attribute;} set{attribute = value;}}

	private Pos thisPos;
	public Pos ThisPos { get { return thisPos; } set { thisPos = value; } }

	private Pos parentPos;
	public Pos ParentPos { get { return parentPos; } set { parentPos = value; } }

}



public class Pathfind : MonoBehaviour {

	private Node[,] _arrMap = new Node[20, 20];
	private Pos _curCheckNodePos;
	private Pos _startNodePos, _endNodePos; 

	private List<Pos> _openList = new List<Pos>();
	private List<Pos> _closeList = new List<Pos>();



	public bool AddOpenList(Pos pos, Pos parentPos){
		
		if (_arrMap [pos.I, pos.J].Attribute != 1) { // exept obstacle terrain.
			if (parentPos != null) {
				if ( !_openList.Exists(x => x.I == pos.I && x.J == pos.J)) {
					_openList.Add (pos);
					_arrMap [pos.I, pos.J].ParentPos = parentPos; 
				} else {

				}
			} else {
				Debug.Log (" parentPos doesn't exist ");
				return false;
			}
		} else {
			Debug.Log (string.Format ("{0},{1} is obstacle", pos.I, pos.J));
			return false;
		}

		return true;
	}

	public bool AddCloseList(Pos pos){
		if (pos != null) {
			_closeList.Add (pos);
			return true;
		} else {
			return false;
		}
	}

	public bool PrintParent(Pos pos){
		string outstr = null;

		if (_arrMap [pos.I, pos.J].ParentPos != null) {
			outstr += string.Format ("({0},{1}) -> ({2},{3})", _arrMap [pos.I, pos.J].ParentPos.I, _arrMap [pos.I, pos.J].ParentPos.J,    pos.I, pos.J);
			Debug.Log (outstr);

			return true;
		} else {
			Debug.Log( string.Format( "({0},{1}) has no parent.", pos.I, pos.J));
			return false;
		}
	}

	public void PrintOpenList() {
		string outstr = null;
		outstr += "openList: ";
		_openList.ForEach( x => 
			{
				//PrintParent(x);
				outstr += "(" + x.I.ToString() + "," + x.J.ToString() + ") ";

			});

		Debug.Log(outstr);

	}

	public void PrintCloseList() {
		string outstr = null;
		outstr += "closeList: ";
		_closeList.ForEach( x => 
			{
				outstr += "(" + x.I.ToString() + "," + x.J.ToString() + ") ";
			});

		Debug.Log(outstr);

	}



	public Pos GetMinCostNode()
	{
		int min = 99999; 
		Pos minCostPos = null;

		_openList.ForEach( x => 
			{
				// Get g cost
				if ( x.I != _arrMap[x.I,x.J].ParentPos.I && x.J != _arrMap[x.I,x.J].ParentPos.J )
					_arrMap[x.I,x.J].G += 14;
				else
					_arrMap[x.I,x.J].G += 10; 

				// Get h cost
				_arrMap[x.I, x.J].H = ( Mathf.Abs( _endNodePos.I - x.I ) + Mathf.Abs( _endNodePos.J - x.J) ) * 10;

				// Get f cost
				_arrMap[x.I, x.J].F = _arrMap[x.I,x.J].G + _arrMap[x.I, x.J].H;

				if( min > _arrMap[x.I, x.J].F ){
					min = _arrMap[x.I, x.J].F;
					minCostPos = x;
				}
			});

		return minCostPos;

	}


	void Start(){

		//map initialize
		for (int i = 0; i < _arrMap.GetLength (0); i++)
			for (int j = 0; j < _arrMap.GetLength (1); j++) {
				_arrMap [i, j] = new Node ();
				_arrMap [i, j].Attribute = 0;
				_arrMap [i, j].ThisPos = new Pos (i, j);
			}

		// assign node attribute
		_arrMap [10, 8].Attribute = 5;  // start
		_arrMap [9, 10].Attribute = _arrMap [10, 10].Attribute = _arrMap [11, 10].Attribute = 1; //obstacles
		_arrMap [10, 12].Attribute = 6; // end
	

		//print map status
		string output = null;
		for (int i = 0; i < _arrMap.GetLength (0); i++) {
			output += string.Format ("[{0:00}]: ", i);
			for (int j = 0; j < _arrMap.GetLength (1); j++)
				output += " " + _arrMap [i, j].Attribute.ToString ();

			output += '\n';
		}
		Debug.Log (output);


		// find start, end node
		for (int i = 0; i < _arrMap.GetLength (0); i++) {
			for (int j = 0; j < _arrMap.GetLength (1); j++) {
				if (_arrMap [i, j].Attribute == 5)
					_startNodePos = _arrMap [i, j].ThisPos;
				else if (_arrMap [i, j].Attribute == 6)
					_endNodePos = _arrMap [i, j].ThisPos;
			}
		}

		//1. 2. Finding Adjacent node from start and adding to openlist.
		AddOpenList( new Pos(_startNodePos.I, _startNodePos.J+1), _startNodePos);
		AddOpenList( new Pos(_startNodePos.I+1, _startNodePos.J+1), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I+1, _startNodePos.J), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I+1, _startNodePos.J-1), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I, _startNodePos.J-1), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I-1, _startNodePos.J-1), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I-1, _startNodePos.J), _startNodePos );
		AddOpenList( new Pos(_startNodePos.I-1, _startNodePos.J+1), _startNodePos );



		// 3. Add start Node to closeList.
		AddCloseList(_startNodePos);

		// 4. path scoring
		Pos minCostPos = GetMinCostNode();
		Debug.Log (string.Format ("minCostPos: ({0},{1}): F:{2}, G:{3}, H:{4}", minCostPos.I, minCostPos.J, _arrMap[minCostPos.I, minCostPos.J].F, _arrMap[minCostPos.I, minCostPos.J].G, _arrMap[minCostPos.I, minCostPos.J].H));
		// deleting selected node from openList and Adding to closeList
		_openList.Remove(minCostPos);
		_closeList.Add (minCostPos);

		PrintOpenList ();
		PrintCloseList ();
		Debug.Log(_openList.Exists(x => x.I == 11 && x.J == 9));
		Debug.Log(_openList.Exists(x => x.I == 2 && x.J == 3));


		// 5. Find adjacent Node from selected Node 
		//    and among those things, only not exist things at openlist already, Add to openlist
		//    In other words, minCostPos is new parent from adjacent nodes.

		AddOpenList( new Pos(minCostPos.I, minCostPos.J+1 ), minCostPos);
		AddOpenList( new Pos(minCostPos.I+1, minCostPos.J+1 ), minCostPos);
		AddOpenList( new Pos(minCostPos.I+1, minCostPos.J ), minCostPos);
		AddOpenList( new Pos(minCostPos.I+1, minCostPos.J-1 ), minCostPos);
		AddOpenList( new Pos(minCostPos.I, minCostPos.J-1 ), minCostPos);
		AddOpenList( new Pos(minCostPos.I-1, minCostPos.J-1 ), minCostPos);
		AddOpenList( new Pos(minCostPos.I-1, minCostPos.J ), minCostPos);
		AddOpenList( new Pos(minCostPos.I-1, minCostPos.J+1 ), minCostPos);

	}
	
}
