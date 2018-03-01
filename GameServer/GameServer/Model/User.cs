using System;
namespace GameServer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Place { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        public User(int id,string name,int place,int level,int score)
        {
            this.Id = id;
            this.Name = name;
            this.Place = place;
            this.Level = level;
            this.Score = score;
        }

        public User(string userStr) {
            string[] s = userStr.Split(',');
            this.Id= int.Parse(s[0]);
            this.Name = s[1];
            this.Place = int.Parse(s[2]);
            this.Level = int.Parse(s[3]);
            this.Score = int.Parse(s[4]);
        }
    }
}
