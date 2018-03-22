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
        private long TotalArea = 14400000000;
        private UserDAO userDAO = new UserDAO();

        public GameController(){
            requestCode = RequestCode.Game;
        }

        //返回的数据是 id,排名
        public string GetPersonalResult(string data,Client client,Server server)
        {
            User user = new User(data);
            if (user.Id == 0) {
                user.Id = userDAO.AddUser();
            }

            int rank = -1;
            if (!Compare(user.Level, user.Score, TotalRank.Length - 1))
                return user.Id + "," + rank;
            
            //判断有无重复
            for (int i = 0; i < TotalRank.Length; i++) {
                if (TotalRank[i].Id == user.Id)
                {
                    if (Compare(user.Level, user.Score, i))
                    {
                        TotalRank[i] = user;
                        rank = ReOrder(i);
                    }
                    return user.Id + "," + rank;
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
            return user.Id + "," + rank;
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

        //返回的数据是id,Name,Place,Level,Score;id...
        public string GetTotalRank(string data, Client client, Server server){
            string s = "";
            for (int i = 0; i < TotalRank.Length;i++){
                if (TotalRank[i] != null)
                {
                    s += TotalRank[i].Id + "," + TotalRank[i].Name + "," + TotalRank[i].Place + "," + TotalRank[i].Level + "," + TotalRank[i].Score + ";";
                }    
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }

        public void InitRanks(string data, Client client, Server server){
            userDAO.SetServerTotalRank(ref TotalRank);
        }

        public void SaveRanks(string data, Client client, Server server){
            userDAO.SaveTotalRank(TotalRank);
        }

        long GetArea(long score,long totalScore) {
            return Math.Min(score / 1000, score / totalScore * TotalArea);
        }
    }
}
