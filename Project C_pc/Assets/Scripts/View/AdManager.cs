using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager:MonoBehaviour
{
    public Button ShowAdBtn;
	string appId = "5a9746eb1aadac9e71006a08";
	Dictionary<string,bool> placements = new Dictionary<string, bool>{
		{"REWARDED-4680244",false}	
	};
	string[] placementStr;
//	public Text adState;
    
	void Start(){
//		placementStr = new string[]{ "REWARDED-4680244","DEFAULT-0910675"};
		placementStr = new string[placements.Keys.Count];
		placements.Keys.CopyTo (placementStr, 0);
		Vungle.init (appId, placementStr);
		Vungle.onAdFinishedEvent += (placementId, args) =>{
			AdFinished(placementId,args);
		} ;
		Vungle.onLogEvent += (log) => {
			Debug.Log ("Log:" + log);
		};

//		Vungle.loadAd (placementStr [1]);
    }

    void Update(){
		if (Vungle.isAdvertAvailable (placementStr [0]) )
//			|| Vungle.isAdvertAvailable (placementStr [1])) 
		{
			ShowAdBtn.interactable = true;
			ShowAdBtn.gameObject.GetComponentInChildren<Text> ().color = Color.green;
//			adState.text = "Done";
		} else {
			ShowAdBtn.interactable = false;
			ShowAdBtn.gameObject.GetComponentInChildren<Text> ().color = Color.grey;
//			adState.text = "Requesting";
		}
	}

	public void OnShowAd(){
        HideAdNotice ();

		Dictionary<string, object> options = new Dictionary<string, object> ();
		options ["userTag"] = "广告";
		options ["alertTitle"] = "请注意!";
		options ["alertText"] = "如果广告未播放完，则无法获得奖励！";
		options ["closeText"] = "关闭";
		options ["continueText"] = "继续观看";

		Vungle.playAd (options, placementStr [0]);

//		if (Vungle.isAdvertAvailable (placementStr [0]))
//			Vungle.playAd (placementStr [0]);
//		else
//			Vungle.playAd (placementStr [1]);
    }

	void AdFinished(string placementId, AdFinishedEventArgs args){
		if (args.WasCallToActionClicked || args.IsCompletedView) {
			Initialize _ini = GetComponentInChildren<Initialize> ();
			_ini.AddResetPoint ();
			_ini.Reset ();
		} else {
			GetComponentInChildren<Initialize> ().ConfirmComplete ();
		}
    }

    public void OnCancelAd(){
        HideAdNotice ();
        GetComponentInChildren<Initialize> ().ConfirmComplete ();
    }

    void HideAdNotice(){
        ViewManager _view = GetComponent<ViewManager>();
        _view.HideAdNotice ();
    }
}


