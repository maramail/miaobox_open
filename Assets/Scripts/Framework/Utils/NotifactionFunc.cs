using UnityEngine;
using System.Collections;

public class NotifactionFunc : MonoBehaviour {

    private UIPanel uipanel;
    private UILabel uilable;
    public float  fatetimer=0f;
    private   float timer = 0f;
    bool isok = false ;
    public static NotifactionFunc _instance;

    void Awake()
    {

        _instance = this;
    }
    void Start()
    {
        uipanel = this .GetComponent<UIPanel>();
        uilable = this.transform.GetChild(0).GetComponent<UILabel>();
        this.gameObject.SetActive(false);
        timer = fatetimer;
         

    }
    void FixedUpdate()
    {

        if (isok)
        {
            fateIn();

        }
           
       
    }
    void fateIn()                   //处理逐渐消退
    {
        timer -= Time.deltaTime;
        float curretnalpha = timer / fatetimer;
         
        if (curretnalpha <= 0.1f)
        {
            timer = fatetimer;
            isok = false;
            this.gameObject.SetActive(false);
        }
        uipanel.alpha = curretnalpha;

    }
    public void showNotifaction(string content)         //处理消息框
    {
        uilable.text = content;
        this.gameObject.SetActive(true);
        uipanel.alpha = 1;
        timer = fatetimer;
        isok = true;

    }

}
