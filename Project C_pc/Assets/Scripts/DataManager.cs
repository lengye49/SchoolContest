using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour  {
	[HideInInspector]
	public int score;
	[HideInInspector]
	public int highScore1;
	[HideInInspector]
	public int highScore2;
	[HideInInspector]
	public int highScore3;
	[HideInInspector]
	public int highLevel1;
	[HideInInspector]
	public int highLevel2;
	[HideInInspector]
	public int highLevel3;

		
	#region SaveData
	void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.persistentDataPath+"/info.dat");

		PlayerInfo info = new PlayerInfo ();
		info.score = score;
		info.highScore1 = highScore1;
		info.highScore2 = highScore2;
		info.highScore3 = highScore3;
		info.highLevel1 = highLevel1;
		info.highLevel2 = highLevel2;
		info.highLevel3 = highLevel3;

		bf.Serialize (file, info);
		file.Close ();
	}
	#endregion

	#region LoadData
	void Load(){
		if (File.Exists (Application.persistentDataPath + "/info.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open(Application.persistentDataPath+"/info.dat",FileMode.Open);
			PlayerInfo info = (PlayerInfo)bf.Deserialize (file);
			file.Close ();
			score = info.score;
			highScore1 = info.highScore1;
			highScore2 = info.highScore2;
			highScore3 = info.highScore3;
			highLevel1 = info.highLevel1;
			highLevel2 = info.highLevel2;
			highLevel3 = info.highLevel3;
		}
	}
	#endregion

	public Color GetColorByNum(int num)
	{
		switch (num) {
		case 1:
			return new Color32 (192, 192, 192, 255); //Grey
		case 3:
			return new Color32 (255, 250, 250, 255); //SnowWhite
		case 9:
			return new Color32 (0, 250, 154, 255); 	 //MedSpringGreen
		case 27:
			return new Color32 (0, 191, 255, 255);	 //DeepSkyBlue
		case 81:
			return new Color32 (147, 112, 219, 255); //MediumPurple
		case 243:
			return new Color32 (255, 215, 0, 255);	 //Gold
		case 729:
			return new Color32 (255, 0, 0, 255);	 //Red
		case 2187:
			return new Color32 (139, 69, 19, 255);	 //SaddleBrown
		case 6561:
			return new Color32 (0, 0, 0, 255);		 //Black
		default:
			return Color.black;
		}
	}

	public string GetGradeByLevel(int level){
		string s = "练气";
		switch (level) {
		case 1:
			s = "练气";
			break;
		case 2:
			s = "筑基";
			break;
		case 3:
			s = "金丹";
			break;
		case 4:
			s = "元婴";
			break;
		case 5:
			s = "出窍";
			break;
		case 6:
			s = "分神";
			break;
		case 7:
			s = "合体";
			break;
		case 8:
			s = "洞虚";
			break;
		case 9:
			s = "大成";
			break;
		case 10:
			s = "渡劫";
			break;
		case 11:
			s = "升仙";
			break;
		default:
			s = "升仙";
			break;
		}
		return s;
	} 

	public  string GetGradeByScore(int score){
		string s = "练气";
		switch (score) {
		case 1:
			s = "练气";
			break;
		case 3:
			s = "筑基";
			break;
		case 9:
			s = "金丹";
			break;
		case 27:
			s = "元婴";
			break;
		case 81:
			s = "出窍";
			break;
		case 243:
			s = "分神";
			break;
		case 729:
			s = "合体";
			break;
		case 2187:
			s = "洞虚";
			break;
		case 6561:
			s = "大成";
			break;
		case 19684:
			s = "渡劫";
			break;
		case 59049:
			s = "升仙";
			break;	
		default:
			s = "升仙";
			break;
		}
		return s;
	}
}

class PlayerInfo{
	public int score;
	public int highScore1;
	public int highScore2;
	public int highScore3;
	public int highLevel1;
	public int highLevel2;
	public int highLevel3;
}
	