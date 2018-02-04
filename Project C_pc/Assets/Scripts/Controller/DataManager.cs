using System.Collections;
using UnityEngine;
using System.IO;

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

	public static int OnlineRank{
		get{ return PlayerPrefs.GetInt("personalRank", 0); }
		set{ PlayerPrefs.SetInt("personalRank", value); }
	}

	public static int OnlineRate{
		get{ return PlayerPrefs.GetInt("rate", 0); }
		set{ PlayerPrefs.SetInt("rate", value); }
	}

	public static string PlaceAreaRank{
		get{ return PlayerPrefs.GetString ("areaRank", "");}
		set{ PlayerPrefs.SetString ("areaRank", "");}
	}

	public static string TotalRank{
		get{ return PlayerPrefs.GetString ("totalRank", "");}
		set{ PlayerPrefs.SetString ("totalRank", "");}
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

	public void Register(string playerName,string playerCountry,string playerSchool){
		PlayerName = playerName;
		PlayerCountry = playerCountry;
		PlayerSchool = playerSchool;

		Client client = new Client ();
		string msg ="";
		client.GetRemoteService (RequestCode.Register,ActionCode.None, msg);
	}

	//注意账号的处理
	public void SetOnlineRank(){
		if (AccountId <= 0) {
			Debug.Log ("暂时没有网络账号，请获取账号id后再上传成绩！");
			return;
		} 
		Client client = new Client ();
		string msg = PlayerName + "," + PlayerCountry + "," + PlayerSchool + "," + HighLevel + "," + HighScore;
		client.GetRemoteService (RequestCode.Game,ActionCode.PersonalRank, msg);
	}
		
}

//	#region SaveData
//using System.Runtime.Serialization.Formatters.Binary;
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
	