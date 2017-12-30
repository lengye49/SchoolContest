using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {

	public Text textName;
	public Text textCountry;
	public Text textSchool;

	private string playerName;
	private string playerCountry;
	private string playerSchool;

	public void Confirm(){
		playerName = textName.text;
		if (playerName == "") {
			this.GetComponentInParent<Warning> ().ShowWarning (2, "请道友报上大名！");
			return;
		}

		playerCountry = textCountry.text;
		if (playerCountry == "") {
			this.GetComponentInParent<Warning> ().ShowWarning (2, "请道友报上仙府！");
			return;
		}

		playerSchool = textSchool.text;
		if (playerSchool == "") {
			this.GetComponentInParent<Warning> ().ShowWarning (2, "请道友报上派别！");
			return;
		}
			
		//DataManager.Register(playerName,playerCountry,playerSchool)
		this.GetComponentInParent<DataManager> ().Register (playerName, playerCountry, playerSchool);
	
		//Turn to StartPanel
		this.GetComponentInParent<ViewManager>().GoToStartPanel();
	}
}
