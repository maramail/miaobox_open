/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:25
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view\login\loginmediator.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view\login
	file base:	loginmediator
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	LoginView的Mediator
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class LoginViewMediator : Mediator, IMediator
{
    /// <summary>
    /// 用于登陆UI和注册UI的切换
    /// </summary>
    public enum ELoginViewState
    {
        LOGIN,
        REGISTER,
    }

    public new const string NAME = "LoginViewMediator";

    public const string LOGIN_RESPONSE = "LOGIN_RESPONSE";
    public const string REGISTER_RESPONSE = "REGISTER_RESPONSE";

    public LoginViewMediator() : base(NAME)
    {

    }

    //需要监听的消息号
    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add(LoginViewMediator.LOGIN_RESPONSE);
            list.Add(LoginViewMediator.REGISTER_RESPONSE);
            return list;
        }
    }
    //接收消息到消息之后处理
    public override void HandleNotification(INotification notification)
    {
        string name = notification.Name;
        
        switch (name)
        {
            case LoginViewMediator.LOGIN_RESPONSE:
                (this.ViewComponent as LoginView).ReceiveLoginMessage(notification.Body as TempLogRegDataVO);
                break;
            case LoginViewMediator.REGISTER_RESPONSE:
                (this.ViewComponent as LoginView).ReceiveRegisterMessage(notification.Body as TempLogRegDataVO);
                break;
        }
    }
}