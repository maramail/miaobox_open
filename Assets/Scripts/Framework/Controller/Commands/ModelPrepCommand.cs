/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:19
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands\modelprepcommand.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\commands
	file base:	modelprepcommand
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	Proxy注册Command
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class ModelPrepCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        Facade.RegisterProxy(new LoginProxy());
        Facade.RegisterProxy(new UserInfoProxy());
    }
}