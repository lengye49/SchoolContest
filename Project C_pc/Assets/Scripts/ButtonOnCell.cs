using UnityEngine;
using System.Collections;

public class ButtonOnCell : MonoBehaviour {

	public void SendButtonMessage()
	{
		string[] s = this.gameObject.name.Split(',');
		try {
			int i = int.Parse (s [0]);
			int j = int.Parse (s [1]);
			Initialize ini = this.gameObject.GetComponentInParent<Initialize>();
			ini.ButtonClickCal(i,j);
		} catch (UnityException e) {
			Debug.Log (this.gameObject.name + " can not get a index!" + e.Message);
		}
	}

}
