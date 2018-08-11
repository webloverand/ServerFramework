using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
                Debug.Log("[Register]成功");
                GameRoot.Instance.TipText = "[Register]成功";
                GameRoot.Instance.isShowTip = true ;
            }
            else if (returnCode == ReturnCode.Fail)
            {
                Debug.Log("[Register]失败:" + reasonCode);
                GameRoot.Instance.TipText = "[Register]失败:" + reasonCode;
                GameRoot.Instance.isShowTip = true;
            }
        }
    }
}
