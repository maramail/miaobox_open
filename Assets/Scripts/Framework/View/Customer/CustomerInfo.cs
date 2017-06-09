using UnityEngine;
using System.Collections;

public class CustomerInfo : MonoBehaviour {

    public int Money { get; set; }

    void OnDisable()
    {
        Destroy(this);
    }

}
