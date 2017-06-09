/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:25
	filename: 	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View\MainMenu\MainMenuUIMediator.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View\MainMenu
	file base:	MainMenuUIMediator
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	Main UI的Mediator
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class MainMenuUIMediator : Mediator, IMediator
{
    public new const string NAME = "MainMenuUIMediator";

    public const string REFRESH_USER_INFO_VALUE = "REFRESH_USER_INFO_VALUE";



    public MainMenuUIMediator() : base(NAME)
    {

    }
    //需要监听的消息号
    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add(MainMenuUIMediator.REFRESH_USER_INFO_VALUE);
            return list;
        }
    }
    //接收消息到消息之后处理
    public override void HandleNotification(INotification notification)
    {
        MainMenuUIView uiView = ViewComponent as MainMenuUIView;
        switch (notification.Name)
        {
            case MainMenuUIMediator.REFRESH_USER_INFO_VALUE:
                {
                    uiView.Gold = (notification.Body as UserInfoVO).Gold;
                    uiView.Diamond = (notification.Body as UserInfoVO).Diamond;
                    uiView.Exp = (notification.Body as UserInfoVO).Exp;
                    
                    break;
                }

        }
    }
}
