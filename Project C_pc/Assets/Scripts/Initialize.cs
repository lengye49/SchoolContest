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
	public DataManager _data;
	public GameObject playPanel;
	public GameObject startPanel;
	public GameObject failCover;
	public Button continueButton;
	public GameObject instructions;
	public GameObject instrCover;

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

	public Text score1;
	public Text score2;
	public Text score3;

	void Start () {
		
		_playerMusic = this.gameObject.GetComponentInParent<PlayMusic> ();

		o = Resources.Load("Cell");

        continueButton.interactable = (_data.HasMemory > 0);
	}

	public void OnStartGame(){
		InitData (true);
		GoToGamePanel ();
	}

	public void OnContinueGame(){
		InitData (false);
		GoToGamePanel ();
	}

	void GoToGamePanel(){
		startPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		playPanel.transform.localPosition = Vector3.zero;
		
        //设置游戏面板
        _playerMusic.PlayBg ("playPanelBg");
        instructions.SetActive (false);
        instrCover.SetActive (false);
        upgradeText.gameObject.SetActive (false);
        addScoreText.gameObject.SetActive (false);
        scoreText.text = score.ToString();
        gradeText.text = _data.GetGradeByLevel (maxLv);

        //根据数据初始化格子
        //Todo
	}
        

	/// <summary>
	///初始化数据
	/// </summary>
	/// <param name="isStart">True:重新开始 False:继续游戏</param></param>
	void InitData(bool isStart){
        if (isStart)
        {
            score = 0;
            maxLv = 1;

            nums [0] = new int[3];
            nums [1] = new int[4];
            nums [2] = new int[5];
            nums [3] = new int[4];
            nums [4] = new int[3];

            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < nums[i].Length; j++)
                {
                    int n = Random.Range (0, 3);
                    int num = (int)Mathf.Pow (3, n);
                    nums [i] [j] = num;
                    maxLv = (maxLv > n + 1) ? maxLv : n + 1;
                }
            }
            StoreData();
        }
        else
        {
            score = _data.Score;
            maxLv = _data.Level;
            nums = _data.NumList;
        }
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
                g.GetComponent<Image> ().color = GetColorByNum (nums[i][j]);
                string s = _data.GetGradeByScore (nums[i][j]);
                g.GetComponentInChildren<Text> ().text = s;
            }
        }
    }

    public void ClickCell(int row,int column)
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
        
    void StoreData(){
        _data.Score = score;
        _data.Level = maxLv;
        _data.NumList = nums;
        _data.HasMemory = 1;
    }

	void CheckGameOver()
	{
		if (maxLv > 11) {
			coverWin.gameObject.SetActive (true);
			_playerMusic.PlayerSound ("win");
			string win = "游戏通关!\n 你本局的积分是 " + score.ToString () + "！";
            _data.HasMemory = 0;
			win += UpdateRank();
			winText.text = win;
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
        _data.HasMemory = 0;
		coverFail.gameObject.SetActive (true);
		_playerMusic.PlayerSound ("fail");
		string fail = "没有三个相连的等级，游戏结束!\n 你本局的积分是 " + score.ToString () + "! \n你已经处于"+_data.GetGradeByLevel(maxLv)+"学霸的水平，再接再厉哦!";
		fail += UpdateRank ();
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
			string s = _data.GetGradeByScore (newSeed);

			cells[row][column].gameObject.GetComponentInChildren<Text>().text = s;
			cells [row] [column].gameObject.GetComponent<Image> ().color = GetColorByNum (newSeed);

			for(int i=0;i<sameNumIndex.Count;i++)
			{
				int[] ins = sameNumIndex[i] as int[];
                int n = Random.Range (0, 3);
				if (maxLv < n + 1) {
					maxLv = (n + 1);
					if (maxLv >= 5)
						FloatingUpgrade (upgradeText, 0, maxLv);
				}
				int nn = (int)Mathf.Pow (3,n);
				nums [ins[0]] [ins[1]] = nn;
				s = _data.GetGradeByScore (nn);
				cells[ins[0]] [ins[1]].gameObject.GetComponentInChildren<Text>().text = s;
				cells [ins [0]] [ins [1]].gameObject.GetComponent<Image> ().color = GetColorByNum (nn);
			}
			gradeText.text = _data.GetGradeByLevel (maxLv);
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
		
	void FloatingUpgrade(Text t,int type,int param){
		Vector3 startPosition;
		int y1;
		int y2;
		if (type == 0) {
			t.text = "恭喜你超越了" + _data.GetGradeByLevel (param - 1) + "学霸，达到" + _data.GetGradeByLevel (param) + "水平！";
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


    string UpdateRank(){
        int n = _data.SetHighScore ();
        if (n > 0) {
            UpdateRankPanel ();
            return "\n你当前的排名是第" + n + "名！";
        }else
            return "";
    }

    void UpdateRankPanel(){
        score1.text = "No.1：" + _data.GetGradeByLevel (_data.HighLevel1) + ", " + _data.HighScore1.ToString () + "分";
        score2.text = "No.2：" + _data.GetGradeByLevel (_data.HighLevel2) + ", " + _data.HighScore2.ToString () + "分";
        score3.text = "No.3：" + _data.GetGradeByLevel (_data.HighLevel3) + ", " + _data.HighScore3.ToString () + "分";
    }

}
