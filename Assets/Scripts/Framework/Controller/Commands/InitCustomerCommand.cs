using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class InitCustomerCommand : SimpleCommand {

    public override void Execute(INotification notification) {
        //base.Execute(notification);
        //暂时用Find，没想到好办法
        GameObject customerViewObj = GameObject.FindGameObjectWithTag(TagName.CUSTOMER_VIEW);
        
        CustomerMediator customerMediator = new CustomerMediator();
        CustomerView customerView = customerViewObj.GetComponent<CustomerView>();
        customerMediator.ViewComponent = customerView;

        Facade.RegisterProxy(new CustomerProxy());
        Facade.RegisterMediator(customerMediator);

        customerView.enabled = true;

    }
}
