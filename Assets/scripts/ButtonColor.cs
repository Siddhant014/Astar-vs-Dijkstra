using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor;
//using VCSKicksCollection;
using PriorityQueueDemo;
public class ButtonColor : MonoBehaviour {
	struct mat{
		public int x;
		public int y;
		public int d;
	};
	struct matHU{
		public int x;
		public int y;
		public int d;
		public int hu;
	};
	public int Xdes,Ydes,Xso,Yso;
	public Text text;
	private ColorBlock theColorBlack;
	private ColorBlock theColorWhite;
	ColorBlock theColorBlue;
	private ColorBlock theColorRed;
	ColorBlock theColorGreen;
	ColorBlock theColorOrange;
	public ColorBlock theColorGrey;
//	ColorBlock ColortoChange;
	int X,Y;
	bool changeColor,source,destination;
	public Button[] b1 = new Button[14];
	public Button[] b2 = new Button[14];
	public Button[] b3 = new Button[14];
	public Button[] b4 = new Button[14];
	public Button[] b5 = new Button[14];
	public Button[] b6 = new Button[14];
	public Button[] b7 = new Button[14];
	public Button[,] theButtonsArray = new Button[20,20];
	int[,] matrix=new int[20,20];
	// Use this for initialization
	//List <mat> dij=new List<mat>();
	PriorityQueue <int ,mat> dij=new PriorityQueue<int, mat>();
	int[,] dijvismatrix=new int[20,20];
	int[,] dijExpmatrix=new int[20,20];
	PriorityQueue <int ,matHU> AStar=new PriorityQueue<int, matHU>();
	int[,] Astarvismatrix=new int[20,20];
	int[,] AstarExpmatrix=new int[20,20];
	bool Selection;
	int DijCount=0,AstarCount=0;
	float fspeed = 1;
	int speed=100;
	int pathLength;
	public void speedup(){
		fspeed *= 1.1f;
		speed--;
	}
	public void speeddown(){
		fspeed /=1.1f;
		speed++;
	}
	public void mahaReset(){
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}
	public void reset(){
		for (int i = 0; i < 14; i++) {
			for (int j = 0; j < 7; j++) {
				theButtonsArray [j,i].GetComponentInChildren<Text>().text ="";
				//				matrix [j, i] = 0;
				dijvismatrix [j, i] = 1000;
				dijExpmatrix [j, i] = 0;
				if (matrix [j, i] == 0)
					theButtonsArray [j, i].colors = theColorWhite;
				if (matrix [j, i] == 2)
					theButtonsArray [j, i].colors = theColorBlue;
				if (matrix [j, i] == 3)
					theButtonsArray [j, i].colors = theColorRed;
				if (matrix [j, i] == 1)
					theButtonsArray [j, i].colors = theColorBlack;
			}
		}
	}
	public void Dijalg(){
		StopAllCoroutines ();
		DijCount = 0;
		reset ();
//		Debug.Log ("button");
		mat m;
		m.x = Xso;
		m.y = Yso;
		m.d = 0;
//		ColorChanging (Xso,Yso);
		Selection =false;
		StartCoroutine(Dijkstra(m));
	}
	public void AStaralg(){
		StopAllCoroutines ();
		AstarCount = 0;
		reset ();
		//		Debug.Log ("button");
		matHU m;
		m.x = Xso;
		m.y = Yso;
		m.d = 0;
		m.hu = diff(Xso,Xdes)+diff(Yso,Ydes);
		//		ColorChanging (Xso,Yso);
		Selection =false;
		StartCoroutine(AStarAlgo(m));
	}
	/*void ColorChanging(int x,int y){
		StartCoroutine (Sec1 (x, y));
	}*/

	IEnumerator Backtrack(int x,int y){
		//change color of this
		pathLength++;
		theButtonsArray [x,y].colors = theColorGrey;
		if (dijvismatrix [x, y] == 0) {
//			yield return new WaitForSeconds (20f);
			text.text = "Dijkstra:-"+DijCount+"\t\tAStar:-"+AstarCount+"\nSpeed="+speed+"\t\tMin Path Length="+pathLength;
			StopAllCoroutines ();
		}
		
		yield return new WaitForSeconds (.2f);
		if(x!=0&&dijvismatrix[x-1,y]==dijvismatrix[x,y]-1){
			yield return StartCoroutine (Backtrack (x-1,y));
		}
		else if(x!=6&&dijvismatrix[x+1,y]==dijvismatrix[x,y]-1) {
			yield return StartCoroutine (Backtrack (x+1,y));
		}
		else if(y!=0&&dijvismatrix[x,y-1]==dijvismatrix[x,y]-1) {
			yield return StartCoroutine (Backtrack (x,y-1));
		}
		else if(y != 13 && dijvismatrix[x,y+1]==dijvismatrix[x,y]-1) {
			yield return StartCoroutine (Backtrack (x,y+1));
		}
	}


	IEnumerator Sec1(int x,int y){
		theButtonsArray [x,y].colors = theColorBlue;
		yield return new WaitForSeconds (fspeed);
		if (x > 0 && dijvismatrix [x - 1, y] == 1000 && matrix [x - 1, y] != 1) {
			if(x-1==Xdes&&y==Ydes)
				theButtonsArray [x-1,y].colors = theColorGrey;
			else
				theButtonsArray [x-1,y].colors = theColorGreen;
		}
		if(x<7&&dijvismatrix[x+1, y]==1000&&matrix[x+1,y]!=1) {
			if(x+1==Xdes&&y==Ydes)
				theButtonsArray [x+1,y].colors = theColorGrey;
			else
				theButtonsArray [x+1,y].colors = theColorGreen;
		}
		if(y>0&&dijvismatrix[x, y-1]==1000&&matrix[x,y-1]!=1) {
			if(x==Xdes&&y-1==Ydes)
				theButtonsArray [x,y-1].colors = theColorGrey;
			else
				theButtonsArray [x,y-1].colors = theColorGreen;
		}
		if(y<13&&dijvismatrix[x, y+1]==1000&&matrix[x,y+1]!=1) {
			if(x==Xdes&&y+1==Ydes)
				theButtonsArray [x,y+1].colors = theColorGrey;
			else
				theButtonsArray [x,y+1].colors = theColorGreen;
		}
		yield return new WaitForSeconds (fspeed);
		theButtonsArray [x,y].colors = theColorOrange;
		//flag = false;
	}
	int diff(int x,int y){
		if (x < y)
			return y - x;
		return x - y;
	}

	IEnumerator AStarAlgo(matHU m){
		
		float p = (float)m.d;
//		beep.pitch = originalPitch* (p/5);
//		beep.Play ();
		AstarCount++;
		text.text = "Dijkstra:-"+DijCount+"\t\tAStar:-"+AstarCount+"\nSpeed="+speed;
		Debug.Log ("Enter with" + "x=" + m.x + "y=" + m.y);
		//		ColortoChange = theColorGreen;
		//		Debug.Log ("dijCall"+m.x+m.y);
		if (m.x>6||m.x<0||m.y>13||m.y<0)
			yield return null;
		if(matrix[m.x,m.y]==1)
			yield return null;
		if (dijvismatrix [m.x, m.y] != 1000) {
			if(m.d>=dijvismatrix [m.x, m.y])
				yield return null;
			m.d = dijvismatrix [m.x, m.y];
		}
		if (matrix [m.x,m. y] == 3) {
			dijvismatrix [m.x, m.y] = m.d;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text>().text =""+ m.d+" "+m.hu;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().fontSize = 50;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().color = Color.red;
			//call
			pathLength=0;
			yield return StartCoroutine (Backtrack(m.x,m.y));
		}
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text>().text =""+  m.d+" "+m.hu;
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().fontSize = 50;
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().color = Color.red;
		dijvismatrix [m.x,m. y] = m.d; //visited
		matHU di;
		//		SelectX (m.x);
		//		SelectY (m.y);
		int x1=m.x,y1=m.y;
		//theButtonsArray [m.x, m.y].colors = theColorBlue;

		di.x = m.x;
		di.y = m.y;
		di.d = m.d+1;
		//		KeyValuePair <int,mat> p = new KeyValuePair<int ,mat> (di.d+1, di);
		//		p.Key = di.d;
		yield return StartCoroutine(Sec1(x1,y1));
		//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.x != 6) {
			di.x = x1 + 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y]==1000&&dijExpmatrix [di.x, di.y] == 0) {
				di.hu = diff (di.x, Xdes) + diff (di.y, Ydes);
				Debug.Log ("Addded (" + di.x + "," + di.y + ")"+di.hu);
				AStar.Add (new KeyValuePair<int ,matHU> (di.d+di.hu, di));
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.x = x1;
		}
		//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.x != 0) {
			di.x = x1 - 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				di.hu = diff (di.x, Xdes) + diff (di.y, Ydes);
				Debug.Log ("Addded (" + di.x + "," + di.y + ")"+di.hu);
				AStar.Add (new KeyValuePair<int ,matHU> (di.d+di.hu, di));	
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.x = x1;
		}
		//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.y != 13) {
			di.y = y1 + 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				di.hu = diff (di.x, Xdes) + diff (di.y, Ydes);
				Debug.Log ("Addded (" + di.x + "," + di.y + ")"+di.hu);
				AStar.Add (new KeyValuePair<int ,matHU> (di.d+di.hu, di));
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.y = y1;
		}
		//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.y != 0) {
			di.y = y1 - 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				di.hu = diff (di.x, Xdes) + diff (di.y, Ydes);
				AStar.Add (new KeyValuePair<int ,matHU> (di.d+di.hu, di));
				Debug.Log ("Addded (" + di.x + "," + di.y + ")"+di.hu);
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.y = y1;
		}
		//		Debug.Log(dij.Count);
		//		yield return StartCoroutine(Sec1());
		//theButtonsArray [m.x, m.y].colors = theColorOrange;
		while (AStar.Count!=0) {
			matHU k = AStar.DequeueValue ();
			yield return StartCoroutine(AStarAlgo(k));
		}
	}




	IEnumerator Dijkstra(mat m){
		float p = (float)m.d;
//		beep.pitch = originalPitch* (p/5);
//		beep.Play ();
		DijCount++;
		text.text = "Dijkstra:-"+DijCount+"\t\tAStar:-"+AstarCount+"\nSpeed="+speed;
		Debug.Log ("Enter with" + "x=" + m.x + "y=" + m.y);
//		ColortoChange = theColorGreen;
//		Debug.Log ("dijCall"+m.x+m.y);`
		if (m.x>6||m.x<0||m.y>13||m.y<0)
			yield return null;
		if(matrix[m.x,m.y]==1)
			yield return null;
		if (dijvismatrix [m.x, m.y] != 1000) {
			if(m.d>=dijvismatrix [m.x, m.y])
				yield return null;
			m.d = dijvismatrix [m.x, m.y];
		}
		if (matrix [m.x,m. y] == 3) {
			dijvismatrix [m.x, m.y] = m.d;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text>().text =""+ m.d;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().fontSize = 50;
			theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().color = Color.red;
			//call
			pathLength=0;
			yield return StartCoroutine (Backtrack(m.x,m.y));
		}
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text>().text =""+ m.d;
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().fontSize = 50;
		theButtonsArray [m.x, m.y].GetComponentInChildren<Text> ().color = Color.red;
		dijvismatrix [m.x,m. y] = m.d; //visited
		mat di;
//		SelectX (m.x);
//		SelectY (m.y);
		int x1=m.x,y1=m.y;
		//theButtonsArray [m.x, m.y].colors = theColorBlue;

		di.x = m.x;
		di.y = m.y;
		di.d = m.d+1;
//		KeyValuePair <int,mat> p = new KeyValuePair<int ,mat> (di.d+1, di);
//		p.Key = di.d;
		yield return StartCoroutine(Sec1(x1,y1));
//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.x != 6) {
			di.x = x1 + 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y]==1000&&dijExpmatrix [di.x, di.y] == 0) {
				Debug.Log ("Addded ("+di.x+","+di.y+")");
				dij.Add (new KeyValuePair<int ,mat> (di.d, di));
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.x = x1;
		}
//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.x != 0) {
			di.x = x1 - 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				Debug.Log ("Addded ("+di.x+","+di.y+")");
				dij.Add (new KeyValuePair<int ,mat> (di.d, di));	
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.x = x1;
		}
//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.y != 13) {
			di.y = y1 + 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				Debug.Log ("Addded (" + di.x + "," + di.y + ")");
				dij.Add (new KeyValuePair<int ,mat> (di.d, di));
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.y = y1;
		}
//		Debug.Log("x="+di.x+"y="+di.y);
		if (di.y != 0) {
			di.y = y1 - 1;
			if (matrix [di.x, di.y] != 1 && dijvismatrix [di.x, di.y] == 1000&&dijExpmatrix [di.x, di.y] == 0) {
				dij.Add (new KeyValuePair<int ,mat> (di.d, di));
				Debug.Log ("Addded (" + di.x + "," + di.y + ")");
				dijExpmatrix [di.x, di.y] = 1;
			}
			di.y = y1;
		}
//		Debug.Log(dij.Count);
//		yield return StartCoroutine(Sec1());
		//theButtonsArray [m.x, m.y].colors = theColorOrange;
		while (dij.Count!=0) {
			mat k = dij.DequeueValue ();
			yield return StartCoroutine(Dijkstra(k));
		}
	}
	//public AudioSource beep;
	//float originalPitch;
	void Start () {
//		beep.Play();
	//	originalPitch=1f;
		Selection = true;
		source = true;
		destination = true;
		changeColor = false;
		X = 0;
		Y = 0;
		theColorBlack.highlightedColor = Color.black;
		theColorBlack.normalColor = Color.black;
		theColorBlack.pressedColor = Color.black;
		theColorBlue.highlightedColor = Color.black;
		theColorBlue.normalColor = Color.blue;
		theColorBlue.pressedColor = Color.blue;
		theColorBlue.disabledColor = Color.blue; 
		theColorBlue.highlightedColor = Color.blue;
		theColorBlue.colorMultiplier = 5f;
		theColorRed.highlightedColor = Color.red;
		theColorRed.normalColor = Color.red;
		theColorRed.pressedColor = Color.red;
		theColorRed.colorMultiplier = 5f;
		theColorWhite.highlightedColor = Color.white;
		theColorWhite.normalColor = Color.white;
		theColorWhite.pressedColor = Color.white;
		theColorWhite.highlightedColor = Color.white;
		theColorWhite.colorMultiplier = 5f;
		theColorGreen.highlightedColor = Color.green;
		theColorGreen.normalColor = Color.green;
		theColorGreen.pressedColor = Color.green;
		theColorGreen.highlightedColor = Color.green;
		theColorGreen.colorMultiplier = 5f;
		theColorOrange.highlightedColor = Color.yellow;
		theColorOrange.normalColor = Color.yellow;
		theColorOrange.pressedColor = Color.yellow;
		theColorOrange.highlightedColor = Color.yellow;
		theColorOrange.colorMultiplier = 5f;
		//theColorGrey.highlightedColor = Color.grey;
		//theColorGrey.normalColor = Color.grey;
		//theColorGrey.pressedColor = Color.grey;
		//theColorGrey.highlightedColor = Color.grey;
		theColorGrey.colorMultiplier = 5f;
		theColorGrey.fadeDuration = 1f;
		//theColorGrey.disabledColor = Color.grey;
//		theColor = GetComponent<Button> ().black;
		for (int i = 0; i < 14; i++) {
			for (int j = 0; j < 7; j++) {
				matrix [j, i] = 0;
				dijvismatrix [j, i] = 1000;
				dijExpmatrix [j, i] = 0;
			}
			theButtonsArray [0,i] = b1 [i];
			theButtonsArray [1,i] = b2 [i];
			theButtonsArray [2,i] = b3 [i];
			theButtonsArray [3,i] = b4 [i];
			theButtonsArray [4,i] = b5 [i];
			theButtonsArray [5,i] = b6 [i];
			theButtonsArray [6,i] = b7 [i];
		}
//		for (int i = 0; i < 7; i++)
//			for (int j = 0; j < 14; j++)
//				theButtonsArray [i, j].colors = theColorBlack;
	}
	
	// Update is called once per frame
	void Update () {
		//for(float i=0;i<10;i+=Time.deltaTime)
		//	beep.Play ();
//		beep.pitch -= Time.deltaTime * 3f/ 1.05f;
//		Debug.Log ("updateCalled");
//		Debug.Log ("x="+X); 

		if (changeColor) {
			if (source) {
				Debug.Log (theColorBlue.disabledColor);
				source = false;
				theButtonsArray [X, Y].colors = theColorBlue;
				Xso = X;
				Yso = Y;
				matrix [X, Y] = 2;
				text.text = "Select The Destination";
//				EditorApplication.Beep ();

			} else if (destination) {
//				EditorApplication.Beep ();
				Xdes = X;
				Ydes = Y;
				destination = false;
				theButtonsArray [X, Y].colors = theColorRed;
				matrix [X, Y] = 3;
				text.text = "Now Select the Obstacles";
			} else if (Selection) {
				theButtonsArray [X, Y].colors = theColorBlack;
				matrix [X, Y] = 1;
				text.text = "Select More Obstacles or Algorithm for PathFinding";
			}
				
			changeColor = false;
		}
	}
	public void SelectX(int i){
		X = i;
//		changeColor = true;
	}
	public void SelectY(int i){
		Y = i;
		changeColor = true;
	}

}
