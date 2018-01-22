using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour  {

	void Awake(){
//		PlayerPrefs.DeleteAll ();
//		Debug.Log ("Delete all memmory");
	}



	#region LocalData

	private int score=0;
	private int level=0;
    private string numList;

	public int Score {
		get {
			return score;
		}
		set {
			score = value; 
		}
	}
	public int Level {
		get {
			return level;
		}
		set {
			level = value; 
		}
	}

	public string PlayerName {
		get{ return PlayerPrefs.GetString ("playerName", "无名氏"); }
		set { PlayerPrefs.SetString ("playerName", value); }
	}

	public string PlayerCountry {
		get{ return PlayerPrefs.GetString ("playerCountry", "流浪者"); }
		set { PlayerPrefs.SetString ("playerCountry", value); }
	}

	public string PlayerSchool {
		get{ return PlayerPrefs.GetString ("playerSchool", "散修"); }
		set { PlayerPrefs.SetString ("playerSchool", value); }
	}

	public static int AccountId
    {
        get{ return PlayerPrefs.GetInt("accountId", 0); }
        set{ PlayerPrefs.SetInt("accountId", value); }
    }
        
	public int HighScore {
		get{ return PlayerPrefs.GetInt ("HighScore", 0); }
		set {
			PlayerPrefs.SetInt ("HighScore", value);
		}
	}
	public int HighLevel{
		get{return PlayerPrefs.GetInt ("HighLevel1", 0);}
		set{
			PlayerPrefs.SetInt ("HighLevel1",value);}
	}

	public int SetHighScore(){
		if ((level > HighLevel) || (level == HighLevel && score > HighScore)) {
			HighLevel = level;
			HighScore = score;
			return 1;
		} else {
			return 0;
		}
	}
	#endregion


	#region GameValueProcess
	public Color GetImageColor(int num)
	{
		switch (num) {
		case 1:
			return new Color32 (255, 255, 255, 255); 
		case 3:
			return new Color32 (0, 255, 0, 255); 
		case 9:
			return new Color32 (0, 255, 255, 255); 	
		case 27:
			return new Color32 (2, 126, 248, 255);	
		case 81:
			return new Color32 (0, 63, 255, 255); 
		case 243:
			return new Color32 (255, 255, 0, 255);
		case 729:
			return new Color32 (255, 0, 255, 255);
		case 2187:
			return new Color32 (255, 0, 0, 255);
		case 6561:
			return new Color32 (251, 183, 6, 255);
		default:
			return Color.black;
		}
	}

	public Color GetTextColor(int num)
	{
		switch (num) {
		case 1:
			return Color.grey; 
		case 3:
			return Color.grey; 
		case 9:
			return Color.magenta;
		case 27:
			return Color.magenta;
		case 81:
			return Color.yellow;
		case 243:
			return Color.yellow;
		case 729:
			return Color.blue;
		case 2187:
			return Color.blue;
		case 6561:
			return Color.white;
		default:
			return Color.white;
		}
	}

	public string GetGradeByLevel(int level){
		string s = "";
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
			s = "";
			break;
		}
		return s;
	} 

	public string GetGradeByScore(int score){
		string s = "";
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
			s = "";
			break;
		}
		return s;
	}
	#endregion

	public void Register(string playerName,string playerCountry,string playerSchool){
		PlayerName = playerName;
		PlayerCountry = playerCountry;
		PlayerSchool = playerSchool;

		//Get Remote AccountId
		AccountId = 1;

	}

    public int GetOnlineRank(){
		
        return 1;
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
	