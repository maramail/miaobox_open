using UnityEngine;
using System.Collections;

/// <summary>
/// 店员猫的AI
/// </summary>
public class CustomerCtl : MonoBehaviour {

    public enum EInnerState {
        FALL_OFF,           //掉落
        MOVE_AROUND,        //游走
        MOVE_TO_OUT_POINT,  //移动到出口
        JUMP_OUT,           //跳出
        GO_CENTER,          //走到中间
        GO_AWAY,            //走出地图
    }

    public GameObject jumpStartObject;     //起跳点
    public GameObject jumpEndObject;       //落点
    public GameObject centerObject;       //中央点
    public GameObject[] goAwayObjects;     //出口数组

    private Vector3 mLastPos;       //用于判断撞墙，也就是如果一直移动不到目标点 那就是撞墙了
    private Vector3 mDeltaDesPos;   //方向
    private Vector3 mDesPos;        //目标点
    private Timer mMove_timer;      //定时器 
    private CharacterController mCc;        
    private Animation mAni;
    private float mFallspeed = 0;
    private EInnerState mInnerState;            //当前状态
    private float mCurrentUpSpeed;              
    private int mRandomGoAwayIndex;             //出口索引

    private const float mMoveLength = 1f;   //步长
    private const float mMoveSpeed = 1.2f;
    private const float g = 0.2f;
    private const float jumpStartSpeed = 6f;

    void Start() {
        mCc = this.GetComponent<CharacterController>();
        mAni = this.GetComponent<Animation>();
        UIEventListener.Get(gameObject).onClick += ClickCat;
    }

    void OnEnable() {
        Init();
    }

    void OnDisable() {
        Destroy(this);
        UIEventListener.Get(gameObject).onClick -= ClickCat;
    }

    public void Init() {
        mDeltaDesPos = Vector3.zero;
        mDesPos = Vector3.zero;
        mLastPos = transform.position;
        mInnerState = EInnerState.FALL_OFF;
        mFallspeed = 0;
    }

    void Update() {
        switch (mInnerState) {
            case EInnerState.FALL_OFF: {
                    if (transform.position.y > -0.1f) {
                        Gravity();
                    } else {
                        mInnerState = EInnerState.MOVE_AROUND;
                        mMove_timer = new Timer(2f, true);
                    }
                    break;
                }
            case EInnerState.MOVE_AROUND: {
                    if (transform.position.y > -0.1f) //地面高度约为-0.1f
                    {
                        mInnerState = EInnerState.FALL_OFF;
                        return;
                    }
                    if (mMove_timer.Ready()) {
                        mDeltaDesPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                        mDesPos = transform.position + mDeltaDesPos * mMoveLength;
                        mMove_timer.Do();
                        //mAni.CrossFade("run", 0.1f);
                    }
                    Move();
                    break;
                }
            case EInnerState.MOVE_TO_OUT_POINT: {
                    Vector3 dir = jumpStartObject.transform.position - transform.position;
                    dir.y = 0f;
                    mDesPos = transform.position + dir;
                    mDeltaDesPos = dir.normalized;
                    //mAni.CrossFade("run", 0.1f);
                    TransMove();
                    //Move();
                    if ((transform.position - mDesPos).sqrMagnitude < 0.01f) {
                        mInnerState = EInnerState.JUMP_OUT;
                        mCurrentUpSpeed = jumpStartSpeed;
                        //AppFacade.GetInstance().SendNotification(NotiConst.CAT_LEAVE_ADD_GOLD);
                    }
                    break;
                }
            case EInnerState.JUMP_OUT: {
                    Vector3 dir = jumpEndObject.transform.position - transform.position;
                    dir.y = 0f;
                    mDesPos = transform.position + dir;
                    mDeltaDesPos = dir.normalized;
                    //mAni.CrossFade("run", 0.1f);
                    TransMove();
                    //Move();
                    if (mCurrentUpSpeed > -jumpStartSpeed) //落地
                    {
                        transform.Translate(Vector3.up * mCurrentUpSpeed * Time.deltaTime);
                        mCurrentUpSpeed -= g;
                    }
                    if ((transform.position - mDesPos).sqrMagnitude < 0.001f) {
                        mRandomGoAwayIndex = Random.Range(0, goAwayObjects.Length);
                        if (goAwayObjects[mRandomGoAwayIndex].name == "GoAway") //除了第一个路口 都需要先走到中间然后拐一下
                        {
                            mInnerState = EInnerState.GO_AWAY;
                        } else {
                            mInnerState = EInnerState.GO_CENTER;
                        }
                    }
                    break;
                }
            case EInnerState.GO_CENTER: {
                    Vector3 dir = centerObject.transform.position - transform.position;
                    dir.y = 0f;
                    mDesPos = transform.position + dir;
                    mDeltaDesPos = dir.normalized;
                    //mAni.CrossFade("run", 0.1f);
                    TransMove();
                    //Move();
                    if ((transform.position - mDesPos).sqrMagnitude < 0.001f) {
                        mInnerState = EInnerState.GO_AWAY;
                    }
                    break;
                }
            case EInnerState.GO_AWAY: {
                    Vector3 dir = goAwayObjects[mRandomGoAwayIndex].transform.position - transform.position;
                    dir.y = 0f;
                    mDesPos = transform.position + dir;
                    mDeltaDesPos = dir.normalized;
                    //mAni.CrossFade("run", 0.1f);
                    TransMove();
                    //Move();
                    if ((transform.position - mDesPos).sqrMagnitude < 0.001f) {
                        gameObject.SetActive(false); //隐掉 回收
                    }
                    break;
                }
        }
    }

    void Gravity() {
        mFallspeed -= Time.deltaTime / 10;
        mCc.Move(new Vector3(0, mFallspeed - g * Time.deltaTime * Time.deltaTime, 0));
    }

    /// <summary>
    /// 仅用作逛商店时的移动
    /// </summary>
    public void Move() {
        bool isNearDes = (transform.position - mDesPos).magnitude <= Time.deltaTime * mMoveSpeed;
        if (!isNearDes) {
            mCc.Move(mDeltaDesPos * Time.deltaTime * mMoveSpeed);
            Vector3 actualVelocity = mCc.velocity;
            actualVelocity.y = 0;
            if (actualVelocity != Vector3.zero) {

                Vector3 lookAtDir = Vector3.RotateTowards(transform.forward, actualVelocity, 5f * Time.deltaTime, 10000); //慢慢转身 防止抖动
                transform.LookAt(transform.position + lookAtDir);
            }
        }
        if ((isNearDes) || transform.position == mLastPos) {
            mAni.CrossFade("idle", 0.1f);
        } else {
            mAni.CrossFade("walk", 0.1f);
        }
        mLastPos = transform.position;
    }

    /// <summary>
    /// Liu Qiran
    /// 避免碰撞器的干扰产生不必要的麻烦
    /// </summary>
    public void TransMove() {
       gameObject.transform.Translate(mDeltaDesPos * Time.deltaTime * mMoveSpeed,Space.World);
       Vector3 lookAtDir = Vector3.RotateTowards(transform.forward, mDeltaDesPos, 5f * Time.deltaTime, 10000); //慢慢转身 防止抖动
       transform.LookAt(transform.position + lookAtDir);
       mAni.CrossFade("walk", 0.1f);
    }

    void ClickCat(GameObject go) {
        CustomerMediator customerMediator = AppFacade.getInstance.RetrieveMediator(CustomerMediator.NAME) as CustomerMediator;
        customerMediator.customerSpawnView.RemoveCustomer();
        //LeaveManor();
    }
    public void LeaveManor() {
        if (mInnerState == EInnerState.MOVE_AROUND) {
            mInnerState = EInnerState.MOVE_TO_OUT_POINT;        
        }
    }
}

