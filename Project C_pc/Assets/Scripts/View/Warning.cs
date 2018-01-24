using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Warning : MonoBehaviour {

	/// <summary>
	/// 显示警告信息
	/// </summary>
	public void ShowWarning(){
		
	}

	/// <summary>
	/// 显示加载圈圈
	/// </summary>
	public void ShowLoading(){
		
	}



	/// <summary>
	/// 小提示.
	/// </summary>
	/// <param name="type">Type 0Black 1Green 2Red</param>
	/// <param name="content">Content.</param>
	public void ShowTip(int colorType,string content,Vector3 pos){
		GameObject f = Instantiate (Resources.Load ("shortWarning")) as GameObject;
		f.SetActive (true);
		f.transform.SetParent (this.gameObject.transform);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = content;

		Color c = Color.black;
		if (colorType == 0)
			c = Color.black;
		else if (colorType == 1)
			c = Color.green;
		else if (colorType == 2)
			c = Color.red;
		t.color = c;

		GetComponent<MyTween> ().PopIn (f.transform);
	}

		
}
