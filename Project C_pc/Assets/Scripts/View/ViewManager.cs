using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour {

    //界面
    public Transform startPanel;
	public Transform rankPanel;
	public Transform playPanel;

	public Transform registerPanel;
	private Vector3 startPos = new Vector3 (-5000, 0, 0);

    //游戏界面
    public Text scoreText;
    public Text gradeText;
	public Sprite[] cells;
	public Sprite[] resetBg;

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
		registerPanel.localPosition = startPos;
		startPanel.localPosition = startPos;
		playPanel.localPosition = startPos;
		rankPanel.localPosition = startPos;
		_playMusic = GetComponentInParent<PlayMusic> ();
		UpdateLocalRank ();

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
		myTween.SlideIn (startPanel);
		myTween.SlideOut (registerPanel);
		myTween.SlideOut (playPanel);

		_playMusic.PlayBg ("startMenuBg");
	}

	public void GoToRankPanel(){
		myTween.SlideIn (rankPanel);
		myTween.SlideOut (startPanel);
		DataManager.SetTotalRank ();
	}

	public void OnRankReturnButton(){
		myTween.SlideIn (startPanel);
		myTween.SlideOut (rankPanel);
	}

	public void GoToRegisterPanel(){
		myTween.SlideIn (registerPanel);
		_playMusic.PlayBg ("startMenuBg");
	}

	public void OnFailReturnButton()
	{
		myTween.SlideIn (startPanel);
		myTween.SlideOut (playPanel);

		coverFail.gameObject.SetActive (false);
	}

	public void OnWinReturnButton(){
		myTween.SlideIn (startPanel);
		myTween.SlideOut (playPanel);
		coverWin.gameObject.SetActive (false);
	}
        

	public void GoToGamePanel(){

		myTween.SlideIn (playPanel);
		myTween.SlideOut (startPanel);


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

	public void UpdateLocalRank(){
		int s = DataManager.HighScore;
		string g = Configs.LevelList [DataManager.HighLevel-1];

		if (s > 0)
			localScore.text = "个人最高道行：" + g + "(法力" + s + ")";
		else
			localScore.text = "";	
	}

	public void SetResetState(bool hasEnergy){
		ResetOne.interactable = hasEnergy;
		if (hasEnergy) {
			ResetOne.image.sprite = resetBg [0];
		}else
			ResetOne.image.sprite = resetBg [1];
	}

//	public void ResetOneOn(bool isOn){
//	
//		if (isOn) {
//			Debug.Log ("开启仙人指路");
//			//特效打开
//		} else {
//			Debug.Log ("关闭仙人指路");
//			//特效关闭
//		}
//	}

	public void Upgrade(int newLevel){
//		_levelShow.UpdateLevelShow (newLevel);
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
		GetComponentInChildren<Initialize> ().ResetAllSmallNum ();
	}

	public void OnCancelAd(){
		HideAdNotice ();
		GetComponentInChildren<Initialize> ().ConfirmComplete ();
	}
		
	#region 单元格属性
	public Sprite GetCellSprite(int num){
		switch (num) {
			case 1:
				return cells[0]; 
			case 3:
				return cells[1]; 
			case 9:
				return cells[2]; 	
			case 27:
				return cells[3];	
			case 81:
				return cells[4]; 
			case 243:
				return cells[5];
			case 729:
				return cells[6];
			case 2187:
				return cells[7];
			case 6561:
				return cells[8];
			case 19683:
				return cells [9];
			default:
				return cells[10];
		}
	}

	public Color GetCellColor(int num){
		switch (num) {
			case 1:
				return new Color (249f / 255f, 231f / 255f, 211f / 255f);
			case 3:
				return new Color (210f / 255f, 243f / 255f, 138f / 255f);
			case 9:
				return new Color (103f / 255f, 204f / 255f, 50f / 255f);
			case 27:
				return new Color (255f / 255f, 216f / 255f, 0f / 255f);
			case 81:
				return new Color (54f / 255f, 141f / 255f, 234f / 255f);
			case 243:
				return new Color (153f / 255f, 51f / 255f, 153f / 255f);
			case 729:
				return new Color (255f / 255f, 102f / 255f, 0f / 255f);
			case 2187:
				return new Color (210f / 255f, 31f / 255f, 217f / 255f);
			case 6561:
				return new Color (0f / 255f, 255f / 255f, 255f / 255f);
			case 19683:
				return new Color (255f / 255f, 56f / 255f, 104f / 255f);
			default:
				return Color.red;
		}
	}

	public Color GetTextColor(int num)
	{
		if (num > 27 && num <= 2187)
			return Color.white;
		return Color.black;
//		switch (num) {
//			case 1:
//				return new Color (0f / 255f, 160f / 255f, 233f / 255f);
//			case 3:
//				return new Color (250f / 255f, 205f / 255f, 137f / 255f);
//			case 9:
//				return new Color (128f / 255f, 194f / 255f, 105f / 255f);
//			case 27:
//				return new Color (255f / 255f, 247f / 255f, 153f / 255f);
//			case 81:
//				return new Color (236f / 255f, 105f / 255f, 65f / 255f);
//			case 243:
//				return new Color (0f / 255f, 255f / 255f, 255f / 255f);
//			case 729:
//				return new Color (0f / 255f, 255f / 255f, 0f / 255f);
//			case 2187:
//				return new Color (255f / 255f, 255f / 255f, 0f / 255f);
//			case 6561:
//				return new Color (255f / 255f, 0f / 255f, 0f / 255f);
//			case 19683:
//				return new Color (255f / 255f, 255f / 255f, 255f / 255f);
//			default:
//				return Color.white;
//		}
	}

	public string GetGradeByScore(int score){
		for (int i = 0; i <= 10; i++) {
			if (Mathf.Pow (3, i) == score)
				return Configs.LevelList [i];
		}
		return Configs.LevelList [0];
	}
	#endregion
}
