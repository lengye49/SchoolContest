using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewManager : MonoBehaviour {

    //开始界面
    public GameObject startPanel;
    public Button continueButton;

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
    public GameObject failCover;
    public GameObject coverFail;
    public Text failText;
    public GameObject coverWin;
    public Text winText;

    void Start(){
        startPanel.transform.localPosition = Vector3.zero;
        playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);

        _playMusic = transform.GetComponentInParent<PlayMusic> ();
        _playMusic.PlayBg ("startMenuBg");

        coverFail.gameObject.SetActive (false);
        coverWin.gameObject.SetActive (false);

        SetContinueButton(PlayerPrefs.GetInt("HasMemory", 0));
    }

    void SetContinueButton(bool isAct){
        continueButton.interactable = isAct;
    }

	
}
