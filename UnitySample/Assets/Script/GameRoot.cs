using ClientFramework;
using ClientFramework.Request;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientFramework.Tool.Singleton;
public class GameRoot : MonoBehaviour {

    private ClientManage clientManage = new ClientManage();
    private LoginRequest loginRequest;
    private RegisterRequest registerRequest;

    public InputField nameInput;
    public InputField pwdInput;
    public Text ButtonTexxt;
    public bool isLoginOrRegister = true;

    public static GameRoot Instance;
    public bool isShowTip = false;
    public string TipText = "";
    public void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        clientManage.OnInit();
        loginRequest = new LoginRequest();
        registerRequest = new RegisterRequest();
    }
	
	// Update is called once per frame
	void Update () {
		if(isShowTip)
        {
            ShowTip(TipText);
            isShowTip = false;
        }
	}
    public void LoginToggle(bool isOn)
    {
        if(isOn)
        {
            isLoginOrRegister = true;
            ButtonTexxt.text = "Login";
        }
    }
    public void RegisterToggle(bool isOn)
    {
        if (isOn)
        {
            isLoginOrRegister = false;
            ButtonTexxt.text = "Register";
        }
    }
    public void ButtonOnClick()
    {
        if(nameInput.text == "" || pwdInput.text == "")
        {
            ShowTip("用户名或者密码不能为空");
            return;
        }
        if(isLoginOrRegister)
          loginRequest.SendRequest(clientManage,nameInput.text,pwdInput.text);
        else
          registerRequest.SendRequest(clientManage, nameInput.text, pwdInput.text);
    }
    #region 显示提示
    public Text tipTex;
    public void ShowTip(string txt)
    {
        tipTex.text = txt;
        tipTex.transform.parent.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        tipTex.transform.parent.gameObject.SetActive(false);
    }
    #endregion
}
