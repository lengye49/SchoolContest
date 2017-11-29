using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonStart : MonoBehaviour {

	public GameObject playPanel;
	public GameObject startPanel;
	public GameObject failCover;
	public Text supportText;
	public Button continueButton;
	public GameObject instructions;
	public GameObject instrCover;

//	public GameObject settingContent;
	private PlayMusic _playMusic;

	void Start () {
		startPanel.transform.localPosition = Vector3.zero;
		playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		continueButton.interactable = false;
		_playMusic = startPanel.gameObject.GetComponentInParent<PlayMusic> ();
		supportText.gameObject.SetActive (true);
		instructions.SetActive (false);
		instrCover.SetActive (false);
	}

	void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			if (startPanel.transform.localPosition == Vector3.zero) {
				Application.Quit ();
			} else if (failCover.gameObject.activeSelf) {
				OnFailReturnButton ();
			} else {
				OnPlayReturnButton ();
			}
		}
	}
	
	public void OnStartButton(){
		_playMusic.PlayerSound ("click");
		startPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		playPanel.transform.localPosition = Vector3.zero;
		playPanel.GetComponent<Initialize> ().InitializeCells ();
	}

	public void OnRestartButton(){
		_playMusic.PlayerSound ("click");
		playPanel.GetComponent<Initialize> ().InitializeCells ();
	}

	public void OnPlayReturnButton()
	{
		_playMusic.PlayerSound ("click");
		playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		startPanel.transform.localPosition = Vector3.zero;
		continueButton.interactable = true;
	}

	public void OnFailReturnButton()
	{
		_playMusic.PlayerSound ("click");
		playPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		startPanel.transform.localPosition = Vector3.zero;
		failCover.gameObject.SetActive (false);
		continueButton.interactable = false;
	}

	public void OnContinue()
	{
		_playMusic.PlayerSound ("click");
		startPanel.transform.localPosition = new Vector3 (2000f, 0, 0);
		playPanel.transform.localPosition = Vector3.zero;
	}


	public void OnInstrButton(){
		instrCover.SetActive (true);
		instructions.SetActive (true);
		instructions.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
		instructions.transform.DOBlendableScaleBy (new Vector3 (0.8f, 0.8f, 1f), 0.5f);
	}

	public void OnInstrCover(){
		instrCover.SetActive (false);
		instructions.SetActive (false);
	}

}
