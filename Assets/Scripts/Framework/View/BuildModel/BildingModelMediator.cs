using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

/*
 * 
 * 
 * 类名：BuildingModelMediator
 * 
 * 
 * 
 * 作用：监听和处理初始化模型和改变模型的事件
 * 
 * 
 * 
 * 时间  2017/1/6
 * 
 * */
public class BuildingModelMediator : Mediator,IMediator {


    public new const string NAME = "BUILDCHANGEMODELMEDIATOR";

    public const string INITBUILTMODEL = "INITBULITMODEL";             //加载更新模型消息
    public const string CHANGEMODEL = "CHANGEMODEL";                   //改变模型消息

    public BuildingChangeCtrl buildchangeModel
    {
        get
        {

            return m_viewComponent as BuildingChangeCtrl;
        }
    }
    public BuildingModelMediator():base (NAME)
    {

    }

    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
             
            list.Add(BuildingModelMediator.INITBUILTMODEL);
            list.Add(BuildingModelMediator.CHANGEMODEL);
            return list;
        }
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
             
            case BuildingModelMediator.INITBUILTMODEL:

                {
                   
                    buildchangeModel.showinitmodel(notification.Body);
                }
                break;
            case BuildingModelMediator.CHANGEMODEL:
                {
                    BuildModelVo vo = new BuildModelVo();
                    vo = notification.Body as BuildModelVo;
                    buildchangeModel.ChangeBulit(vo);

                }
                break;
        }
    }
}
