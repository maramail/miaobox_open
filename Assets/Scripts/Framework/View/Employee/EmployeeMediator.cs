using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class EmployeeMediator : Mediator, IMediator
{

    public new const string NAME = "EmployeeSpawnMediator";

    public const string GENERATE_EMPLOYEE = "GENERATE_EMPLOYEE";
    public const string EMPLOY_SUCCESS = "EMPLOY_SUCCESS";
    public const string EMPLOY_FAILURE = "EMPLOY_FAILURE";


    public EmployeeMediator() : base(NAME)
    {
    }

    public EmployeeView employeeView
    {
        get
        {
            return ViewComponent as EmployeeView;
        }
    }

    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add(EmployeeMediator.GENERATE_EMPLOYEE);
            list.Add(EmployeeMediator.EMPLOY_SUCCESS);
            list.Add(EmployeeMediator.EMPLOY_FAILURE);
            return list;
        }
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case EmployeeMediator.GENERATE_EMPLOYEE:
                employeeView.GenerateEmployee((EmployeeInfoVO)notification.Body);
                break;
            case EmployeeMediator.EMPLOY_SUCCESS:
                employeeView.EmploySuccess();
                break;
            case EmployeeMediator.EMPLOY_FAILURE:

                employeeView.EmployFailure((EnumGlobal.FailType)notification.Body);
                break; 
        }
    }
}
