using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelShow : MonoBehaviour {

    public Image[] levelList;
    public Sprite undone;
    public Sprite done;
    public Sprite going;
    public GameObject levelText;

	
    public void UpdateLevelShow (int levelNow) {
		for (int i = 0; i < levelList.Length; i++) {
			if (i < levelNow - 1)
				levelList [i].sprite = done;
			else if (i == levelNow - 1)
				levelList [i].sprite = going;
			else
				levelList [i].sprite = undone;
		}

		levelText.transform.DOLocalMoveX (levelList [levelNow - 1].transform.localPosition.x, 0.5f);
	}
}
