using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewManager : MonoBehaviour {

    //开始界面
    public GameObject startPanel;

    //游戏界面
    public GameObject playPanel;
    public Text scoreText;
    public Text gradeText;
    public GameObject instructions;
    public GameObject instrCover;

    //游戏辅助
    public Text upgradeText;
    public Text addScoreText;
    private PlayMusic _playMusic;

    //结算界面
    public GameObject coverFail;
    public Text failText;
    public GameObject coverWin;
    public Text winText;

	//本地排名积分
	public Text localScore1;
	public Text localScore2;
	public Text localScore3;

	//等级图
//	public Image[] gradeImages;

    void Start(){
        startPanel.transform.localPosition = Vector3.zero;
        playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);

        _playMusic = transform.GetComponentInParent<PlayMusic> ();
        _playMusic.PlayBg ("startMenuBg");

        coverFail.gameObject.SetActive (false);
        coverWin.gameObject.SetActive (false);
		instructions.SetActive (false);
		instrCover.SetActive (false);

    }
		

	public void OnFailReturnButton()
	{
		playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		startPanel.transform.localPosition = Vector3.zero;
		coverFail.gameObject.SetActive (false);
	}

	public void OnInstrButton(){
		instrCover.SetActive (true);
		instructions.SetActive (true);
		instructions.transform.localPosition = Vector3.zero;
		instructions.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
		instructions.transform.DOBlendableScaleBy (new Vector3 (0.8f, 0.8f, 1f), 0.5f);
	}

	public void OnInstrCover(){
		instrCover.SetActive (false);
		instructions.SetActive (false);
	}

	public void GoToGamePanel(){
		startPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		playPanel.transform.localPosition = Vector3.zero;

		//设置游戏面板
		instructions.SetActive (false);
		instrCover.SetActive (false);
		upgradeText.gameObject.SetActive (false);
		addScoreText.gameObject.SetActive (false);
		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
	}

	public void SetScore(int score){
		scoreText.text = score.ToString();
	}

	public void SetGrade(string grade){
		gradeText.text = grade;
	}

	public void WinMsg(string msg){
		coverWin.gameObject.SetActive (true);
		winText.text = msg;
	}

	public void FailMsg(string msg){
		coverFail.gameObject.SetActive (true);
		failText.text = msg;
	}

	public void UpdateLocalRank(int s1,int s2,int s3,string g1,string g2,string g3){
		if(s1>0)
			localScore1.text = "首座：" + g1 + ", " + s1 + "分";
		else
			localScore1.text = "首座：虚位以待";	

		if (s2 > 0)
			localScore2.text = "次席：" + g2 + ", " + s2 + "分";
		else
			localScore2.text = "次席：虚位以待";

		if (s3 > 0)
			localScore3.text = "第三：" + g3 + ", " + s3 + "分";
		else
			localScore3.text = "第三：虚位以待";
	}

	public void UpgradeFloating(string msg){
		Vector3 startPosition = new Vector3 (25, 0, 0);;
		float y1 = 60f;
		float y2 = 100f;
		upgradeText.text = msg;
		PopMsg (upgradeText.gameObject, startPosition, y1, y2);
	}

	public void AddScoreFloating(string msg){
		Vector3 startPosition =  new Vector3 (250, 280, 0);
		float y1 = 300;
		float y2 = 320f;
		addScoreText.text = msg;
		PopMsg (addScoreText.gameObject, startPosition, y1, y2);
	}

	void PopMsg(GameObject o,Vector3 startPos,float y1,float y2){
		o.SetActive (true);
		o.transform.localPosition = startPos;
		o.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
		o.transform.DOLocalMoveY (y1, 0.5f);
		o.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f), 0.5f);
		o.GetComponent<Text> ().DOFade (1, 0.5f);
		StartCoroutine (PopUp (o, y2));
	}

	IEnumerator PopUp(GameObject o,float y2){
		yield return new WaitForSeconds (1.5f);
		PopEnd (o,y2);
	}

	void PopEnd(GameObject o,float y2){
		o.transform.DOLocalMoveY (y2, 0.5f);
		o.GetComponent<Text> ().DOFade (0, 0.5f);
		StartCoroutine (Vanish (o));
	}

	IEnumerator Vanish(GameObject o){
		yield return new WaitForSeconds (0.5f);
		o.SetActive (false);
	}
}
