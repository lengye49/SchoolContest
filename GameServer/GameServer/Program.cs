using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GameServer.Servers;
using GameServer.Controller;

namespace GameServer
{
    class Program
    {
        static System.Timers.Timer timer;
        static Server server;
        static ControllerManager controllerManager;
        static void Main(string[] args)
        {
            server = new Server("127.0.0.1", 7457);
            server.StartServer();
            controllerManager = server.Controller;
            InitTimer();
            LoadRanks();
            while (true)
            {
                Console.ReadKey();
            }
        }

        static void LoadRanks(){
            controllerManager.HandleRequest(Common.RequestCode.Game, Common.ActionCode.InitRanks, null, null);
        }

        /// <summary>
        /// 计时5分钟保存排行
        /// </summary>
        static void InitTimer(){
            int interval = 300000;
            timer = new System.Timers.Timer(interval);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(StoreRanks);
        }

        static void StoreRanks(object sender,System.Timers.ElapsedEventArgs e){
            Console.WriteLine("正在保存排行信息...");
            controllerManager.HandleRequest(Common.RequestCode.Game, Common.ActionCode.SaveRanks, null, null);
            Console.WriteLine("保存成功！");
        }
    }
}
