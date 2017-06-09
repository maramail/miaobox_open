using UnityEngine;
using System.Collections;

public class BuildingBluepointCtrl : MonoBehaviour {

    /// <summary>
    /// 猫粮的ID 用于区分点击的位置
    /// </summary>
    public int foodBowID ;  
    /// <summary>
    /// 建筑是否已经建设
    /// </summary>
    public bool isBuild;
    /// <summary>
    /// 建筑位置ID
    /// </summary>
    public int modeltrsID;

	void Start () {
        UIEventListener.Get(gameObject).onClick += onBtnClick;
	}
	
	
    private void onBtnClick(GameObject go)
    {
        Debug.Log("onclick  onBtnClick BuildingBluepointCtrl");
        BuildingBlueprintMediator mediator = AppFacade.GetInstance().RetrieveMediator(BuildingBlueprintMediator.NAME) as BuildingBlueprintMediator;
        mediator.buldingBlueprint.isBulid = isBuild;
        mediator.buldingBlueprint.modeltrsid = modeltrsID;
//        mediator.buldingBlueprint.foodBowID = foodBowID;
//        mediator.buldingBlueprint.gameObject.SetActive(true);
//        mediator.buldingBlueprint.showBulidingTitle();
        mediator.buldingBlueprint.showBuildingBlueprint(foodBowID);
        mediator.buldingBlueprint.menutype = EnumGlobal.MenuType.BuildingBlueprintMenu;

    }  
    void OnEnable()
    {
        UIEventListener.Get(gameObject).onClick += onBtnClick;
    }

    void OnDisable()
    {
        UIEventListener.Get(gameObject).onClick -= onBtnClick;
    }


}
