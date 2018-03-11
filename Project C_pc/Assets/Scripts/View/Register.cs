using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {

	public Text textName;
	public Text textCountry;

	private string playerName;
	private string playerCountry;
	private Vector3 pos = new Vector3(0,-650,0);

	public void Confirm(){
		playerName = textName.text;
		if (playerName == "") {
			Warning.ShowShortWarning (2, "请问道友高姓大名？",pos);
			return;
		}
		if (playerName.Length > 5) {
			Warning.ShowShortWarning (2, "道号最多五个字！", pos);
			return;
		}

		playerCountry = textCountry.text;
		if (playerCountry == "") {
			Warning.ShowShortWarning (2, "请问道友仙乡何处？",pos);
			return;
		}
			
		DataManager.Register (playerName, playerCountry);
		this.GetComponentInParent<ViewManager>().GoToStartPanel();
	}
}
