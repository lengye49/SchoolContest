using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        public Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server) {
            this.server = server;
            Init();
        }

        void Init()
        {
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(defaultController.RequestCode, defaultController);

            GameController gameController = new GameController();
            controllerDic.Add(gameController.RequestCode, gameController);

            RegisterController registerController = new RegisterController();
            controllerDic.Add(registerController.RequestCode, registerController);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            //1. 获取controller
            BaseController controller;
            bool isGet = controllerDic.TryGetValue(requestCode, out controller);
            if (!isGet)
            {
                //这个应该输出日志
                Console.WriteLine("Cannot find controller of RequestCode：[" + requestCode + "]");
            }

            //2. 获取action并处理数据
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo methodInfo = controller.GetType().GetMethod(methodName);
            if (methodInfo == null)
            {
                Console.WriteLine("Can not find method" + controller.GetType() + "--" + methodName + "].");
                return;
            }
            object[] parameters = new object[] { data, client, server };
            object o = methodInfo.Invoke(controller, parameters);

            if (o == null || string.IsNullOrEmpty(o as string)) {
                return;
            }

            server.SendResponse(client, requestCode, actionCode, o as string);
        }
    }
}
