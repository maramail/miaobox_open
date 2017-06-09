using UnityEngine;
using System.Collections;

public class CtrlMiaopos : MonoBehaviour
{

    public void changeallposbyClockwise() {
        CreatPoint.Instance.changepos(1);
    }

    public void changeallposbyEastern()
    {
        CreatPoint.Instance.changepos(-1);
    }
}
