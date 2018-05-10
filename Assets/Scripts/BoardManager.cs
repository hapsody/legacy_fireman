using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TGAME;

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

    public int columns = 8;                                         //Number of columns in our game board.
    public int rows = 8;                                            //Number of rows in our game board.

    private const float c_fTileScale = 2.0f;

    private const int c_nTileLength = 14;

    string m_strPath = "Assets/Resources/";

    //Awake is always called before any Start functions
    void Awake()
    {
        TPrefabMgr.Load("Prefabs", null);

        BoardInit();
    }

    /*
    private void PutTile(int iX, int iY, int iZ)
    {
        GameObject tile = TPrefabMgr.Instance("Tile1", "Tile1", iX, iY, iZ);
        Vector3 objectScl = Vector3.Scale(tile.transform.localScale, new Vector3(c_fTileScale, c_fTileScale, 1.0f));
        tile.transform.localScale = objectScl;
    }
    */

    public void BoardInit()
	{

        xSize = ySize = 9; // map size
        _map = new BoardHolder[xSize, ySize];

        //_map init
        for (int i = 0; i < ySize; i++)
            for (int j = 0; j < xSize; j++)
                _map[i, j] = new BoardHolder();

        //tile set up
        bool isLight = true;
        for (int i = 0; i < ySize; i++)
        {
            if (ySize % 2 == 0)
            {
                if (isLight == true)
                    isLight = false;
                else
                    isLight = true;
            }

            for (int j = 0; j < xSize; j++)
            {


                if (isLight == true)
                {
                    _grassLight.transform.position = new Vector3(j * c_nTileLength, 0, i * c_nTileLength);
                    _map[i, j].objList.Add(GameObject.Instantiate(_grassLight));
                    isLight = false;
                }
                else
                {
                    _grassDark.transform.position = new Vector3(j * c_nTileLength, 0, i * c_nTileLength);
                    _map[i, j].objList.Add(GameObject.Instantiate(_grassDark));
                    isLight = true;
                }
            }

        } // for statement


        //int iX = 0;
        int iY = 0;
        int iZ = 10;
        
        TextAsset data = Resources.Load("stage1", typeof(TextAsset)) as TextAsset;

        StringReader sr = new StringReader(data.text);

        // 먼저 한줄을 읽는다. 
        string source = sr.ReadLine();

        string[] values;                // 쉼표로 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)

        {
            values = source.Split(',');  // 쉼표로 구분한다. 저장시에 쉼표로 구분하여 저장하였다.

            if (values.Length == 0)

            {

                sr.Close();

                return;

            }

            for (int i = 0; i < values.Length; i++)
            {
                //0: outer wall, 1:tree, 2:emptyspace, 3:climber, 4:fire, 5:fireman
                string objname = "";
                if (values[i].Equals("0"))
                {
                    //objname = "OuterWall1";
                    continue;
                }
                else if (values[i].Equals("1"))
                {
                    //PutTile(iX + i, iY, iZ);

                    objname = "Tree1";
                }
                else if (values[i].Equals("2"))
                {
                    //PutTile(iX + i, iY, iZ);
                    continue;
                }
                else if (values[i].Equals("3"))
                {
                    //PutTile(iX + i, iY, iZ);

                    objname = "Climber";
                }
                else if (values[i].Equals("4"))
                {
                    //PutTile(iX + i, iY, iZ);

                    //objname = "Fire";
                    continue;
                }
                else if (values[i].Equals("5"))
                {
                    //PutTile(iX + i, iY, iZ);

                    objname = "Fireman";
                }


                GameObject obj = TPrefabMgr.Instance(objname, objname, ((i-1)* c_nTileLength) - (c_nTileLength / 2), iY, (iZ * c_nTileLength) - (c_nTileLength / 2));
                Vector3 objectScale = Vector3.Scale(obj.transform.localScale, new Vector3(c_fTileScale, c_fTileScale, c_fTileScale));
                obj.transform.localScale = objectScale;
                

            }

            source = sr.ReadLine();    // 한줄 읽는다.
            iZ--;
        }

        /*
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

			
    */
    } // BoardInit

    // Use this for initialization
    void Start () {
		//BoardInit ();
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
