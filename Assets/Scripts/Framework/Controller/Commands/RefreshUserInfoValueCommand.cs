/**********
 * 
 *  Liu Qiran
 * 
 **********/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

class RefreshUserInfoValueCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        UserInfoProxy userInfoProxy = (UserInfoProxy)Facade.RetrieveProxy(UserInfoProxy.NAME);
        switch (notification.Name)
        {
            case NotiConst.GET_USER_INFO_VALUE:
                
                userInfoProxy.RefreshUserInfoValue();
                break;
            case NotiConst.SET_GOLD:
                
                userInfoProxy.SetGold((int)notification.Body);
                break;
            case NotiConst.SET_EXP:
                userInfoProxy.setEXP((int)notification.Body);
                 
                break;
            default: break;
        }
    }
}
