using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomerView : MonoBehaviour {

    public GameObject spawnObject;
    public GameObject jumpStartObject;
    public GameObject jumpEndObject;
    public GameObject centerObject;
    public GameObject[] goAwayObjects;

    public int maxCustomer = 0;
    public int currentCustomer = 0;
    public int visibleCustomer = 0;
    public int spawnNum = 0;
    public float refreshTime = 9999f; 

    private List<GameObject> customerLists = new List<GameObject>();
    private float tempTime = 0f;
    private float timeTag = 0f;

    /// <summary>
    /// 每次添加一批顾客之后，刷新一下数据
    /// </summary>
    public void FixedUpdate() {
        tempTime = Time.realtimeSinceStartup;
        if (tempTime - timeTag > refreshTime) {
          AddCustomer();
          timeTag = tempTime;
       }
    }

    public void Start() {
        AppFacade.getInstance.SendNotification(NotiConst.GET_CUSTOMER_SPAWN_VALUE);

        tempTime = Time.realtimeSinceStartup;
        timeTag = Time.realtimeSinceStartup;

        //补齐已有顾客
        if (currentCustomer > 0 && currentCustomer < visibleCustomer)
        {
            for (int i = 0; i < currentCustomer; i++)
            {
                SendRandomAddCustomerCommand();
            }
        }
        else if (currentCustomer >= visibleCustomer)
        {
            for (int i = 0; i < visibleCustomer; i++)
            {
                SendRandomAddCustomerCommand();
            }
        }
        else
        { }
    }

    private void AddCustomer() {
        for (int i = 0; i < spawnNum; i++)
        {
            //不管是否增加模型对象，顾客数都+1
            if (customerLists.Count < visibleCustomer)
            {
                SendRandomAddCustomerCommand();
            }
            
        }

        AppFacade.getInstance.SendNotification(NotiConst.SET_CURRENT_CUSTOMER, currentCustomer + spawnNum);

        
    }

    public void RemoveCustomer()
    {
        if (currentCustomer > 0)
        {
            customerLists[0].GetComponent<CustomerCtl>().LeaveManor();
            int Money = customerLists[0].GetComponent<CustomerInfo>().Money;
            customerLists.RemoveAt(0);
            AppFacade.getInstance.SendNotification(NotiConst.SET_CURRENT_CUSTOMER, currentCustomer - 1);
            //补齐显示的顾客
            if (currentCustomer >= visibleCustomer)
            {
                SendRandomAddCustomerCommand();
            }

            MainMenuUIMediator mainMenuUIMediator = (MainMenuUIMediator)AppFacade.GetInstance().RetrieveMediator(MainMenuUIMediator.NAME);
            MainMenuUIView mainMenuUIView = (MainMenuUIView)mainMenuUIMediator.ViewComponent;
            mainMenuUIView.SetGold(mainMenuUIView.Gold + Money);
        }

    }

    public void GenerateCustomer(CustomerInfoVO customerVO)
    {
        GameObject customer = CatPool.GetInstance().GetCatPool(customerVO.Id).CreateObject(spawnObject.transform.position);
        CustomerCtl customerCtl = customer.AddComponent<CustomerCtl>();
        customerCtl.jumpStartObject = jumpStartObject;
        customerCtl.jumpEndObject = jumpEndObject;
        customerCtl.centerObject = centerObject;
        customerCtl.goAwayObjects = goAwayObjects;
        CustomerInfo customerInfo = customer.AddComponent<CustomerInfo>();
        customerInfo.Money = customerVO.Money;
        customerLists.Add(customer);
    }

    private void SendRandomAddCustomerCommand()
    {
        AppFacade.getInstance.SendNotification(NotiConst.ADD_CUSTOMER_MODEL,Random.Range(5,8));
    }

}