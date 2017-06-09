using UnityEngine;
using System.Collections;
using PureMVC.Interfaces;
using PureMVC.Patterns;

public class ShowCatGroupCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        var proxy  = Facade.RetrieveProxy(CatGroupProxy.NAME) as CatGroupProxy;

        switch (notification.Name) {
            case NotiConst.GET_CAT_GROUP_DATA:
                proxy.LoadCatGroupInfo();
                break;
            case NotiConst.GET_CAT_GROUP_TITLE:
                proxy.SendCatGroupTitle(notification.Body);
                break;
            case NotiConst.GET_CAT_GROUP_INFO:
                proxy.SendCatGroupInfo(notification.Body);
                break;
            case NotiConst.CAT_SWITCH_GROUP:
                proxy.SwitchCatGroup(notification.Body);
                break;
            case NotiConst.CAT_DELETE:
                proxy.DeleteCat(notification.Body);
                break;
            case NotiConst.CAT_GROUP_CLOSE:
                proxy.SaveToDb();
                break;
        }

    }
}
