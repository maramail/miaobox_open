/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:25
	filename: 	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View\MainMenu\MainMenuUIView.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View\MainMenu
	file base:	MainMenuUIView
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	MainUI的View
*********************************************************************/
using UnityEngine;
using LitJson;
using System.Collections;

public class MainMenuUIView : MonoBehaviour {

    public int Gold = 0;
    public int Diamond = 0;
    public int Stamina = 0;
    public int Exp = 0;

    public GameObject mainMenuGameObject;

    public GameObject BattleBtn;
    public GameObject CatGroupBtn;
    public GameObject CatHRBtn;
    public GameObject PromoteBtn;
    public GameObject PromoteBtn_Exp;
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public UILabel GoldValueLabel;
    public UILabel DiamondValueLabel;
    public UILabel StaminaValueLabel;
    public UILabel ExpValueLabel;

    void Start()
    {
      
    }

    void OnEnable()
    {
        mainMenuGameObject.SetActive(true);
        UIEventListener.Get(BattleBtn).onClick += OnBtnClick;
        UIEventListener.Get(CatGroupBtn).onClick += OnBtnClick;
        UIEventListener.Get(CatHRBtn).onClick += OnBtnClick;
        UIEventListener.Get(PromoteBtn).onClick += OnBtnClick;
        UIEventListener.Get(PromoteBtn_Exp).onClick += OnBtnClick;
        UIEventListener.Get(LeftBtn).onClick += OnBtnClick;
        UIEventListener.Get(RightBtn).onClick += OnBtnClick;
       
        AppFacade.GetInstance().SendNotification(NotiConst.GET_USER_INFO_VALUE);
    

    }

    void Update()
    {
        GoldValueLabel.text = Gold.ToString();
        DiamondValueLabel.text = Diamond.ToString();
        StaminaValueLabel.text = Stamina.ToString();
        ExpValueLabel.text = Exp.ToString();

    }

    void OnBtnClick(GameObject go) {
        if (go == CatGroupBtn)
        {
            Debug.Log(" CatGroupBtn");
            AppFacade.getInstance.SendNotification(NotiConst.GET_CAT_GROUP_DATA);
            CatGroupMenuMediator catGroupMediator = AppFacade.getInstance.RetrieveMediator(CatGroupMenuMediator.NAME) as CatGroupMenuMediator;
            //            catGroupMediator.catGroupMenuView.catGroupMenuGameObject.SetActive(true);
            //0107 lgx 修改 显示猫分组不能这就草草的显示界面就行需要准备
            catGroupMediator.catGroupMenuView.menutype = EnumGlobal.MenuType.CatGroupMenu;
            catGroupMediator.catGroupMenuView.showCatGroup();

        }
        else if (go == PromoteBtn)
        {
            CustomerMediator customerMediator = AppFacade.GetInstance().RetrieveMediator(CustomerMediator.NAME) as CustomerMediator;
            (customerMediator.ViewComponent as CustomerView).RemoveCustomer();
           
            //AppFacade.getInstance.SendNotification(NotiConst.CAT_LEAVE_ADD_GOLD);
            //StartCoroutine(SpawnCats());
        }
        else if (go == PromoteBtn_Exp)
        {
            SetExp(Exp + 1000);
        }else if (go ==BattleBtn)
        {

            (AppFacade.getInstance.RetrieveMediator(SceneMediator.NAME) as SceneMediator).LoadScene(SceneConst.BattleScene);
                 

        }
    }

    public void SetGold(int count)
    {
       
        AppFacade.GetInstance().SendNotification(NotiConst.SET_GOLD, count);
    }
    public void SetExp(int count)
    {
 
        AppFacade.GetInstance().SendNotification(NotiConst.SET_EXP, count);

    }
    /*
    IEnumerator SpawnCats() {
        for (int i = 0; i < mSpawnNum; i++) {
            AppFacade.GetInstance().SendNotification(NotiConst.SPAWN_CAT);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }*/
}
