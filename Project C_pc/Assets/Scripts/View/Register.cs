using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {

	public Text textName;
	public Text textCountry;
//	public Text textSchool;

	private string playerName;
	private string playerCountry;
//	private string playerSchool;
	private Vector3 pos = new Vector3(0,-650,0);

	public void Confirm(){
		playerName = textName.text;
		if (playerName == "") {
			Warning.ShowShortWarning (2, "请问高姓大名？",pos);
			return;
		}

		playerCountry = textCountry.text;
		if (playerCountry == "") {
			Warning.ShowShortWarning (2, "请问仙乡何处？",pos);
			return;
		}

//		playerSchool = textSchool.text;
//		if (playerSchool == "") {
//			Warning.ShowShortWarning (2, "请道友报上派别！",pos);
//			return;
//		}
			

		//DataManager.Register(playerName,playerCountry,playerSchool)
		DataManager.Register (playerName, playerCountry);
	
		//Turn to StartPanel
		this.GetComponentInParent<ViewManager>().GoToStartPanel();
	}
}
