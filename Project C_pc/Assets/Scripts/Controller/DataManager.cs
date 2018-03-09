using System.Collections;
using UnityEngine;
//using System.IO;

public class DataManager : MonoBehaviour  {

	void Awake(){
//		PlayerPrefs.DeleteAll ();
//		Debug.Log ("Delete all memmory");
	}

	#region LocalData
	static int score=0;
	static int level=0;
    static string numList;

	public static int Score {
		get {
			return score;
		}
		set {
			score = value; 
		}
	}
	public static int Level {
		get {
			return level;
		}
		set {
			level = value; 
		}
	}

	public static string PlayerName {
		get{ return PlayerPrefs.GetString ("playerName", "无名氏"); }
		set { PlayerPrefs.SetString ("playerName", value); }
	}

	public static string PlayerCountry {
		get{ return PlayerPrefs.GetString ("playerCountry", "流浪者"); }
		set { PlayerPrefs.SetString ("playerCountry", value); }
	}

	public static int AccountId
    {
        get{ return PlayerPrefs.GetInt("accountId", 0); }
        set{ PlayerPrefs.SetInt("accountId", value); }
    }
		
	public static string GetRankStr(int rank){
		string s = "";
		if (rank == 0) {
			if (level < 6)
				s = "籍籍无名！\n道友仍需努力！";
			else {
				s = "仙道";
			}
		} else {
			s="道友已超过了99.9%的同道。\n位列仙道第"+rank+"名！";
		}
		return s;
	}

	private static float GetRankPercent(){
		if (score < 1000)
			return 50.0f;
		else if (score > 9000)
			return 99.0f;
		else
			return 50.0f + (score - 1000f) * 0.005f;
	}

	public static string TotalRank{
		get{ return PlayerPrefs.GetString ("totalRank", "");}
		set{ PlayerPrefs.SetString ("totalRank", "");}
	}
        
	public static int HighScore {
		get{ return PlayerPrefs.GetInt ("HighScore", 0); }
		set {
			PlayerPrefs.SetInt ("HighScore", value);
		}
	}
	public static int HighLevel{
		get{return PlayerPrefs.GetInt ("HighLevel1", 1);}
		set{
			PlayerPrefs.SetInt ("HighLevel1",value);}
	}

	public static int SetHighScore(){
		if ((level > HighLevel) || (level == HighLevel && score > HighScore)) {
			HighLevel = level;
			HighScore = score;
			return 1;
		} else {
			return 0;
		}
	}
	#endregion

	public static void Register(string playerName,string playerCountry){
		PlayerName = playerName;
		PlayerCountry = playerCountry;

		Client client = new Client ();
		client.GetRemoteService (RequestCode.Register,ActionCode.None, "");
	}
		
	public static void SetOnlineRank(){
		Client client = new Client ();
		string msg = AccountId + "," + PlayerName + "," + PlayerCountry + "," + HighLevel + "," + HighScore;
		client.GetRemoteService (RequestCode.Game,ActionCode.GetPersonalResult, msg);
	}
		

	public static void RequestTotalRank(){
		Client client = new Client ();
		client.GetRemoteService(RequestCode.Game,ActionCode.GetTotalRank,"");
	}
	
	
	public static void SetTotalRank(string str){
		RankManager.TopUserList = new User[100];
		string[] ss = str.Split(';');
		for (int i = 0; i < 100; i++) {
			string[] s = ss[i].Split(',');
			RankManager.TopUserList [i] = new User ();
			RankManager.TopUserList [i].id = int.Parse(s[0]);
			RankManager.TopUserList [i].name = s[1];
			RankManager.TopUserList [i].place = int.Parse(s[2]);
			RankManager.TopUserList [i].level = int.Parse(s[3]);
			RankManager.TopUserList [i].score = int.Parse(s[4]);
		}
		RankManager.isRankReady = true;
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
	