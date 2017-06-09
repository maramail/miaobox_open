/**********
 * 
 *  Liu Qiran
 * 
 **********/

using PureMVC.Interfaces;
using PureMVC.Patterns;

class RefreshManorValueCommand: SimpleCommand
{
    public override void Execute(INotification notification)
    {
        CustomerProxy customerProxy = (CustomerProxy)Facade.RetrieveProxy(CustomerProxy.NAME);
        switch (notification.Name)
        {
            case NotiConst.GET_CUSTOMER_SPAWN_VALUE:
                customerProxy.RefreshCustomerSpawnValue();
                break;
            case NotiConst.SET_CURRENT_CUSTOMER:
                customerProxy.SetCurrentCustomer((int)notification.Body);
                break;
            default: break;
         }
    }
}
