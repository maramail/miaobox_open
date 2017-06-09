using UnityEngine;
using System.Collections;

public class EmployeeRandomAnimation : MonoBehaviour {
    public enum EmployeeState
    {
        Idle
    }

    public Animation employeeAnimation;
    public EmployeeState employeeState;

    void Start()
    {
        employeeAnimation = this.GetComponent<Animation>();
        employeeState = EmployeeState.Idle;
    }

    void OnDisable()
    {
        Destroy(this);
    }

    void Update()
    {
        switch (employeeState)
        {
            case EmployeeState.Idle:
                employeeAnimation.CrossFade("idle", 0.1f);
                break;
            default:break;
        }
    }
}
