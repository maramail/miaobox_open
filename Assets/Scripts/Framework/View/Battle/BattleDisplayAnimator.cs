using UnityEngine;
using System.Collections;

public class BattleDisplayAnimator : MonoBehaviour {

    private float timer = 0f;
    private float interval = 2f;
    
    private Animation _Animation;
    private int flag = 0;
	// Use this for initialization
	void Start () {
        _Animation = this.GetComponent<Animation>();
       
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer>interval)
        {
            flag = Random.Range(0, 5);
            timer = 0f;

        }
        
        switch (flag )
        {
            case 0:
                _Animation.CrossFade("idle", 0.1f);
                break;
            case 1:
                _Animation.CrossFade("attack01", 0.1f);
                break;
            case 2:
                _Animation.CrossFade("attack02", 0.1f);
                break;
            case 3:
                _Animation.CrossFade("dance", 0.1f);
                break;
            case 4:
                _Animation.CrossFade("lucky", 0.1f);
                break;
            default: break;
        }
    }
}
