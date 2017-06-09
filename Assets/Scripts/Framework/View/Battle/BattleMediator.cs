using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine;



public    class BattleMediator : Mediator, IMediator
{

    public new const string NAME = "BattleMediator";
    public const string BATTLE_CAT_GROUP_INFO = "battlecatGroupInfo";


    public BattleView BattleView { get { return ViewComponent as BattleView; } }
    public  BattleMediator(): base(NAME)
     {
        
        
     }
    public override IEnumerable<string> ListNotificationInterests
    {

        get
        {
            List<string> list = new List<string>();
            list.Add(BATTLE_CAT_GROUP_INFO);

            return list;
        }

    }
    public override void HandleNotification(INotification notification)
    {
        BattleView uview = ViewComponent as BattleView;
        switch (notification.Name)
        {
            case BATTLE_CAT_GROUP_INFO:
                Debug.Log("init battle view ");
                uview.ShowCurrentCatGroup(notification.Body);
                break;
        }

    }
}


