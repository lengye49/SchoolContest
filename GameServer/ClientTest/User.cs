using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Place { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        public User(int id, string name, int place,int level, int score)
        {
            this.Id = id;
            this.Name = name;
            this.Place = place;
            this.Level = level;
            this.Score = score;
        }

        public string GetUserStr() {
            string s = "";
            s += this.Id + ",";
            s += this.Name + ",";
            s += this.Place + ",";
            s += this.Level + ",";
            s += this.Score;
            Console.WriteLine("UserString" + s);
            return s;
        }
    }
}
