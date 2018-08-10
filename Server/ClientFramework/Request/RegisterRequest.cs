using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientFramework.Request
{
    class RegisterRequest :BaseRequest
    {
        public RegisterRequest() : base()
        {
            requestCode = RequestCode.User;
            actionCode = ActionCode.Register;
            RequestManager.Instance.AddRequest(actionCode, this);
        }
        public void SendRequest(ClientManage clientManage, string username, string password)
        {
            string data = username + "," + password;
            base.SendRequest(clientManage,data);
        }

        public override void OnResponse(string reasonCode,string data)
        {
            ReturnCode returnCode = (ReturnCode)int.Parse(data);
            if (returnCode == ReturnCode.Success)
            {
                Console.WriteLine("[Register]成功");
            }
            else if (returnCode == ReturnCode.Fail)
            {
                Console.WriteLine("[Register]失败" + reasonCode);
            }
        }
    }
}
