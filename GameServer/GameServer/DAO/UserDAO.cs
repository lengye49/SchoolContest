using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    public class UserDAO
    {
        MySqlDataReader reader = null;

        MySqlConnection GetUserConn(){
            string str = "Database=user;Data Source=localhost;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            return conn;
        }

        public int AddUser(){
            MySqlConnection conn = GetUserConn();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from account", conn);
                reader = cmd.ExecuteReader();
                int id = 1;
                if (reader.Read())
                {
                    id = reader.GetInt32(1);
                }
                conn.Close();
                conn.Open();
                cmd = new MySqlCommand("update account set account = " + (id + 1), conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Add User Error:\n" + e);
                conn.Close();
            }

            return 0;
        }

        public void SaveTotalRank(User[] rank){
            MySqlConnection conn = GetUserConn();
            try
            {
                for (int i = 0; i < rank.Length; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("update rank set userid="+rank[i].Id+",name = ?Name,place="+rank[i].Place+",level="+rank[i].Level+",score="+rank[i].Score+" where id=" + (i+1) + ";", conn);
                    cmd.Parameters.Add(new MySqlParameter("Name", rank[i].Name));
                    cmd.ExecuteNonQuery();
                }
                //Console.WriteLine("Rank Saved!" );
            }
            catch (Exception e)
            {
                Console.WriteLine("Saving Rank Error!\n" + e);
            }
            finally {
                conn.Close();
            }
        }


        public void SetServerTotalRank(ref User[] ranks)
        {
            MySqlConnection conn = GetUserConn();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from rank", conn);
                reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    if (i >= ranks.Length)
                        break;
                    int id = reader.GetInt32(1);
                    string name = reader.GetString(2);
                    int place = reader.GetInt32(3);
                    int level = reader.GetInt32(4);
                    int score = reader.GetInt32(5);
                    ranks[i] = new User(id, name, place, level, score);
                    i++;
                }
                //Console.WriteLine("Rank Inited!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Init Rank Error\n" + e);
            }
            finally {
                conn.Close();
            }
        }



    }
}
