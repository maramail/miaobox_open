using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine;
using LitJson;


class ShowBattleCatGropCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        Debug.Log("battle ");
        CatGroupProxy proxy = Facade.RetrieveProxy(CatGroupProxy.NAME) as CatGroupProxy;
        Debug.Log(notification.Body);
        JsonData data = new JsonData();
        object ob = notification.Body;
        data  = (JsonData)ob;
        int i = (int )data["id"];


        Debug.Log(i);

        proxy.SendBattleGroupInfo(notification.Body);


    }
    }
 
