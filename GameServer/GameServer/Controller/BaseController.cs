using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;

        public RequestCode RequestCode {
            get { return requestCode; }
        }

        public virtual string DefaultHandler(string data,Client client,Server server) {
            return null;
        }
    }
}
