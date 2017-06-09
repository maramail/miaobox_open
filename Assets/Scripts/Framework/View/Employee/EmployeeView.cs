using UnityEngine;
using System.Collections;

public class EmployeeView : MonoBehaviour {

    public GameObject employPanel;
    public GameObject yesBTN;
    public GameObject cancelBTN;
    public GameObject catPosition;
   
    public UILabel eName;
    public UILabel eLevel;
    public UILabel eIq;
    public UILabel eAtk;
    public UILabel eReact;
    public UILabel eSkill;
    public UILabel eHireprice; 

    public GameObject[] bornPoints;
    public GameObject[] disappearPoints;

    public EmployeeInfo currentClickCatInfo;

    public bool employeesussce = false;

    //雇员刷新时间数据 暂时不写到Proxy中
    private float refreshTime = 0f;
    private float currentTime = 0f;
    private float lastTime = 0f;

	// Use this for initialization
	void Start () {

         
        refreshTime = Random.Range(10f,15f);

        currentTime = Time.realtimeSinceStartup;
        lastTime = Time.realtimeSinceStartup;

        UIEventListener.Get(cancelBTN).onClick += (go) => 
        {
            GameObject employeeCat = catPosition.transform.GetChild(0).gameObject;
            Destroy(employeeCat.GetComponent<EmployeeRandomAnimation>());
            employeeCat.layer = 8;
            employeeCat.transform.SetChildLayer(8);
            employeeCat.SetActive(false);
            employPanel.SetActive(false);     
        };

        UIEventListener.Get(yesBTN).onClick += (go) =>
        {
            GameObject employeeCat = catPosition.transform.GetChild(0).gameObject;
            Destroy(employeeCat.GetComponent<EmployeeRandomAnimation>());
            employeeCat.layer = 8;
            employeeCat.transform.SetChildLayer(8);

            //发送消息 存入info_cats数据库(如果不够钱直接返回付不起)
            AppFacade.Instance.SendNotification(NotiConst.EMPLOY_EMPLOYEE, currentClickCatInfo.Id);
        };

    }
     

    //10~15秒 生出来一只小雇员
    void FixedUpdate() {
        currentTime = Time.realtimeSinceStartup;
        if (currentTime - lastTime > refreshTime)
        {
            SendAddEmployeeCommand();

            refreshTime = Random.Range(10f, 15f);
            lastTime = Time.realtimeSinceStartup;
        }
    }

    public void GenerateEmployee(EmployeeInfoVO employeeVO)
    {
        //设置出生点和消失点
        int bornPointIndex = Random.Range(0, 4);
        Vector3 bornPoint = bornPoints[bornPointIndex].transform.position;
        Vector3 disappearPoint = disappearPoints[bornPointIndex].transform.position;

        GameObject employee = CatPool.GetInstance().GetCatPool(employeeVO.Id).CreateObject(bornPoint);
        EmployeeCtl employeeCtl = employee.AddComponent<EmployeeCtl>();
        employeeCtl.bornPosition = bornPoint;
        employeeCtl.disappearPosition = disappearPoint;
        EmployeeInfo employeeInfo = employee.AddComponent<EmployeeInfo>();
        employeeInfo.Id = employeeVO.Id;
        employeeInfo.Name = employeeVO.Name;
        employeeInfo.Level = employeeVO.Level;
        employeeInfo.Evo = employeeVO.Evo;
        employeeInfo.Iq = employeeVO.Iq;
        employeeInfo.Atk = employeeVO.Atk;
        employeeInfo.React = employeeVO.React;
        employeeInfo.Skill = employeeVO.Skill;
        employeeInfo.About = employeeVO.About;
        employeeInfo.Hireprice = employeeVO.HirePrice;
    }

    public void   EmploySuccess()
    {
        Debug.Log("雇佣成功!");
        string s = "雇佣成功!";
        NotifactionFunc._instance.showNotifaction(s);


        employeesussce = true;
        GameObject employeeCat = catPosition.transform.GetChild(0).gameObject;
        employeeCat.SetActive(false);
        employPanel.SetActive(false);
       
    }

    public void  EmployFailure(object data)
    {

        EnumGlobal.FailType fialtype = (EnumGlobal.FailType)data;
        if (fialtype == EnumGlobal.FailType.Money)
        {
            Debug.Log("金钱不够!雇佣失败!");
            string s = "金钱不够!雇佣失败!";
            NotifactionFunc._instance.showNotifaction(s);

        } else if (fialtype == EnumGlobal.FailType.LimitNum)
        {
            string s = "仓库已经达到最大限制数，雇佣失败！请清除库存猫成员后雇佣！";
            NotifactionFunc._instance.showNotifaction(s);

        }
       


        GameObject employeeCat = catPosition.transform.GetChild(0).gameObject;
        employeeCat.SetActive(false);
        employPanel.SetActive(false);
        
    }

    private void SendAddEmployeeCommand()
    {
        AppFacade.getInstance.SendNotification(NotiConst.ADD_EMPLOYEE_MODEL, Random.Range(5, 8));
    }



}
