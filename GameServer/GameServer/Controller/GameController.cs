using System;
using GameServer.Model;
using GameServer.Servers;
using Common;
using GameServer.DAO;
namespace GameServer.Controller
{
    class GameController : BaseController
    {
        private User[] TotalRank = new User[100];
        private int[] PlaceScore = new int[33];
        private long TotalArea = 14400000000;
        private UserDAO userDAO = new UserDAO();

        public GameController(){
            requestCode = RequestCode.Game;
        }


        public string GetPersonalResult(string data,Client client,Server server)
        {
            User user = new User(data);
            SetScore(user);

            int rank = 0;
            if (!Compare(user.Level, user.Score, TotalRank.Length - 1))
                return rank.ToString();
            
            //判断有无重复
            for (int i = 0; i < TotalRank.Length; i++) {
                if (TotalRank[i].Id == user.Id)
                {
                    if (Compare(user.Level, user.Score, i))
                    {
                        TotalRank[i] = user;
                        rank = ReOrder(i);
                    }
                    return rank.ToString();
                }
            }
            //没有重复则插入
            for (int i = 0; i < TotalRank.Length; i++)
            {
                if (Compare(user.Level, user.Score, i))
                {
                    Moveback(i);
                    TotalRank[i] = user;
                    rank = i;
                    break;
                }
            }
            return rank.ToString();
        }

        bool Compare(int level, int score,int index) {
            return level > TotalRank[index].Level || (level == TotalRank[index].Level && score > TotalRank[index].Score);
        }

        void Moveback(int index)
        {
            for (int i = TotalRank.Length - 1; i > index; i--)
            {
                TotalRank[i] = TotalRank[i - 1];
            }
        }

        int ReOrder(int index) {
            int rank = index;
            for (int i = index; i > 0; i--) {
                if (Compare(TotalRank[i].Level, TotalRank[i].Score, i - 1))
                {
                    Swap(i);
                    rank = i;
                }
                else break;
            }
            return rank;
        }

        void Swap(int index) {
            User u = TotalRank[index];
            TotalRank[index] = TotalRank[index - 1];
            TotalRank[index - 1] = u;
        }

        public string GetTotalRank(string data, Client client, Server server){
            string s = "";
            for (int i = 0; i < TotalRank.Length;i++){
                if(TotalRank[i]!=null){
                    s += TotalRank[i].Name + "," + TotalRank[i].Place  + "," + TotalRank[i].Level + "," + TotalRank[i].Score + ";";
                }    
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }

        public string GetGetAreaList(string data, Client client, Server server) {
            string s = "";
  
            long totalScore = 0;
            for (int i = 0; i < PlaceScore.Length; i++)
            {
                totalScore += PlaceScore[i];
            }
            for (int i = 0; i < PlaceScore.Length; i++)
            {
                s += GetArea(PlaceScore[i], totalScore)+",";
            }
            s = s.Substring(0, s.Length - 1);
            return "";
        }

        public void InitRanks(string data, Client client, Server server){
            userDAO.SetServerTotalRank(ref TotalRank);
            //userDAO.SetServerPlaceScore(ref PlaceScore);
            CheckRank();
        }

        public void SaveRanks(string data, Client client, Server server){
            CheckRank();
            userDAO.SaveTotalRank(TotalRank);
            //userDAO.SavePlaceScore(PlaceScore);
        }

        void CheckRank() {
            Console.WriteLine("Rank30 is " + TotalRank[30].Name);
        }


        void SetScore(User user)
        {
            PlaceScore[user.Place] += user.Score;
        }

        long GetArea(long score,long totalScore) {
            return Math.Min(score / 1000, score / totalScore * TotalArea);
        }
    }
}
