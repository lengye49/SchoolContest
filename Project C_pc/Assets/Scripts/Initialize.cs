using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Advertisements;
using DG.Tweening;

public class Initialize : MonoBehaviour {

	public Text scoreText;
	public Text gradeText;
	public GameObject coverFail;
	public Text failText;
	public GameObject coverWin;
	public Text winText;
	public Text upgradeText;
	public Text addScoreText;

	private Object o;
	private GameObject[][] cells = new GameObject[5][];
	private int[][] nums = new int[5][];
	private bool[][] allCellsCheck = new bool[5][];
	private Vector3 offsetPos = new Vector3 (0, 100, 0);

	private ArrayList sameNumIndex;
	private ArrayList unCheckNeighbour;
	private int seed;
	private int midNum = 2;
	private int totalNum;
	private int score = 0;
	private PlayMusic _playerMusic;
	private int maxLv;

	private int highScore1;
	private int highScore2;
	private int highScore3;
	private int highLv1;
	private int highLv2;
	private int highLv3;
	public Text score1;
	public Text score2;
	public Text score3;

	void Start () {
		_playerMusic = this.gameObject.GetComponentInParent<PlayMusic> ();
		_playerMusic.PlayBg ("startMenuBg");
		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
		o=Resources.Load("Cell");
		GetScore ();
		upgradeText.gameObject.SetActive (false);
		addScoreText.gameObject.SetActive (false);
	}

	void GetScore(){
		highScore1 = PlayerPrefs.GetInt ("highScore1", 0);
		highLv1 = PlayerPrefs.GetInt ("highLv1", 1);
		highScore2 = PlayerPrefs.GetInt ("highScore2", 0);
		highLv2 = PlayerPrefs.GetInt ("highLv2", 1);
		highScore3 = PlayerPrefs.GetInt ("highScore3", 0);
		highLv3 = PlayerPrefs.GetInt ("highLv3", 1);
		DisplayScore ();
	}

	int SetScore(int newScore){
		if ((maxLv > highLv1) || (maxLv == highLv1 && newScore > highScore1)) {
			highLv3 = highLv2;
			highLv2 = highLv1;
			highLv1 = maxLv;
			highScore3 = highScore2;
			highScore2 = highScore1;
			highScore1 = score;
			StoreData ();
			DisplayScore ();
			return 1;
		} else if ((maxLv > highLv1) || (maxLv == highLv2 && newScore > highScore2)) {
			highLv3 = highLv2;
			highLv2 = maxLv;
			highScore3 = highScore2;
			highScore2 = score;
			StoreData ();
			DisplayScore ();
			return 2;
		} else if ((maxLv > highLv3) || (maxLv == highLv3 && newScore > highScore3)) {
			highLv3 = maxLv;
			highScore3 = score;
			StoreData ();
			DisplayScore ();
			return 3;
		} else {
			return 0;
		}
	}

	void DisplayScore(){
		score1.text = "No.1：" + GetGradeByLv (highLv1) + ", " + highScore1.ToString () + "分";
		score2.text = "No.2：" + GetGradeByLv (highLv2) + ", " + highScore2.ToString () + "分";
		score3.text = "No.3：" + GetGradeByLv (highLv3) + ", " + highScore3.ToString () + "分";
	}

	string GetGradeByLv(int lv){
		string s = "一年级";
		switch (lv) {
		case 1:
			s = "练气";
			break;
		case 2:
			s = "筑基";
			break;
		case 3:
			s = "金丹";
			break;
		case 4:
			s = "元婴";
			break;
		case 5:
			s = "出窍";
			break;
		case 6:
			s = "分神";
			break;
		case 7:
			s = "合体";
			break;
		case 8:
			s = "洞虚";
			break;
		case 9:
			s = "大成";
			break;
		case 10:
			s = "渡劫";
			break;
		case 11:
			s = "升仙";
			break;
		default:
			s = "升仙";
			break;
		}
		return s;
	}

	void StoreData(){
		PlayerPrefs.SetInt ("highScore1", highScore1);
		PlayerPrefs.SetInt ("highLv1", highLv1);
		PlayerPrefs.SetInt ("highScore2", highScore2);
		PlayerPrefs.SetInt ("highLv2", highLv2);
		PlayerPrefs.SetInt ("highScore3", highScore3);
		PlayerPrefs.SetInt ("highLv3", highLv3);
	}

	public void InitializeCells()
	{
		_playerMusic.PlayBg ("playPanelBg");
		coverFail.gameObject.SetActive (false);

		score = 0;
		scoreText.text = "0";
		maxLv = 1;

		System.Random r = new System.Random();

		cells[0] = new GameObject[3];
		cells[1] = new GameObject[4];
		cells[2] = new GameObject[5];
		cells[3] = new GameObject[4];
		cells[4] = new GameObject[3];

		nums [0] = new int[3];
		nums [1] = new int[4];
		nums [2] = new int[5];
		nums [3] = new int[4];
		nums [4] = new int[3];

		allCellsCheck [0] = new bool[3];
		allCellsCheck [1] = new bool[4];
		allCellsCheck [2] = new bool[5];
		allCellsCheck [3] = new bool[4];
		allCellsCheck [4] = new bool[3];
	
		for (int i=0; i<cells.Length; i++) {
			for (int j=0; j<cells[i].Length; j++) {
				GameObject g = Instantiate (o) as GameObject;
				g.gameObject.transform.SetParent (this.gameObject.transform);
				g.transform.localPosition = offsetPos+ new Vector3 ((float)(j - (0.5 * cells [i].Length - 0.5)) * 84.5f, (midNum - i) * 71f, 0f);
				g.name = i.ToString () + "," + j.ToString ();
				cells [i] [j] = g;

				int n = r.Next (0, 3);
				maxLv = (maxLv > n + 1) ? maxLv : n + 1;
				int num = (int)Mathf.Pow (3, n);
				nums [i] [j] = num;
				g.GetComponent<Image> ().color = GetColorByNum (num);
				string s = GetTxtFromNum (num);
				g.GetComponentInChildren<Text> ().text = s;
			}
		}
		gradeText.text = GetGradeByLv (maxLv);
	}
		
	string GetTxtFromNum(int num){
		string s = "练气";
		switch (num) {
		case 1:
			s = "练气";
			break;
		case 3:
			s = "筑基";
			break;
		case 9:
			s = "金丹";
			break;
		case 27:
			s = "元婴";
			break;
		case 81:
			s = "出窍";
			break;
		case 243:
			s = "分神";
			break;
		case 729:
			s = "合体";
			break;
		case 2187:
			s = "洞虚";
			break;
		case 6561:
			s = "大成";
			break;
		case 19684:
			s = "渡劫";
			break;
		case 59049:
			s = "升仙";
			break;	
		default:
			s = "升仙";
			break;
		}
		return s;
	}


	public void ButtonClickCal(int row,int column)
	{
		CheckThisCell (row, column);
		if (totalNum >= 3) {
			_playerMusic.PlayerSound ("success");
			Calculate (row, column);
			CheckGameOver ();
		} else {
			_playerMusic.PlayerSound ("click");
		}
	}

	void CheckThisCell(int row,int column)
	{
		ResetArrays ();
		allCellsCheck [row] [column] = true;
		unCheckNeighbour.Add (new int[]{row,column});
		seed = nums [row] [column];
		totalNum = 1;

		while (unCheckNeighbour.Count>0) {
			int[] ins = unCheckNeighbour[0] as int[];
			CheckNeighbour(ins[0],ins[1]);
			unCheckNeighbour.RemoveAt(unCheckNeighbour.IndexOf(ins));
		}
	}

	void CheckGameOver()
	{
		if (maxLv > 9) {
			Debug.Log ("GameOver Win!");
			coverWin.gameObject.SetActive (true);
			_playerMusic.PlayerSound ("win");
			int n = SetScore (score);
			string win = "游戏通关!\n 你本局的积分是 " + score.ToString () + "! \n你已经成为超级学霸！\n努力吧，少年！新世界的大门已经为你打开！";
			if (n > 0)
				win += "\n你当前的排名是第" + n + "名！";
			winText.text = win;
		} else {
			for (int i = 0; i < cells.Length; i++) {
				for (int j = 0; j < cells [i].Length; j++) {
					CheckThisCell (i, j);
					if (totalNum >= 3)
						return;
				}
			}
		}
		Debug.Log ("GameOver");
		coverFail.gameObject.SetActive (true);
		_playerMusic.PlayerSound ("fail");
		int l = SetScore (score);
		string fail = "没有三个相连的等级，游戏结束!\n 你本局的积分是 " + score.ToString () + "! \n你已经处于"+GetGradeByLv(maxLv)+"学霸的水平，再接再厉哦!";
		if (l > 0)
			fail += "\n你当前的排名是第" + l + "名！";
		failText.text = fail;

	}

	void Calculate(int row, int column)
	{
		if (totalNum >= 3) {
			
			score += seed * totalNum;
			FloatingUpgrade (addScoreText, 1, seed * totalNum);
			scoreText.text = score.ToString ();
			int newN = (int)(Mathf.Log (totalNum, 3f));

			if ((maxLv < (newN + 1 + (int)(Mathf.Log (seed, 3f))))) {
				maxLv = (newN + 1+(int)(Mathf.Log(seed,3f)));
				if (maxLv >= 5)
					FloatingUpgrade (upgradeText, 0, maxLv);
			}

			int newSeed = seed * (int)Mathf.Pow (3, newN);
			nums [row] [column] = newSeed;
			string s = GetTxtFromNum (newSeed);
			Debug.Log (newSeed + ", " + s);
			cells[row][column].gameObject.GetComponentInChildren<Text>().text = s;
			cells [row] [column].gameObject.GetComponent<Image> ().color = GetColorByNum (newSeed);

			System.Random r = new System.Random();
			for(int i=0;i<sameNumIndex.Count;i++)
			{
				int[] ins = sameNumIndex[i] as int[];
				int n = r.Next (0, 3);
				if (maxLv < n + 1) {
					maxLv = (n + 1);
					if (maxLv >= 5)
						FloatingUpgrade (upgradeText, 0, maxLv);
				}
				int nn = (int)Mathf.Pow (3,n);
				nums [ins[0]] [ins[1]] = nn;
				s = GetTxtFromNum (nn);
				cells[ins[0]] [ins[1]].gameObject.GetComponentInChildren<Text>().text = s;
				cells [ins [0]] [ins [1]].gameObject.GetComponent<Image> ().color = GetColorByNum (nn);
			}
			gradeText.text = GetGradeByLv (maxLv);
		}
	}

	void ResetArrays()
	{
		sameNumIndex = new ArrayList ();

		for (int i=0; i<allCellsCheck.Length; i++) {
			for (int j=0; j<allCellsCheck[i].Length; j++) {
				allCellsCheck[i][j]=false;
			}
		}
		unCheckNeighbour = new ArrayList ();
	}

	void CheckNeighbour(int row, int column)
	{
		ArrayList ns = GetNeighbourhood (row, column);
		for (int i=0; i<ns.Count; i++) {
			int[] ins = ns[i] as int[];
			if(nums[ins[0]][ins[1]] == seed)
			{
				sameNumIndex.Add(ins);
				unCheckNeighbour.Add (ins);
				totalNum++;
			}
			allCellsCheck[ins[0]][ins[1]]=true;
		}
	}

	ArrayList GetNeighbourhood(int row, int column)
	{
		ArrayList ns = new ArrayList ();

		if (row < midNum) {
				ns.Add (new int[]{row - 1,column - 1});
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column });
				ns.Add (new int[]{row + 1,column + 1});
		} else if (row == midNum) {
				ns.Add (new int[]{row - 1,column - 1});
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column - 1 });
				ns.Add (new int[]{row + 1,column });
		} else {
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row - 1,column + 1});
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column - 1 });
				ns.Add (new int[]{row + 1,column });
		}

		//i++ will change ns
		for (int i=ns.Count-1; i>=0; i--) {
			int[] a = ns [i] as int[];
			if (a [0] >= allCellsCheck.Length || a [0] < 0 || a [1] >= allCellsCheck [a [0]].Length || a [1] < 0 || allCellsCheck [a [0]] [a [1]])
				ns.Remove (ns [i]);
		}
		return ns;
	}

	Color GetColorByNum(int num)
	{
		switch (num) {
		case 1:
			return new Color32 (192, 192, 192, 255); //Grey
		case 3:
			return new Color32 (255, 250, 250, 255); //SnowWhite
		case 9:
			return new Color32 (0, 250, 154, 255); 	 //MedSpringGreen
		case 27:
			return new Color32 (0, 191, 255, 255);	 //DeepSkyBlue
		case 81:
			return new Color32 (147, 112, 219, 255); //MediumPurple
		case 243:
			return new Color32 (255, 215, 0, 255);	 //Gold
		case 729:
			return new Color32 (255, 0, 0, 255);	 //Red
		case 2187:
			return new Color32 (139, 69, 19, 255);	 //SaddleBrown
		case 6561:
			return new Color32 (0, 0, 0, 255);		 //Black
		default:
			return Color.black;
		}
	}

	/// <summary>
	/// Floatings the upgrade.
	/// </summary>
	/// <param name="t">T.</param>
	/// <param name="type">Type 0upgrade,1score.</param>
	/// <param name="param">Parameter.</param>
	void FloatingUpgrade(Text t,int type,int param){
		Vector3 startPosition;
		int y1;
		int y2;
		if (type == 0) {
			t.text = "恭喜你超越了" + GetGradeByLv (param - 1) + "学霸，达到" + GetGradeByLv (param) + "水平！";
			startPosition = new Vector3 (25, 220, 0);
			y1 = 280;
			y2 = 320;
		} else {
			startPosition = new Vector3 (250, 280, 0);
			y1 = 300;
			y2 = 320;
			t.text = "积分 +" + param;
		}
		t.gameObject.SetActive (true);
		t.gameObject.transform.localPosition = startPosition;
		t.gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
		t.gameObject.transform.DOLocalMoveY (y1, 0.5f);
		t.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f), 0.5f);
		t.DOFade (1, 0.5f);
		StartCoroutine (WaitAndNext (t, y2));
	}
	IEnumerator WaitAndNext(Text t,int y2){
		yield return new WaitForSeconds (1.5f);
		EndFloatUpgrade (t,y2);
	}
	void EndFloatUpgrade(Text t,int y2){
		t.gameObject.transform.DOLocalMoveY (y2, 0.5f);
		t.DOFade (0, 0.5f);
		StartCoroutine (WaitAndEnd (t.gameObject));
	}

	IEnumerator WaitAndEnd(GameObject o){
		yield return new WaitForSeconds (0.5f);
		o.SetActive (false);
	}

}
