using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{

	public class Player : MonoBehaviour {



		static public bool boardReady = false;
		static public BoardManager boardManager;
		private bool move;

		private Vector3 mousePos;
		private Vector3 playerPos;
		private bool playerSelected = false;
		[SerializeField]
		private Circle circleUI;

		static public void receiveBoardManager(BoardManager x)
		{
			boardManager = x;
			boardReady = true;
		}

		void Start() {
			

		}

		// move character 1 unit 
		// if character will be target position return false;
		// if character will being moved, return true;
		public bool MoveCharacter()
		{
			string outstr = null;
			//				boardManager.naviMapList.ForEach (x => outstr += string.Format ("({0},{1}) ->", x.x, x.y));
			if(outstr != null)
				Debug.Log (outstr);
			Debug.Log ("char pos : (" + (transform.position.x + 1) + "," + (transform.position.y + 1) + ")");
			int currentIndex = boardManager.naviMapList.FindIndex (x => x.x == transform.position.x + 1 && x.y == transform.position.y + 1);
			Debug.Log ("currentIndex: " + currentIndex);

			if ( (int) (mousePos.x) == transform.position.x && (int) (mousePos.y) == transform.position.y) { // exit state
				return false;
			}

			if (boardManager.naviMapList [currentIndex + 1].x - (transform.position.x + 1) > 0) {
				this.transform.position = new Vector3 (this.transform.position.x + 1, this.transform.position.y, 0);
			} else if (boardManager.naviMapList [currentIndex + 1].x - (transform.position.x + 1) < 0) {
				this.transform.position = new Vector3 (this.transform.position.x - 1, this.transform.position.y, 0);
			} else {

				if (boardManager.naviMapList [currentIndex + 1].y - (transform.position.y + 1) > 0)
					this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1, 0);
				else if (boardManager.naviMapList [currentIndex + 1].y - (transform.position.y + 1) < 0)
					this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 1, 0);
			}	
			return true;
		}
			

		public void ToggleCharacterCircleUI(){
			if (circleUI.gameObject.activeSelf)
				circleUI.gameObject.SetActive (false);
			else
				circleUI.gameObject.SetActive (true);
		}

		// Update is called once per frame
		void Update () {
			/*
			if (Input.GetKeyDown (KeyCode.LeftArrow))
				this.transform.position = new Vector3 (this.transform.position.x -1, this.transform.position.y, 0);
			if (Input.GetKeyDown (KeyCode.RightArrow))
				this.transform.position = new Vector3 (this.transform.position.x + 1, this.transform.position.y, 0);
			if (Input.GetKeyDown (KeyCode.UpArrow))
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1, 0);
			if (Input.GetKeyDown (KeyCode.DownArrow))
				this.transform.position = new Vector3 (this.transform.position.x , this.transform.position.y-1, 0);
			*/


			if (Input.GetMouseButtonUp(0)) {

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
				RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity); 

				if (hit.collider != null) { 
					if (hit.collider.name == "Player"){
						ToggleCharacterCircleUI ();
						/*
						if (circleUI.gameObject.activeSelf)
							circleUI.gameObject.SetActive (false);
						else
							circleUI.gameObject.SetActive (true);
							*/
					}
					
				} else {
					mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					if ((mousePos.x >= 0 && mousePos.y >= 0) && (mousePos.x <= boardManager.rows && mousePos.y <= boardManager.columns)) {
						boardManager.naviMapList.Clear ();
						playerPos = transform.position;
						boardManager.pathFinder.AssignEnd ((int)(mousePos.x + 1), (int)(mousePos.y + 1));
						boardManager.pathFinder.AssignStart ((int)(playerPos.x + 1), (int)(playerPos.y + 1));
						Debug.Log ("start : " + "(" + playerPos.x + "," + playerPos.y + "), end : (" + (int)mousePos.x + "," + (int)mousePos.y + ")");
						boardManager.pathFinder.DoPathFind (boardManager.naviMapList);
						move = true;
					} else
						Debug.Log ("Moving target point is on out of range area");
				}
			}
				
			if (boardReady && move) {
				move = MoveCharacter ();
			}


		}
	}

}