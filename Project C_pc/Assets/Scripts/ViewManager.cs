using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewManager : MonoBehaviour {

    //界面
    public GameObject startPanel;
	public Text welcomeMsg;
	public GameObject registerPanel;

    //游戏界面
    public GameObject playPanel;
    public Text scoreText;
    public Text gradeText;
    public GameObject instructions;
//    public GameObject instrCover;

    //音乐
    private PlayMusic _playMusic;

    //结算
    public GameObject coverFail;
    public Text failText;
    public GameObject coverWin;
    public Text winText;
	public GameObject coverNotice;

	//本地排名积分
	public Text localScore1;
	public Text localScore2;
	public Text localScore3;

	//辅助
	public Button ResetOne;

    void Start(){
		registerPanel.transform.localPosition = new Vector3 (5000f, 0, 0);
		startPanel.transform.localPosition = new Vector3 (5000f, 0, 0);
		playPanel.transform.localPosition = new Vector3 (5000f, 0, 0);
		_playMusic = transform.GetComponentInParent<PlayMusic> ();

		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
		instructions.SetActive (false);
//		instrCover.SetActive (false);

		if (DataManager.AccountExist) {
			GoToStartPanel ();
		} else {
			GoToRegisterPanel ();
		}
    }
		
	public void GoToStartPanel(){
		startPanel.transform.DOLocalMoveX (0, 0.5f);
		registerPanel.transform.DOLocalMoveX (5000f, 0.5f);
		playPanel.transform.DOLocalMoveX (5000f, 0.5f);
		_playMusic.PlayBg ("startMenuBg");

		DataManager _data = GetComponent<DataManager> ();
		welcomeMsg.text = "恭迎" + "\t" + _data.PlayerCountry + "\t" 
			+ _data.PlayerSchool + "\n" + _data.PlayerName + "\n道友大驾光临!";
	}

	public void GoToRegisterPanel(){
		registerPanel.transform.DOLocalMoveX (0, 0.5f);
//		startPanel.transform.DOLocalMoveX (2000f, 0.5f);
//		playPanel.transform.DOLocalMoveX (2000f, 0.5f);
		_playMusic.PlayBg ("startMenuBg");
	}

	public void OnFailReturnButton()
	{
		playPanel.transform.localPosition = new Vector3 (5000f, 0, 0);
		startPanel.transform.localPosition = Vector3.zero;
		coverFail.gameObject.SetActive (false);
	}

	public void OnInstrButton(){
//		instrCover.SetActive (true);
		instructions.SetActive (true);
		instructions.transform.localPosition = Vector3.zero;
		instructions.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
		instructions.transform.DOBlendableScaleBy (new Vector3 (0.8f, 0.8f, 1f), 0.5f);
	}

	public void OnInstrCover(){
//		instrCover.SetActive (false);
		instructions.SetActive (false);
	}

	public void GoToGamePanel(){
		startPanel.transform.localPosition = new Vector3 (5000f, 0, 0);
		playPanel.transform.localPosition = Vector3.zero;

		//设置游戏面板
		instructions.SetActive (false);
//		instrCover.SetActive (false);
		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
	}

	public void SetScore(int score){
		scoreText.text = score.ToString();
	}

	public void SetGrade(string grade){
		gradeText.text = grade;
	}

    public void WinMsg(int score,int localRank,int onlineRank){
		coverWin.gameObject.SetActive (true);
        winText.text = score + ";" + localRank + ";" + onlineRank;
	}

    public void FailMsg(int maxLv,int score,int localRank,int onlineRank){
		coverFail.gameObject.SetActive (true);
        failText.text = maxLv + ";" + score + ";" + localRank + ";" + onlineRank;
	}

	public void ShowAdNotce(){
		coverNotice.gameObject.SetActive (true);
	}

	public void HideAdNotice(){
		coverNotice.gameObject.SetActive (false);
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

	public void SetResetOneState(bool hasEnergy){
		ResetOne.interactable = hasEnergy;
	}

	public void ResetOneOn(bool isOn){
	
		if (isOn) {
			Debug.Log ("开启仙人指路");
			//特效打开
		} else {
			Debug.Log ("关闭仙人指路");
			//特效关闭
		}
	}

	public void OnShowAd(){
		HideAdNotice ();
		//ShowAd
		//Todo...

		//测试时直接完成广告
		CompleteAd();

	}

	void CompleteAd(){
		GetComponentInChildren<Initialize> ().ResetAllCells ();
	}

	public void OnCancelAd(){
		HideAdNotice ();
		GetComponentInChildren<Initialize> ().ConfirmComplete ();
	}
		
}
