using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Warning : MonoBehaviour {

	private float upTime = 0.2f;
	private float waitTime = 1.3f;
	private float disappearTime = 0.5f;
	private float y;

	/// <summary>
	/// Shows the warning.
	/// </summary>
	/// <param name="type">Type 0Black 1Green 2Red</param>
	/// <param name="content">Content.</param>
	public void ShowWarning(int colorType,string content,Vector3 pos){
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

		PopWarning (f,pos);
	}

	void PopWarning(GameObject f,Vector3 pos){
		f.transform.localPosition = pos;
		y = pos.y;
		f.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
		f.transform.DOLocalMoveY (y + 100f, upTime);
		f.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f),upTime);
		f.GetComponentInChildren<Text> ().DOFade (1, upTime);
		StartCoroutine (WaitAndNext (f));
	}

	IEnumerator WaitAndNext(GameObject f){
		yield return new WaitForSeconds (upTime + waitTime);
		EndFloat (f);
	}

	void EndFloat(GameObject f){
		f.transform.DOLocalMoveY (y + 250f, disappearTime);
		f.GetComponentInChildren<Text>().DOFade (0, disappearTime);
		StartCoroutine (WaitAndEnd (f));
	}

	IEnumerator WaitAndEnd(GameObject f){
		yield return new WaitForSeconds (disappearTime);
		Destroy (f);
	}

	/// <summary>
	/// Shows the big words.
	/// </summary>
	/// <param name="g">The green component.</param>
//	public void ShowBigWords(GameObject g){
//		g.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
//		g.SetActive (true);
//		g.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f),upTime);
//	}
}
