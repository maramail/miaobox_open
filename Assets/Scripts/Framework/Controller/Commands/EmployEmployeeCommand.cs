using PureMVC.Interfaces;
using PureMVC.Patterns;

class EmployEmployeeCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        EmployeeProxy employeeProxy = Facade.RetrieveProxy(EmployeeProxy.NAME) as EmployeeProxy;
        employeeProxy.EmployEmployee((int)notification.Body);
    }
}

