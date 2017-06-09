using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class AddChangeModelCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        UserInfoProxy user = AppFacade.GetInstance().RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy;
        BuildModelVo vo = notification.Body as BuildModelVo;
        user.setfacility(vo);
        
        //设置模型与位置信息；
    }
}
