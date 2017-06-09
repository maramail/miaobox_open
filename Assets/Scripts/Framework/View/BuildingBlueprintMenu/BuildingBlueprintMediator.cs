using System;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class BuildingBlueprintMediator : Mediator, IMediator
{
    public new const string NAME = "BuildingBlueprintMediator";
    /// <summary>
    /// 获取蓝图信息
    /// </summary>
    public const string BUILDING_BLUEPRINT_DATA = "BUILDING_BLUEPRINT_DATA";
    public const string BUILD_STAT_DATA = "BUILD_STAT_DATA";
    /// <summary>
    /// 获取建筑信息
    /// </summary>
    public const string BUILDING_DATA = "BUILDING_DATA";

    public const string PURCHASE_BLUEPRINT_RESULT = "PURCHASE_BLUEPRINT_RESULT";
    
   



    public BuildingBlueprintMenu buldingBlueprint
    {
        get {
            return m_viewComponent as BuildingBlueprintMenu;
        }
    }

  
    // 
    public  BuildingBlueprintMediator():base(NAME)
    {
    
    }

    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add(BuildingBlueprintMediator.BUILDING_BLUEPRINT_DATA);
            list.Add(BuildingBlueprintMediator.BUILDING_DATA);
            list.Add(BuildingBlueprintMediator.BUILD_STAT_DATA);
            list.Add(BuildingBlueprintMediator.PURCHASE_BLUEPRINT_RESULT);
            
            return list;
        }
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case BuildingBlueprintMediator.BUILDING_BLUEPRINT_DATA:
                {
                    buldingBlueprint.setBuildingBlueprintData(notification.Body);
                }
                break;
            case BuildingBlueprintMediator.BUILD_STAT_DATA:
                {
                    buldingBlueprint.setBuildStatData(notification.Body);

                }
                break;
            case BuildingBlueprintMediator.BUILDING_DATA:
                {
                   
                    buldingBlueprint.setBuildingData((string)notification.Body);
                }
                break;
            case BuildingBlueprintMediator.PURCHASE_BLUEPRINT_RESULT:
                {
                    buldingBlueprint.PurchaseBlueprintResult((bool)notification.Body);
                }
                break;
            
        }
    }



}


