using System;
using UnityEngine;
using UnityEngine.UI;

public class AdManager:MonoBehaviour
{
    public Button ShowAdBtn;
    void Start(){
        
    }

    void Update(){
        ShowAdBtn.interactable = true;
    }

    public void OnShowAd(){
        HideAdNotice ();
        //ShowAd
        //Todo...

        //测试时直接完成广告
        CompleteAd();

    }

    void CompleteAd(){
        Initialize _ini = GetComponent<Initialize>();
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


