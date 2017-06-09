using UnityEngine;
using System.Collections;

public class GUIManage : MonoBehaviour {

    public  static GUIManage _instance;
    public UILabel cooldownText;
    public UILabel notify;
    void Awake()
    {

        _instance = this;
    }

    void Start()
    {
        cooldownText.gameObject.SetActive(false);
        notify.gameObject.SetActive(false);
    }

    public void notifi(string text,float inteval)
    {
        notify.gameObject.SetActive(true);
        notify.text = text;
        notify.alpha = 1;
        StartCoroutine(MiaoBoxTool.FadeText(notify, inteval, new Color(0, 0, 0,0)));
    }
        
    public IEnumerator  showText(float inteval)
    {
        cooldownText.gameObject.SetActive(true);
        cooldownText.text = inteval.ToString();
        StartCoroutine(MiaoBoxTool.FadeText(cooldownText, 1f, new Color(0, 0, 0, 0)));
        
        while ( inteval>1)
        {
            yield return new WaitForSeconds(1f);
            cooldownText.alpha = 1;
            inteval--;
            cooldownText.text = inteval.ToString();
            StartCoroutine(MiaoBoxTool.FadeText(cooldownText, 1f, new Color(0, 0, 0, 0)));
            yield return null;
        }
      

    }
    public void  cooldownhideorshow(bool state)
    {
        cooldownText.gameObject.SetActive(state);
    }
    public void notifyhideorshow(bool state)
    {
        notify.gameObject.SetActive(state);
    }

}
