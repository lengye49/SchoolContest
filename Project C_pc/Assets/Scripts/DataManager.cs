using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour  {

	private int score;
	private int level;
	private int highScore1;
	private int highScore2;
	private int highScore3;
	private int highLevel1;
	private int highLevel2;
	private int highLevel3;

	public int Score{
		get{ return PlayerPrefs.GetInt ("Score", 0);}
		set{score = value; 
			PlayerPrefs.SetInt ("Score",value);}
	}	
	public int Level{
		get{ return PlayerPrefs.GetInt ("Level", 0);}
		set{level = value; 
			PlayerPrefs.SetInt ("Level",value);}
	}	
	public int HighScore1{
		get{ return PlayerPrefs.GetInt ("HighScore1", 0);}
		set{highScore1 = value;
			PlayerPrefs.SetInt ("HighScore1",value);}
	}
	public int HighScore2{
		get{ return PlayerPrefs.GetInt ("HighScore2", 0);}
		set{highScore2 = value;
			PlayerPrefs.SetInt ("HighScore2",value);}
	}
	public int HighScore3{
		get{ return PlayerPrefs.GetInt ("HighScore3", 0);}
		set{highScore3 = value;
			PlayerPrefs.SetInt ("HighScore3",value);}
	}
	public int HighLevel1{
		get{return PlayerPrefs.GetInt ("HighLevel1", 0);}
		set{highLevel1 = value;
			PlayerPrefs.SetInt ("HighLevel1",value);}
	}
	public int HighLevel2 {
		get{ return PlayerPrefs.GetInt ("HighLevel2", 0); }
		set{highLevel2 = value;
			PlayerPrefs.SetInt ("HighLevel2", value); }
	}
	public int HighLevel3{
		get{ return PlayerPrefs.GetInt ("HighLevel3", 0);}
		set{highLevel3 = value;
			PlayerPrefs.SetInt ("HighLevel3",value);}
	}
	/// <summary>
	/// 是否有存档，0没有1有
	/// </summary>
	/// <value><c>true</c> if this instance has memory; otherwise, <c>false</c>.</value>
	public int HasMemory{
		get{ return PlayerPrefs.GetInt ("HasMemmory", 0);}
		set{ PlayerPrefs.SetInt ("HasMemmory",value);}
	}

	public int SetHighScore(){
		if ((level > HighLevel1) || (level == HighLevel1 && score > HighScore1)) {
			HighLevel3 = HighLevel2;
			HighLevel2 = HighLevel1;
			HighLevel1 = level;
			HighScore3 = HighScore2;
			HighScore2 = HighScore1;
			HighScore1 = score;
			return 1;
		} else if ((level > HighLevel1) || (level == HighLevel2 && score > HighScore2)) {
			HighLevel3 = HighLevel2;
			HighLevel2 = level;
			HighScore3 = HighScore2;
			HighScore2 = score;
			return 2;
		} else if ((level > HighLevel3) || (level == HighLevel3 && score > HighScore3)) {
			HighLevel3 = level;
			HighScore3 = score;
			return 3;
		} else {
			return 0;
		}
	}


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

//	#region SaveData
//	void Save(){
//		BinaryFormatter bf = new BinaryFormatter ();
//		FileStream file = File.Create(Application.persistentDataPath+"/info.dat");
//
//		PlayerInfo info = new PlayerInfo ();
//		info.score = score;
//		info.highScore1 = highScore1;
//		info.highScore2 = highScore2;
//		info.highScore3 = highScore3;
//		info.highLevel1 = highLevel1;
//		info.highLevel2 = highLevel2;
//		info.highLevel3 = highLevel3;
//
//		bf.Serialize (file, info);
//		file.Close ();
//	}
//	#endregion
//
//	#region LoadData
//	void Load(){
//		if (File.Exists (Application.persistentDataPath + "/info.dat")) {
//			BinaryFormatter bf = new BinaryFormatter ();
//			FileStream file = File.Open(Application.persistentDataPath+"/info.dat",FileMode.Open);
//			PlayerInfo info = (PlayerInfo)bf.Deserialize (file);
//			file.Close ();
//			score = info.score;
//			highScore1 = info.highScore1;
//			highScore2 = info.highScore2;
//			highScore3 = info.highScore3;
//			highLevel1 = info.highLevel1;
//			highLevel2 = info.highLevel2;
//			highLevel3 = info.highLevel3;
//		}
//	}
//	#endregion

//class PlayerInfo{
//	public int score;
//	public int highScore1;
//	public int highScore2;
//	public int highScore3;
//	public int highLevel1;
//	public int highLevel2;
//	public int highLevel3;
//}
	