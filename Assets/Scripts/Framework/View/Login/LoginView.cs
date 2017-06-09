/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:26
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view\login\loginview.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view\login
	file base:	loginview
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	Login View
*********************************************************************/
using UnityEngine;
using System.Collections;
using LitJson;

/// <summary>
/// 点击登录按钮会请求登录，然后发送LOGIN_REQUEST请求登录消息和数据(账号密码)到LoginCommand，LoginCommand调用LoginProxy
/// </summary>
public class LoginView : MonoBehaviour
{
    public GameObject LoginGameObject;
    public GameObject LoginBtn;
    public UIInput    UserName;
    public UIInput    PassWord;
    public UIInput    ConfirmPassWord;
    public GameObject RegisterBtn;
    public GameObject OKBtn;
    public GameObject ReturnBtn;
    public GameObject QuitBtn;
    public UILabel    Message;
    private LoginViewMediator.ELoginViewState mState = LoginViewMediator.ELoginViewState.LOGIN;

    //用于登录与注册时临时数据传输（代替Json）
    public TempLogRegDataVO tempData;

    void Awake()
    {
    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        LoginGameObject.SetActive(true);
        UIEventListener.Get(LoginBtn).onClick += OnBtnClick;
        UIEventListener.Get(RegisterBtn).onClick += OnBtnClick;
        UIEventListener.Get(OKBtn).onClick += OnBtnClick;
        UIEventListener.Get(ReturnBtn).onClick += OnBtnClick;
        UIEventListener.Get(QuitBtn).onClick += OnBtnClick;
        //输入过账号密码 自动请求登录
        InputLoginInfoAndTryLogin();
    }


    public void ReceiveLoginMessage(TempLogRegDataVO vo)
    {
        //JsonData data = vo as JsonData;
        Message.text  = "";
        //if ((int)data[JsonConst.ErrorCode] == ErrorCode.SUCCESS)
        if(vo.status == ErrorCode.SUCCESS)
        {
            Debug.Log("登陆成功");
            (AppFacade.getInstance.RetrieveMediator(SceneMediator.NAME) as SceneMediator).LoadScene(SceneConst.GameScene);
            //AppFacade.GetInstance().SendNotification(SceneMediator.LOAD_SCENE, SceneConst.GameScene);
        }
        else
        {
            Message.text = "Failed login UserName or password error";
            Debug.Log("登陆失败");
        }
    }
    public void ReceiveRegisterMessage(TempLogRegDataVO vo)
    {
        //JsonData data = vo as JsonData;
        //if ((int)data[JsonConst.ErrorCode] == ErrorCode.SUCCESS)
        if(vo.status == ErrorCode.SUCCESS)
        {
            Debug.Log("注册成功");
            ChangeState(LoginViewMediator.ELoginViewState.LOGIN);
        }
        else
        {
            Debug.Log("注册失败");
        }
    }
    public void InputLoginInfoAndTryLogin()
    {
        UserName.value = LocalSaveData.LoginUserName;
        PassWord.value = LocalSaveData.LoginPassword;
        if (!string.IsNullOrEmpty(UserName.value) &&
            !string.IsNullOrEmpty(PassWord.value))
        {
            Login();
        }
    }
    public void Login()
    {
        tempData = new TempLogRegDataVO();
        tempData.UserName = UserName.value;
        tempData.Password = PassWord.value;
        AppFacade.GetInstance().SendNotification(NotiConst.LOGIN_REQUEST, tempData);
    }
    public void OnBtnClick(GameObject go)
    {
        if (go == LoginBtn)
        {
            Login();
        }
        else if (go == RegisterBtn)
        {
            // 清空密码输入框 需要重新输入密码
            PassWord.value        = "";
            ConfirmPassWord.value = "";

            ChangeState(LoginViewMediator.ELoginViewState.REGISTER);
        }
        else if (go == OKBtn)
        {
            Message.text = "";
            string errorInfo = "Registration error";
            if (string.IsNullOrEmpty(UserName.value))
            {
                errorInfo = "The user name cannot be empty";

            } else if (string.IsNullOrEmpty(PassWord.value))
            {
                errorInfo = "Password cannot be empty";

            } else if (PassWord.value.Length < 6)
            {
                errorInfo = "Password length cannot be less than 6";

            } else if (string.IsNullOrEmpty(ConfirmPassWord.value))
            {
                errorInfo = "Please input the password again";
            } else if (PassWord.value != ConfirmPassWord.value)
            {
                errorInfo = "Two entered passwords do not match";

            } else
            {
                tempData = new TempLogRegDataVO();
                tempData.UserName = UserName.value;
                tempData.Password = PassWord.value;
                AppFacade.GetInstance().SendNotification(NotiConst.REGISTER_REQUEST, tempData);
                return;
            }

            Message.text = errorInfo;

//            JsonData data2 = new JsonData();
//            data2[MessageView.MessageType] = MessageView.TwoParameters;
//            data2[MessageView.OneParameters] = errorInfo;
//            data2[MessageView.TwoParameters] = gameObject.GetComponent<UIPanel>().depth;
//            AppFacade.GetInstance().SendNotification(NotiConst.MessageMediator, data2);
            //                Debug.LogError("注册信息有误！");

        } else if (go == ReturnBtn)
        {
            // 清空密码输入框 需要重新输入密码
            PassWord.value = "";
            ConfirmPassWord.value = "";
            ChangeState(LoginViewMediator.ELoginViewState.LOGIN);

        } else if (go == QuitBtn)
        {
            //退出游戏
            Application.Quit();
        }
    }
    public void ChangeState(LoginViewMediator.ELoginViewState state)
    {
        if (mState != state)
        {
            mState = state;
            switch(state)
            {
                case LoginViewMediator.ELoginViewState.LOGIN:
                    ConfirmPassWord.gameObject.SetActive(false);
                    OKBtn.SetActive(false);
                    ReturnBtn.SetActive(false);


                    LoginBtn.SetActive(true);
                    RegisterBtn.SetActive(true);
                    QuitBtn.SetActive(true);

                    break;
                case LoginViewMediator.ELoginViewState.REGISTER:
                    ConfirmPassWord.gameObject.SetActive(true);
                    OKBtn.SetActive(true);
                    ReturnBtn.SetActive(true);

                    QuitBtn.SetActive(false);
                    LoginBtn.SetActive(false);
                    RegisterBtn.SetActive(false);

                    break;
            }
        }
    }
}