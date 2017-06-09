using PureMVC.Interfaces;
using PureMVC.Patterns;

class AddEmployeeModelCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        EmployeeProxy employeeProxy = Facade.RetrieveProxy(EmployeeProxy.NAME) as EmployeeProxy;
        employeeProxy.AddEmployeeModel((int)notification.Body);
    }
}

