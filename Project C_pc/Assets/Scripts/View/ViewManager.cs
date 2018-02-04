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

	//网络加载
	public static bool isLoading = false;
	public GameObject loadingCover;
	public GameObject loadingImage;

    void Start(){
		registerPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		startPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		playPanel.transform.localPosition = new Vector3 (-5000f, 0, 0);
		_playMusic = transform.GetComponentInParent<PlayMusic> ();

		coverFail.gameObject.SetActive (false);
		coverWin.gameObject.SetActive (false);
		myTween = GetComponent<MyTween> ();

		if (DataManager.AccountId>0) {
			GoToStartPanel ();
		} else {
			GoToRegisterPanel ();
		}
    }
		
	void Update(){
		if ((!isLoading && loadingCover.activeSelf)) {
			loadingCover.SetActive (false);
		}
		if (isLoading ) {
			if (!loadingCover.activeSelf)
				loadingCover.SetActive (true);
			loadingImage.transform.Rotate (Vector3.back);
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
		
	#region GameValueProcess
	public Color GetImageColor(int num)
	{
		switch (num) {
			case 1:
				return new Color32 (255, 255, 255, 255); 
			case 3:
				return new Color32 (0, 255, 0, 255); 
			case 9:
				return new Color32 (0, 255, 255, 255); 	
			case 27:
				return new Color32 (2, 126, 248, 255);	
			case 81:
				return new Color32 (0, 63, 255, 255); 
			case 243:
				return new Color32 (255, 255, 0, 255);
			case 729:
				return new Color32 (255, 0, 255, 255);
			case 2187:
				return new Color32 (255, 0, 0, 255);
			case 6561:
				return new Color32 (251, 183, 6, 255);
			default:
				return Color.black;
		}
	}

	public Color GetTextColor(int num)
	{
		switch (num) {
			case 1:
				return Color.grey; 
			case 3:
				return Color.grey; 
			case 9:
				return Color.magenta;
			case 27:
				return Color.magenta;
			case 81:
				return Color.yellow;
			case 243:
				return Color.yellow;
			case 729:
				return Color.blue;
			case 2187:
				return Color.blue;
			case 6561:
				return Color.white;
			default:
				return Color.white;
		}
	}

	public string GetGradeByLevel(int level){
		string s = "";
		switch (level) {
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
				s = "";
				break;
		}
		return s;
	} 

	public string GetGradeByScore(int score){
		string s = "";
		switch (score) {
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
				s = "";
				break;
		}
		return s;
	}
	#endregion
}
