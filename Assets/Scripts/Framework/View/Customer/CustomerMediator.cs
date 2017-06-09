using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class CustomerMediator : Mediator, IMediator {

    public new const string NAME = "CustomerSpawnMediator";

    public const string REFRESH_CUSTOMER_SPAWN_VALUE = "REFRESH_CUSTOMER_SPAWN_VALUE";
    public const string GENERATE_CUSTOMER = "GENERATE_CUSTOMER";
    

    public CustomerMediator() : base(NAME) {
    }

    public CustomerView customerSpawnView {
        get {
            return ViewComponent as CustomerView;
        }
    }

    public override IEnumerable<string> ListNotificationInterests {
        get {
            List<string> list = new List<string>();
            list.Add(CustomerMediator.REFRESH_CUSTOMER_SPAWN_VALUE);
            list.Add(CustomerMediator.GENERATE_CUSTOMER);
            return list;
        }
    }
    public override void HandleNotification(INotification notification) {
        switch (notification.Name) {
            case CustomerMediator.REFRESH_CUSTOMER_SPAWN_VALUE:
                customerSpawnView.maxCustomer = ((CustomerSpawnVO)notification.Body).MaxCustomer;
                customerSpawnView.currentCustomer = ((CustomerSpawnVO)notification.Body).CurrentCustomer; 
                customerSpawnView.visibleCustomer = ((CustomerSpawnVO)notification.Body).VisibleCustomer;
                customerSpawnView.spawnNum = ((CustomerSpawnVO)notification.Body).SpawnNum; 
                customerSpawnView.refreshTime = ((CustomerSpawnVO)notification.Body).RefreshTime;
                break;
            case CustomerMediator.GENERATE_CUSTOMER:
                customerSpawnView.GenerateCustomer((CustomerInfoVO)notification.Body);
                break;
            default:
                break;
        }
    }
}
