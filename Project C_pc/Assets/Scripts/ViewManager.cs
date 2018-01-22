using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour {

    //界面
    public GameObject startPanel;
	public Text welcomeMsg;
	public GameObject registerPanel;

    //游戏界面
    public GameObject playPanel;
    public Text scoreText;
    public Text gradeText;

    //音乐
    private PlayMusic _playMusic;

    //结算
    public GameObject coverFail;
    public Text failText;
    public GameObject coverWin;
    public Text winText;
	public GameObject coverNotice;

	//本地排名积分
	public Text localScore;

	//辅助
	public Button ResetOne;
	public LevelShow _levelShow;

	//动画
	private MyTween myTween;

    void Start(){
		registerPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		startPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		playPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		_playMusic = transform.GetComponentInParent<PlayMusic> ();

		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);


		if (DataManager.AccountId>0) {
			GoToStartPanel ();
		} else {
			GoToRegisterPanel ();
		}
    }
		
	public void GoToStartPanel(){
		myTween.SlideIn (startPanel.transform);
		myTween.SlideOut (registerPanel.transform);
		myTween.SlideOut (playPanel.transform);

		_playMusic.PlayBg ("startMenuBg");

		DataManager _data = GetComponent<DataManager> ();
		welcomeMsg.text = "恭迎" + "\t" + _data.PlayerCountry + "\t" 
			+ _data.PlayerSchool + "\n" + _data.PlayerName + "\n道友大驾光临!";
	}

	public void GoToRegisterPanel(){
		myTween.SlideIn (registerPanel.transform);
		_playMusic.PlayBg ("startMenuBg");
	}

	public void OnFailReturnButton()
	{
		myTween.SlideIn (startPanel.transform);
		myTween.SlideOut (playPanel.transform);

		coverFail.gameObject.SetActive (false);
	}
        

	public void GoToGamePanel(){

		myTween.SlideIn (playPanel.transform);
		myTween.SlideOut (startPanel.transform);


		//设置游戏面板
		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
	}

	public void SetScore(int score){
		scoreText.text = score.ToString();
	}

	public void SetGrade(string grade){
		gradeText.text = grade;
	}

    public void WinMsg(int score,int localRank){
		coverWin.gameObject.SetActive (true);
        winText.text = score + ";" + localRank ;
	}

    public void FailMsg(int maxLv,int score,int localRank){
		coverFail.gameObject.SetActive (true);
        failText.text = maxLv + ";" + score + ";" + localRank + ";" ;
	}

	public void ShowAdNotce(){
		coverNotice.gameObject.SetActive (true);
	}

	public void HideAdNotice(){
		coverNotice.gameObject.SetActive (false);
	}

	public void UpdateLocalRank(int s,string g){
		if (s > 0)
			localScore.text = "个人最高修为：" + g + "(" + s + ")";
		else
			localScore.text = "";	
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

	public void Upgrade(int newLevel){
		_levelShow.UpdateLevelShow (newLevel);
		if (newLevel >= 5) {
			//ToDo 特效
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
