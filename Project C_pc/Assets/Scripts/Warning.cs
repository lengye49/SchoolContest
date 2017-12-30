using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Warning : MonoBehaviour {

	private float upTime = 0.2f;
	private float waitTime = 1.3f;
	private float disappearTime = 0.5f;

	public void ShowWarning(int type,string content){
		GameObject f = Instantiate (Resources.Load ("shortWarning")) as GameObject;
		f.SetActive (true);
		f.transform.SetParent (this.gameObject.transform);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = content;

		Color c = Color.black;
		if (type == 0)
			c = Color.black;
		else if (type == 1)
			c = Color.green;
		else if (type == 2)
			c = Color.red;
		t.color = c;

		PopWarning (f);
	}

//	public void ShowWarning(string head, string content){
//		GameObject f = Instantiate (Resources.Load ("longWarning")) as GameObject;
//		f.SetActive (true);
//		Text[] t = f.GetComponentsInChildren<Text> ();
//		t [0].text = head;
//		t [1].text = content;
//		PopWarning (f);
//	}

	void PopWarning(GameObject f){
		f.transform.localPosition = new Vector3 (0, -650, 0);
		f.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
		f.transform.DOLocalMoveY (-550, upTime);
		f.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f),upTime);
		f.GetComponentInChildren<Text> ().DOFade (1, upTime);
		StartCoroutine (WaitAndNext (f));
	}

	IEnumerator WaitAndNext(GameObject f){
		yield return new WaitForSeconds (upTime + waitTime);
		EndFloat (f);
	}

	void EndFloat(GameObject f){
		f.transform.DOLocalMoveY (-400, disappearTime);
		f.GetComponentInChildren<Text>().DOFade (0, disappearTime);
		StartCoroutine (WaitAndEnd (f));
	}

	IEnumerator WaitAndEnd(GameObject f){
		yield return new WaitForSeconds (disappearTime);
		Destroy (f);
	}
}
