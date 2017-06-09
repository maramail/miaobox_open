using PureMVC.Interfaces;
using PureMVC.Patterns;

class AddCustomerModelCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        CustomerProxy customerProxy = Facade.RetrieveProxy(CustomerProxy.NAME) as CustomerProxy;
        customerProxy.AddCustomerModel((int)notification.Body);
    }
}

