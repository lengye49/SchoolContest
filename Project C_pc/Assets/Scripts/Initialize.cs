using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Initialize : MonoBehaviour {

	private DataManager _data;
	private ViewManager _view;
	private Warning _warning;
	private Object o;
	private GameObject[][] cells = new GameObject[5][];
	private int[][] nums = new int[5][];
	private bool[][] allCellsCheck = new bool[5][];
	private Vector3 offsetPos = new Vector3 (0, -300, 0);

	private ArrayList sameNumIndex;
	private ArrayList unCheckNeighbour;
	private ArrayList listCells;
	private int seed;
	private int midNum = 2;
	private int totalNum;
	private int score = 0;
	private PlayMusic _playerMusic;
	private int maxLv;

	private bool hasResetEnergy = false;
	private bool isResetCell = false;
	private bool isAdDone = false;

	void Start () {
		_data = GetComponentInParent<DataManager> ();
		_view = GetComponentInParent<ViewManager> ();
		_warning = GetComponentInParent<Warning> ();

		_playerMusic = this.gameObject.GetComponentInParent<PlayMusic> ();
		o = Resources.Load("Cell");
		listCells = new ArrayList ();
	}

	public void OnGameStart(){
		InitData ();
		StartGame ();
	}

	void StartGame(){
		for (int i = 0; i < listCells.Count; i++) {
			DestroyImmediate (listCells [i] as GameObject);
		}
		_playerMusic.PlayBg ("playPanelBg");
		_view.GoToGamePanel ();
		_view.SetScore (score);
		_view.SetGrade (_data.GetGradeByLevel (maxLv));
		isResetCell = false;
		hasResetEnergy = false;
		isAdDone = false;
		_view.SetResetOneState (false);
		_view.ResetOneOn (isResetCell);

		UpdateRankPanel ();

		InitCell ();
	}

	/// <summary>
	///初始化数据
	/// </summary>
	/// </param></param>
	void InitData(){

		score = 0;
		maxLv = 1;

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

		for (int i = 0; i < nums.Length; i++) {
			for (int j = 0; j < nums [i].Length; j++) {
				int n = Random.Range (0, 3);
				int num = (int)Mathf.Pow (3, n);
				nums [i] [j] = num;
				maxLv = (maxLv > n + 1) ? maxLv : n + 1;
			}
		}
		StoreData ();
		_view.Upgrade (maxLv);
	}

    /// <summary>
    /// 初始化格子
    /// </summary>
    public void InitCell()
    {
        cells[0] = new GameObject[3];
        cells[1] = new GameObject[4];
        cells[2] = new GameObject[5];
        cells[3] = new GameObject[4];
        cells[4] = new GameObject[3];

        for (int i=0; i<cells.Length; i++) {
            for (int j=0; j<cells[i].Length; j++) {
                GameObject g = Instantiate (o) as GameObject;
                g.gameObject.transform.SetParent (this.gameObject.transform);
//                g.transform.localPosition = offsetPos+ new Vector3 ((float)(j - (0.5 * cells [i].Length - 0.5)) * 152.1f, (midNum - i) * 127.8f, 0f);
                g.transform.localPosition = offsetPos+ new Vector3 ((float)(j - (0.5 * cells [i].Length - 0.5)) * 172.1f, (midNum - i) * 138f, 0f);
                g.name = i.ToString () + "," + j.ToString ();
                cells [i] [j] = g;
				g.GetComponent<Image> ().color = _data.GetImageColor (nums[i][j]);
                string s = _data.GetGradeByScore (nums[i][j]);
                g.GetComponentInChildren<Text> ().text = s;
				g.GetComponentInChildren<Text> ().color = _data.GetTextColor (nums [i] [j]);
				listCells.Add (g);
            }
        }
    }

    public void ClickCell(int row,int column)
    {
		if (isResetCell) {
			_playerMusic.PlayerSound ("success");
			GenerateNewCell (row, column);
			isResetCell = false;
			hasResetEnergy = false;
			_view.SetResetOneState (hasResetEnergy);
			_view.ResetOneOn (false);

			CheckGameOver ();
		} else {
			CheckThisCell (row, column);
			if (totalNum >= 3) {
				_playerMusic.PlayerSound ("success");
				Calculate (row, column);
				CheckGameOver ();
			} else {
				_playerMusic.PlayerSound ("click");
			}
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
        
    void StoreData(){
        _data.Score = score;
        _data.Level = maxLv;
    }

	void CheckGameOver()
	{
		if (maxLv > 11) {
			//游戏通关
			_playerMusic.PlayerSound ("win");
			int localRank;
            int onlineRank;
            SettleRank(out localRank,out onlineRank);
			_view.WinMsg (score,localRank,onlineRank);
		} else {
			for (int i = 0; i < cells.Length; i++) {
				for (int j = 0; j < cells [i].Length; j++) {
					CheckThisCell (i, j);
                    if (totalNum >= 3)
                    {
                        StoreData();
                        return;
                    }
				}
			}
		}

		//游戏失败
		_playerMusic.PlayerSound ("fail");

		if (isAdDone) {
			ConfirmComplete ();
		} else {
			_view.ShowAdNotce ();
			isAdDone = true;
		}
	}



	void SetCell(int row,int column,int newSeed){
		nums [row] [column] = newSeed;
		string s = _data.GetGradeByScore (newSeed);
		cells[row][column].gameObject.GetComponentInChildren<Text>().text = s;
		cells [row] [column].gameObject.GetComponent<Image> ().color = _data.GetImageColor (newSeed);
		cells[row][column].gameObject.GetComponentInChildren<Text>().color = _data.GetTextColor (newSeed);
	}

	void GenerateNewCell(int row,int column){
		int max = Mathf.Max (maxLv - 4, 3);
		int min = Mathf.Max (maxLv - 7, 0);
		int n = Random.Range (min, max);
		if (maxLv < n + 1) {
			maxLv = (n + 1);
			_view.Upgrade (maxLv);
			if (maxLv >= 5) {
				hasResetEnergy = true;
				_view.SetResetOneState (hasResetEnergy);
			}
		}
		int newSeed = (int)Mathf.Pow (3,n);
		SetCell (row, column, newSeed);
	}

	void Calculate(int row, int column)
	{
		if (totalNum >= 3) {
			
			score += seed * totalNum;
			string msg = "真元 +" + seed * totalNum;
			_warning.ShowWarning (1, msg, new Vector3 (-345, 0, 0));

			int newN = (int)(Mathf.Log (totalNum, 3f));

			if ((maxLv < (newN + 1 + (int)(Mathf.Log (seed, 3f))))) {
				maxLv = (newN + 1+(int)(Mathf.Log(seed,3f)));
				_view.Upgrade (maxLv);
				if (maxLv >= 5) {
					hasResetEnergy = true;
					Debug.Log ("获得能量");
					_view.SetResetOneState (hasResetEnergy);
				}
			}

			int newSeed = seed * (int)Mathf.Pow (3, newN);
			SetCell (row, column, newSeed);

			//生成新的单元格
			for(int i=0;i<sameNumIndex.Count;i++)
			{
				int[] ins = sameNumIndex[i] as int[];
				GenerateNewCell (ins [0], ins [1]);
			}
			_view.SetScore (score);
			_view.SetGrade (_data.GetGradeByLevel (maxLv));
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
		

    void UpdateRankPanel(){
		int s1 = _data.HighScore1;
		int s2 = _data.HighScore2;
		int s3 = _data.HighScore3;

		string g1 = _data.GetGradeByLevel (_data.HighLevel1);
		string g2 = _data.GetGradeByLevel (_data.HighLevel2);
		string g3 = _data.GetGradeByLevel (_data.HighLevel3);

		_view.UpdateLocalRank (s1, s2, s3, g1, g2, g3);
    }

	/// <summary>
	/// 将各元素随机交换位置
	/// </summary>
	public void ResetAllCells(){
		for (int i = 0; i < 100; i++) {
			int row1 = Random.Range (0, 5);
			int column1 = Random.Range (0, nums [row1].Length);

			int row2 = Random.Range (0, 5);
			int column2 = Random.Range (0, nums [row2].Length);

			SwitchCell (row1, column1, row2, column2);
		}
		CheckGameOver ();
	}

	void SwitchCell(int r1,int c1,int r2,int c2){
		int t = nums [r1] [c1];
		nums [r1] [c1] = nums [r2] [c2];
		nums [r2] [c2] = t;
		SetCell (r1, c1, nums [r1] [c1]);
		SetCell (r2, c2, nums [r2] [c2]);
	}

	public void ChangeResetOneState(){
		if (!hasResetEnergy || isResetCell) {
			isResetCell = false;
		} else {
			isResetCell = true;
		}
		_view.ResetOneOn (isResetCell);
	}

	public void ConfirmComplete(){
		int localRank;
        int onlineRank;
        SettleRank(out localRank,out onlineRank);
		_view.FailMsg (maxLv,score,localRank,onlineRank);

	}

    void SettleRank(out int localRank,out int onlineRank){
        localRank = _data.SetHighScore ();
        onlineRank = 0;
        if (localRank == 1)
            onlineRank = _data.GetOnlineRank();

        UpdateRankPanel ();
    }
}
