using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour {

	[SerializeField]
	private GameObject _grassLight = null;
	[SerializeField]
	private GameObject _grassDark = null;
	[SerializeField]
	private GameObject[] _tree = null;
	private BoardHolder[,] _map = null;
	[SerializeField]
	private Character _character = null;

	private int xSize;
	private int ySize;



	public void BoardInit()
	{
		
		xSize = ySize = 8; // map size
		_map = new BoardHolder[xSize, ySize];

		//_map init
		for (int i = 0; i < ySize; i++)
			for (int j = 0; j < xSize; j++)
				_map[i,j] = new BoardHolder ();

		//tile set up
		bool isLight = true; 
		for (int i = 0; i < ySize; i++) {
			if (ySize % 2 == 0) {
				if (isLight == true)
					isLight = false;
				else
					isLight = true;
			}

			for (int j = 0; j < xSize; j++) {
				

				if (isLight == true) {
					_grassLight.transform.position = new Vector3 (j * 14, 0, i * 14);
					_map[i,j].objList.Add(GameObject.Instantiate (_grassLight));
					isLight = false;
				} else {
					_grassDark.transform.position = new Vector3 (j * 14, 0, i * 14);
					_map[i,j].objList.Add(GameObject.Instantiate (_grassDark));
					isLight = true;
				}
			} 
		} // for statement


		// Determining firefighter's starting point.
		List<int> charPositionNumbers = new List<int>();
		charPositionNumbers = GetRandomNumbers(0, xSize, _character._fireMans.Length, null);
		charPositionNumbers.Sort ();

		int cnt=0;
		charPositionNumbers.ForEach (item => {
			
			_character._charTextures[cnt].transform.position = new Vector3( item * 14 -7, 0, 7);
			_character._charTextures[cnt].transform.localScale = new Vector3( 4,4,4);
			_character._fireMans[cnt] = GameObject.Instantiate(_character._charTextures[cnt]);
			cnt++;
		});
		

		//tree random set up
		List<int> treePositionNumbers = new List<int>();
		int mapArea = xSize * ySize - 1;
		treePositionNumbers = GetRandomNumbers(0, mapArea, mapArea / 2, charPositionNumbers);
		treePositionNumbers.Sort ();


		treePositionNumbers.ForEach (item => {
			int x = item % xSize;
			int y = item / xSize;
			int ranNum =  UnityEngine.Random.Range(0,3);
			_tree[ranNum].transform.position = new Vector3( (x)*14-7, 0, (y)*14 + 7);
			_tree[ranNum].transform.localScale = new Vector3( 2,2,2);
			_map[x, y].objList.Add(GameObject.Instantiate(_tree[ranNum]));
		});

			

	} // BoardInit

	// Use this for initialization
	void Start () {
		BoardInit ();
	} 
	
	// Update is called once per frame
	void Update () {
		
	} //Update() end


	public List<int> GetRandomNumbers(int start, int end, int n, List<int> exceptList)
	{
		List<int> randomNumbers = new List<int> ();
		int temp;
		
		for ( int i=0; i<n;)
		{
			temp = UnityEngine.Random.Range (start, end);
			if (!randomNumbers.Contains (temp)) {
				if (exceptList == null || !exceptList.Contains (temp)) {
					randomNumbers.Add (temp);
					i++;
				}	
			}
		}
		
		return randomNumbers;
	}

} // class end


public class BoardHolder {
	public List<GameObject> objList = new List<GameObject>();

	public BoardHolder ()
	{
		objList = new List<GameObject> ();
	}
}
