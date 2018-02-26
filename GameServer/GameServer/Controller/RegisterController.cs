using System;
using GameServer.Servers;
using Common;
using GameServer.DAO;
namespace GameServer.Controller
{
    class RegisterController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        public RegisterController()
        {
            requestCode = RequestCode.Register;
        }

        public string Register(string data,Client client,Server server){
            int id = userDAO.AddUser();
            return id.ToString();
        }

    }
}
