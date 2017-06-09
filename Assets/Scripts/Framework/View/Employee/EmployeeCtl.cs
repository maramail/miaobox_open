using UnityEngine;
using System.Collections;

public class EmployeeCtl : MonoBehaviour {

    public enum EmployeeState
    {
        FALL_OFF,
        MOVE
    }

    public Vector3 bornPosition;
    public Vector3 disappearPosition;

    private float fallSpeed = -0.3f;
    private float moveSpeed = 0.5f;

    private Animation m_animation;
    private CharacterController m_characterController;
    private EmployeeState m_employeeState;
    private EmployeeView _employeeview;

    // Use this for initialization
    void Start () {
        m_animation = this.GetComponent<Animation>();
        m_characterController = this.GetComponent<CharacterController>();
        UIEventListener.Get(gameObject).onClick += ShowEmployeeInfo;

    }

    void OnEnable()
    {
        m_employeeState = EmployeeState.FALL_OFF;
    }

    void OnDisable()
    {
        Destroy(this);
        UIEventListener.Get(gameObject).onClick -= ShowEmployeeInfo;
    }

	// Update is called once per frame
	void Update () {
        switch (m_employeeState)
        {
            case EmployeeState.FALL_OFF:
                if (transform.position.y <= -0.4f)
                {
                    m_employeeState = EmployeeState.MOVE;
                }
                else
                {
                    Gravity();
                }
                break;
            case EmployeeState.MOVE:
                TransMove();
                if ((transform.position - disappearPosition).sqrMagnitude < 0.5f)
                {
                    gameObject.SetActive(false); //隐掉 回收
                }
                break;
            default:break;
        }
        if (_employeeview && _employeeview.employeesussce)    //处理雇佣成功后模型的隐藏
        {

            gameObject.SetActive(false );
        }
	}

    private void Gravity()
    {
        this.gameObject.transform.Translate(new Vector3(0, fallSpeed * Time.deltaTime, 0));
    }

    private void TransMove()
    {
        if (disappearPosition != null && bornPosition != null)
        {
            Vector3 direction = (disappearPosition - bornPosition).normalized;
            gameObject.transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);
            Vector3 lookAtDir = Vector3.RotateTowards(transform.forward, direction, 5f * Time.deltaTime, 10000);
            transform.LookAt(transform.position + lookAtDir);
            m_animation.CrossFade("run", 0.1f);
        }
    }

    private void ShowEmployeeInfo(GameObject go)
    {
        EmployeeInfo employeeInfo = go.GetComponent<EmployeeInfo>();
        EmployeeMediator employeeMediator = AppFacade.getInstance.RetrieveMediator(EmployeeMediator.NAME) as EmployeeMediator;
        _employeeview = employeeMediator.employeeView;

        _employeeview.employeesussce = false;
        _employeeview.currentClickCatInfo = employeeInfo;

        _employeeview.employPanel.SetActive(true);
        GameObject employee = CatPool.GetInstance().GetCatPool(employeeInfo.Id).CreateObject(_employeeview.catPosition.transform.position);
        employee.AddComponent<EmployeeRandomAnimation>();
        employee.layer = 9;
        employee.transform.SetChildLayer(9);
        employee.transform.rotation = _employeeview.catPosition.transform.rotation;
        employee.transform.parent = _employeeview.catPosition.transform;


        _employeeview.eName.text = employeeInfo.Name + " 来应聘了！";
        _employeeview.eLevel.text = "等级：" + employeeInfo.Level.ToString();
        _employeeview.eIq.text = "智力：" + employeeInfo.Iq.ToString();
        _employeeview.eAtk.text = "攻击力：" + employeeInfo.Atk.ToString();
        _employeeview.eReact.text = "反应力：" + employeeInfo.React.ToString();
        _employeeview.eSkill.text = "拥有技能：" + employeeInfo.Skill.ToString();
        _employeeview.eHireprice.text = "佣金：" + employeeInfo.Hireprice.ToString();

    }

}
