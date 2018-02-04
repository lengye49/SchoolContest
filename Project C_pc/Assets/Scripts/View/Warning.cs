using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Warning : MonoBehaviour {

	/// <summary>
	/// 小提示.
	/// </summary>
	/// <param name="type">Type 0Black 1Green 2Red</param>
	/// <param name="content">Content.</param>
	public static void ShowShortWarning(int colorType,string content,Vector3 pos){
		GameObject f = Instantiate (Resources.Load ("shortWarning")) as GameObject;
		GameObject p = GameObject.Find ("Canvas");
		f.SetActive (true);
		f.transform.SetParent (p.transform);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = content;
		t.color = GetWarningColor(colorType);

		p.GetComponent<MyTween> ().PopIn (f.transform);
	}

	public static void ShowNewRank(string content){
		GameObject f = Instantiate (Resources.Load ("rankWarning")) as GameObject;
		GameObject p = GameObject.Find ("Canvas");
		f.SetActive (true);
		f.transform.SetParent (p.transform);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = content;
		t.color = Color.green;

		p.GetComponent<MyTween> ().PopIn (f.transform,3f);
	}

	public static void ShowTotalRank(User[] uList){
		GameObject p = GameObject.Find ("RankContainer");
		for (int i = 0; i < uList.Length; i++) {
			GameObject f = Instantiate (Resources.Load ("rankCell")) as GameObject;
			f.SetActive (true);
			f.transform.SetParent (p.transform);
			Text[] t = f.GetComponentsInChildren<Text> ();
			t [0].text = uList [i].name;
			t [1].text = Configs.PlaceList [uList [i].place];
			t [2].text = Configs.SchoolList [uList [i].school];
		}
	}



	public static void ShowAreaRank(){
	
	}

	static Color GetWarningColor(int colorType){
		Color c = Color.black;
		if (colorType == 0)
			c = Color.black;
		else if (colorType == 1)
			c = Color.green;
		else if (colorType == 2)
			c = Color.red;

		return c;
	}

		
}
