/********************************************************************
	created:	2016/08/23
	created:	23:8:2016   22:06
	filename: 	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\View\CatGroupMenu\CatGroupMenuMediator.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\View\CatGroupMenu
	file base:	CatGroupMenuMediator
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	CatGroupMenuView的Mediator
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;


public class CatGroupMenuMediator : Mediator, IMediator
{
    public new const string NAME = "CatGroupMenuMediator";

    // 猫分组信息
    public const string CAT_GROUP_TITLE      = "catGroupTitle";     //不需要请求了
    // 猫移动分组信息
    public const string CAT_MOVE_GROUP_TITLE = "catMoveGroupTitle";
    // 猫分组内容信息
    public const string CAT_GROUP_INFO       = "catGroupInfo";

    public const string CAT_SWITCH_GROUP     = "catSwitchGroup";

    public const string CAT_SWITCH_GROUP_FAIL = "catswitchgroupfail";

    public const string CAT_DELETE          = "catDelete";

    public CatGroupMenuView catGroupMenuView { get { return ViewComponent as CatGroupMenuView; } }

    public CatGroupMenuMediator() : base(NAME)
    {

    }
    //需要监听的消息号
    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add(CAT_GROUP_TITLE);
            list.Add(CAT_GROUP_INFO);
            list.Add(CAT_MOVE_GROUP_TITLE);
            list.Add(CAT_SWITCH_GROUP);
            list.Add(CAT_SWITCH_GROUP_FAIL);
            list.Add(CAT_DELETE);
            return list;
        }
    }
    //接收消息到消息之后处理
    public override void HandleNotification(INotification notification)
    {
        CatGroupMenuView uiView = ViewComponent as CatGroupMenuView;
        switch (notification.Name) {
            case CAT_GROUP_TITLE:
//                uiView.ShowCatGroupTitle(notification.Body);
                break;
            case CAT_GROUP_INFO:
                uiView.ShowCatGroupInfo(notification.Body);
                break;
            case CAT_MOVE_GROUP_TITLE:
                uiView.CatMoveTitle(notification.Body);
                break;
            case CAT_SWITCH_GROUP:
                uiView.SwitchCatGroup(notification.Body);
                break;
            case  CAT_SWITCH_GROUP_FAIL:
                uiView.limitInfo();
                break;
            case CAT_DELETE:
                uiView.CatDeleteFinsh(notification.Body);
                break;
            default:break;
        }
    }
}
