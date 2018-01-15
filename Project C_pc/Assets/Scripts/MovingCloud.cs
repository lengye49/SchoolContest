using UnityEngine;
using DG.Tweening;

public class MovingCloud : MonoBehaviour {

	public Transform[] clouds;
	private float leftX = -700f;
	private float rightX = 700f;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < clouds.Length; i++) {
			bool ToLeft = (i % 2 == 0);
			StartMoving (clouds [i], ToLeft);
		}
	}

	void StartMoving(Transform t,bool ToLeft){
		float time1 = Random.Range (10.0f, 20.0f);
		float time2 = Random.Range (30.0f, 50.0f);
		float ToX = ToLeft ? leftX : rightX;
		float fromX = ToLeft ? rightX : leftX;
		t.DOLocalMoveX (ToX, time1).OnComplete (() => StartPingPong (t, time2, ToX, fromX));
	}
	
	void StartPingPong(Transform t, float time, float FromX, float ToX){
		t.DOLocalMoveX (ToX, time).OnComplete (() => StartPingPong (t, time, ToX, FromX));
	}
}
