/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:20
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands\showloginmenucommand.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands
	file base:	showloginmenucommand
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	显示LoginUICommand
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class InitLoginMenuViewCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        UIBaseBehaviour<LoginViewMediator>.CreateUI<LoginView>();
    }
}
