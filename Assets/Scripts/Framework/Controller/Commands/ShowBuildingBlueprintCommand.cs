using System;
using PureMVC.Interfaces;
using UnityEngine;

using PureMVC.Patterns ;


public class ShowBuildingBlueprintCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        var porxy = AppFacade.GetInstance().RetrieveProxy(BuildBlueprintProxy.NAME) as BuildBlueprintProxy;

        switch (notification.Name)
        {
            case NotiConst.GET_BUILDINGBLUEPRINT_DATA:  //发送蓝图信息
                porxy.sendBuildingBlueprintData();
                break;
            case NotiConst.GET_STATBUILD_DATA:   //发送静态建筑信息
                porxy.sendBuildStatData();
                break;
            case NotiConst.GET_BUILDING_DATA:   // 发送建筑信息
                porxy.sendBuildData();
                break;
            case NotiConst.PURCHASE_BLUEPRINT:  //购买蓝图
                porxy.purchaseBlueprint(notification.Body);
                Debug.Log("+++" + notification.Body);
                break;
            case NotiConst.GET_BUILDING_MODEL_DATA:
                Debug.Log("GET_BUILDING_MODEL_DATA~~~~~~~~~~~");
                porxy.sendBulidModeldata();//发送模型初始值信息
                break;
            case NotiConst.GET_CHNAGE_MODEL_DATA:
                BuildModelVo vo = notification.Body as BuildModelVo;
                porxy.sendchangemodeldata(vo );//发送模型改变信息


                break;
        }




    }
}

