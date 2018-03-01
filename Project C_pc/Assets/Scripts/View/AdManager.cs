using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager:MonoBehaviour
{
    public Button ShowAdBtn;
	string appId = "5a9746eb1aadac9e71006a08";
	string[] placementStr;
    
	void Start(){
		placementStr = new string[]{ "REWARDED-4680244" };
		Vungle.init (appId, placementStr);
		Vungle.onAdFinishedEvent += (placementId, args) =>{
			AdFinished(placementId,args);
		} ;
		Vungle.loadAd (placementStr [0]);
    }

    void Update(){
        ShowAdBtn.interactable = true;
    }

	public void OnShowAd(){
        HideAdNotice ();

		Dictionary<string, object> options = new Dictionary<string, object> ();
		options ["userTag"] = "广告";
		options ["alertTitle"] = "道友请注意!";
		options ["alertText"] = "如果广告未播放完，则无法获得回天之力！";
		options ["closeText"] = "关闭";
		options ["continueText"] = "继续观看";

		Vungle.playAd (options, placementStr [0]);
    }

	void AdFinished(string placementId, AdFinishedEventArgs args){
		if (args.WasCallToActionClicked)
		{
			Debug.Log ("点击了下载按钮");
		}
		else if (args.IsCompletedView)
		{
			Debug.Log ("完成了播放");
		}
		else
		{
			Debug.Log ("取消了播放");
		}

        Initialize _ini = GetComponentInChildren<Initialize>();
        _ini.AddResetPoint();
        _ini.Reset();
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


