using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ClientFramework.Request
{
    class LoginRequest : BaseRequest
    {
        public LoginRequest() 
        {
            requestCode = RequestCode.User;
            actionCode = ActionCode.Login;
            RequestManager.Instance.AddRequest(actionCode, this);
        }
        public void SendRequest(ClientManage clientManage,string username, string password)
        {
            string data = username + "," + password;
            base.SendRequest(clientManage,data);
        }
        public override void OnResponse(string reasonCode,string data)
        {
            string[] strs = data.Split(',');
            ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
            if (returnCode == ReturnCode.Success)
            {
                Debug.Log("[Login]成功");
                GameRoot.Instance.TipText = "[Login]成功";
                GameRoot.Instance.isShowTip = true;
            }
            else if (returnCode == ReturnCode.Fail)
            {
                Debug.Log("[Login]失败:" + reasonCode);
                GameRoot.Instance.TipText = "[Login]失败:" + reasonCode;
                GameRoot.Instance.isShowTip = true;
            }
        }
    }
}
