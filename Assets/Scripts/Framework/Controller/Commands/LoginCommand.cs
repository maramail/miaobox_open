/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:19
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands\logincommand.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands
	file base:	logincommand
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	登陆Command
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class LoginCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        LoginProxy loginProxy = Facade.RetrieveProxy(LoginProxy.NAME) as LoginProxy;
        switch (notification.Name)
        {
            case NotiConst.LOGIN_REQUEST:
                loginProxy.SendLogin(notification.Body as TempLogRegDataVO);
                break;
            case NotiConst.REGISTER_REQUEST:
                loginProxy.SendRegister(notification.Body as TempLogRegDataVO);
                break;
        }
    }
}