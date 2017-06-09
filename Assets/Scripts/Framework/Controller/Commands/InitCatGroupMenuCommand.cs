/********************************************************************
	created:	2016/08/23
	created:	23:8:2016   22:15
	filename: 	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\Controller\Commands\ShowCatGroupMenuCommand.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\Controller\Commands
	file base:	ShowCatGroupMenuCommand
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	显示猫分组菜单 初始化猫数据
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class InitCatGroupMenuCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        UIBaseBehaviour<CatGroupMenuMediator>.CreateUI<CatGroupMenuView>();      
    }
}
