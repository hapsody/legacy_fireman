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


	private Node[,] _arrMap;
	private Pos _curCheckNodePos;
	private Pos _startNodePos, _endNodePos, _nextStartNodePos, _nextEndNodePos; 
	private bool _assignEnd=false;
	private bool _firstInit = true;

	private List<Pos> _openList = new List<Pos>();
	private List<Pos> _closeList = new List<Pos>();



	public bool AllocMap(int maxRow, int maxCol)
	{
		_arrMap = new Node[maxRow, maxCol];

		for (int i = 0; i < _arrMap.GetLength (0); i++)
			for (int j = 0; j < _arrMap.GetLength (1); j++) {

				_arrMap [i, j] = new Node ();
				if (_firstInit) {
					_arrMap [i, j].Attribute = 0;
					_arrMap [i, j].ThisPos = new Pos (i, j);
					_firstInit = false;
				}

				_arrMap [i, j].ParentPos = null;
				_arrMap [i, j].F = 0;
				_arrMap [i, j].G = 0;
				_arrMap [i, j].H = 0;

			}


		if (_arrMap != null)
			return true;
		else
			Debug.Log ("Map Allocation Error");
			return false;





	}

	public bool AddOpenList(Pos pos){
		
		if (pos != null) {
			_openList.Add (pos);
			return true;
		} else {
			Debug.Log ("null");
			return false;
		}
	}

	public bool AddCloseList(Pos pos){
		
		if (pos != null) {
			_closeList.Add (pos);
			return true;
		} else {
			Debug.Log ("AddCloseList occurs Null Error");
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

	public int GetGValue(Pos pos, Pos parentPos){

		if (pos.I != parentPos.I && pos.J != parentPos.J)
			return 14;
		else
			return 10;
	}

	public int GetHValue(Pos pos)
	{
		return ( Mathf.Abs( _endNodePos.I - pos.I ) + Mathf.Abs( _endNodePos.J - pos.J) ) * 10;
	}

	public void CalTargetCost(Pos pos, Pos centerPos)
	{
		int latestG=0;

		if( centerPos.I != pos.I && centerPos.J != pos.J)
			latestG += 14;
		else
			latestG += 10;

		if (_arrMap [pos.I, pos.J].G == 0) { // first cost calculation of target node.
			_arrMap [pos.I, pos.J].G += latestG;
			_arrMap [pos.I, pos.J].H = GetHValue (pos);
			_arrMap [pos.I, pos.J].F = _arrMap [pos.I, pos.J].G + _arrMap [pos.I, pos.J].H;
			_arrMap [pos.I, pos.J].ParentPos = centerPos;
			AddOpenList(new Pos(pos.I, pos.J));
		} else { // not first cost calulatio of target node.
			if (_arrMap [centerPos.I, centerPos.J].G + latestG < _arrMap [pos.I, pos.J].G) {
				_arrMap [pos.I, pos.J].G = _arrMap [centerPos.I, centerPos.J].G + latestG;
				_arrMap [pos.I, pos.J].ParentPos = centerPos;
			}
		}


	}

	public bool IsInCloseList(Pos pos)
	{
		
		foreach (Pos b in _closeList)
		{
			if (b.I == pos.I && b.J == pos.J) {
				return true;
			}
		}

		return false;
	}


	public void CalAdjCost(Pos centerPos)
	{
		Pos targetPos = new Pos (centerPos.I, centerPos.J);

		//1. 2. Finding Adjacent node from start and adding to openlist.
		// 4. path scoring

		targetPos.J++;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 &&  // it has to not to be obstacle
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // right 
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.J--;

		/*
		targetPos.I++; targetPos.J++;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // right below
			//AddOpenList(new Pos(targetPos.I, targetPos.J));

		}
		targetPos.I--; targetPos.J--;
*/

		targetPos.I++; 
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos) ) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // below
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.I--; 

		/*
		targetPos.I++; targetPos.J--;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // left below
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.I--; targetPos.J++;
*/


		targetPos.J--;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // left
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.J++;

		/*
		targetPos.I--; targetPos.J--;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // upper left
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.I++; targetPos.J++;
*/

		targetPos.I--;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // upper
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.I++;

		/*
		targetPos.I--; targetPos.J++;
		if( _arrMap[targetPos.I, targetPos.J].Attribute >= 0 && 
			!IsInCloseList(targetPos)) //if not obstacle and closeList
		{
			CalTargetCost (targetPos, centerPos); // upper right
			//AddOpenList(new Pos(targetPos.I, targetPos.J));
		}
		targetPos.I++; targetPos.J--;
*/
	}

	public Pos GetMinCostNode()
	{
		int min = 99999; 
		Pos minCostPos = null;

		_openList.ForEach( x => 
			{
				if (_arrMap[x.I, x.J].F < min && _arrMap[x.I, x.J].F > 0 ){
					min = _arrMap[x.I, x.J].F;
					minCostPos = x;
				}
			}
		);

		return minCostPos;
	}


	public void PrintFinalRouteTrace (Pos pos)
	{
		if (pos == null)
			return;
		Debug.Log ("("+ pos.I + "," + pos.J + ")" + "->");
		PrintFinalRouteTrace (_arrMap [pos.I, pos.J].ParentPos);
	}

	public bool AssignStart(int x, int y)
	{
		if (_arrMap != null) {
			if ((x >= 0 && y >= 0) && x <= _arrMap.GetLength (0) && x <= _arrMap.GetLength (1)) {
				_nextStartNodePos = new Pos (x, y);
				return true;
			} else {
				Debug.Log ("Error: Start point is out of range ");
				return false;
			}
		} else
			return false;

	}


	public bool AssignEnd(int x, int y)
	{
		if (_arrMap != null && _assignEnd == false) {
			if ((x >= 0 && y >= 0) && x <= _arrMap.GetLength (0) && x <= _arrMap.GetLength (1)) {
				_nextEndNodePos = new Pos (x, y);
				_assignEnd = true;
				return true;
			} else {
				Debug.Log ("Error: End point is out of range ");
				return false;
			}

		} else
			return false;

	}

	public bool AssignOuterObstacle(int x, int y)
	{
		if (_arrMap != null) {
			_arrMap [x, y].Attribute = -2; // Obstacle
			return true;
		}
		else
			return false;
	}

	public bool AssignObstacle(int x, int y)
	{
		if (_arrMap != null) {
			_arrMap [x, y].Attribute = -1; // Obstacle
			return true;
		}
		else
			return false;
	}

	public bool InitPathFind()
	{
		if (!_assignEnd)
			return false;

		_openList.Clear ();
		_closeList.Clear ();
		//map initialize
		for (int i = 0; i < _arrMap.GetLength (0); i++)
			for (int j = 0; j < _arrMap.GetLength (1); j++) {
				
				//_arrMap [i, j] = new Node ();
				if(_arrMap[i,j].Attribute >= 0) // Because outer wall is -2. if outer obstacle is assigned once, it must not be changed.
					_arrMap [i, j].Attribute = 0;
				if( _firstInit )
					_arrMap [i, j].ThisPos = new Pos (i, j);
				_arrMap [i, j].ParentPos = null;
				_arrMap [i, j].F = 0;
				_arrMap [i, j].G = 0;
				_arrMap [i, j].H = 0;

			}

		// assign start - end node attribute
		_arrMap [_nextStartNodePos.I, _nextStartNodePos.J].Attribute = 5; // StartPoint
		//_startNodePos = _arrMap [_nextStartNodePos.I, _nextStartNodePos.J].ThisPos;
		_startNodePos = new Pos(_nextStartNodePos.I, _nextStartNodePos.J);
		_arrMap [_nextEndNodePos.I, _nextEndNodePos.J].Attribute = 6; // EndPoint
		//_endNodePos = _arrMap [_nextEndNodePos.I, _nextEndNodePos.J].ThisPos;
		_endNodePos = new Pos(_nextEndNodePos.I, _nextEndNodePos.J);




		//print map status
		string output = null;
		for (int i = 0; i < _arrMap.GetLength (0); i++) {
			output += string.Format ("[{0:00}]: ", i);
			for (int j = 0; j < _arrMap.GetLength (1); j++)
				output += " " + _arrMap [i, j].Attribute.ToString ();

			output += '\n';
		}
		Debug.Log (output);



		return true;
	}


	/// <summary>
	/// Just adapter functions ConvertPosToVector2, TraceFinalRoute, WriteNaviMapToList
	/// </summary>

	public Vector2 ConvertPosToVector2(Pos pos)
	{
		return new Vector2 (pos.I, pos.J);
	}

	public void TraceFinalRoute(List<Vector2> naviMapList, Pos pos)
	{
		if (_arrMap [pos.I, pos.J].ParentPos == null) {
			naviMapList.Add(ConvertPosToVector2 (pos));
			return;
		}

		TraceFinalRoute (naviMapList, _arrMap [pos.I, pos.J].ParentPos);
		naviMapList.Add(ConvertPosToVector2 (pos));
	}

	public bool WriteNaviMapToList(List<Vector2> naviMapList)
	{
		if (_arrMap [_endNodePos.I, _endNodePos.J].ParentPos != null) {
			TraceFinalRoute (naviMapList, _endNodePos);
			return true;
		} else
			return false;
	}

	public bool DoPathFind(List<Vector2> naviMapList)
	{
		bool result = InitPathFind ();

		if( !result )
			return false;
			


		// 3. Add start Node to closeList.
		AddCloseList(_startNodePos);

		Pos minCostPos = _startNodePos;
		while (!_closeList.Exists (x => x.I == _endNodePos.I && x.J == _endNodePos.J)) {

			CalAdjCost (minCostPos); // calculation Adjacent Node's costs
			minCostPos = GetMinCostNode ();
			//Debug.Log (string.Format ("minCostPos: ({0},{1}): F:{2}, G:{3}, H:{4}", minCostPos.I, minCostPos.J, _arrMap [minCostPos.I, minCostPos.J].F, _arrMap [minCostPos.I, minCostPos.J].G, _arrMap [minCostPos.I, minCostPos.J].H));
			// deleting selected node from openList and Adding to closeList
			_openList.Remove (minCostPos);
			_closeList.Add (minCostPos);

			//PrintOpenList ();
			//PrintCloseList ();
		}

		//Debug.Log ("Final Route :");
		//PrintFinalRouteTrace (_endNodePos);

		if (naviMapList != null) {
			WriteNaviMapToList (naviMapList);
		}

		_assignEnd = false;

		return true;

	}




}
