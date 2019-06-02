using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataValue;


public class UIEventTrigger : MonoBehaviour {

    PlayerUI PUI;
    PlayerControl Player_Con;
    StageEvent StE;
    DataSystem DS;
    [Header ("輸入UIData的最大數量(Max+1)")]
    public int MaxUITextDataCount = 0;//如在UIData.txt中有到--Max-- 則此欄要填入Max+1
    [Header("得知這塊事件Trigger的Num")]
    public int UIEventNum = 0;
    [Header("此UI在StageEvent中的序列")]
    public int ThisUIEventIndexInStage;
    [Header("此UI完成與否")]
    public bool IsDone = false;
    [Header("此UI需站立不動嗎?")]
    public bool IsNeedStopTime = false;

    int StageEventNum = 0;
    
    
	// Use this for initialization
	void Start () {      
        PUI = FindObjectOfType<PlayerUI>();
        Player_Con = FindObjectOfType<PlayerControl>();
        StE = FindObjectOfType<StageEvent>();
        DS = FindObjectOfType<DataSystem>();
        MaxUITextDataCount = DataValue.EventValue.UIEventMax;
        GetThisUIEventNum();
        GetThisUIEventIndexInStage();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            //Debug.Log(UIEventNum);
            PUI.GetOnTriggerUIEventNum = UIEventNum;
            Player_Con.IsUIEventing = true;
            PUI.NowUIEventPlayingNum = DataValue.UIInfoWindowValue.UITextSearchIndex;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Player_Con.ThisScene.name.Equals("2-1")) {
            //Debug.Log(UIEventNum);
            if (Player_Con.ThisScene.Equals("2-1"))
            {
                for (int StageEventIndex = 0; StageEventIndex < DataValue.EventValue.StageEventMax; StageEventIndex++)
                {
                    if (this.gameObject.transform.parent.name.Equals("StageEvent" + StageEventIndex))
                    {
                        StageEventNum = StageEventIndex;
                    }
                }

                DS.DataSystemValueInfoAll_Temp[DS.StageEventIndex[StageEventNum] + 2 + ThisUIEventIndexInStage] = ("U: ") + true;

                this.gameObject.transform.parent.GetComponent<StageEvent>().CheckIsSelfDone();

            }
            //Debug.Log(DS.StageEventIndex[StageEventNum]);
            Destroy(gameObject,0.1f);
        }

        if (other.CompareTag("Player") && Player_Con.ThisScene.name.Equals("K")) {
            PUI.BossHPUI.GetComponent<CanvasGroup>().alpha = 1;
            PUI.BossGetHp();
            PUI.GetInAniIsDone = true;
            Destroy(gameObject, 0);
        }

    }

    public void GetThisUIEventNum() {

        for (int i = 0; i <= MaxUITextDataCount; i++) {
            if (this.gameObject.name.Equals("UIEvent" + i)) {
                UIEventNum = i;
            }
        }
    }

    public void GetThisUIEventIndexInStage() {

        if (Player_Con.ThisScene.Equals("2-1")) {
            for (int i = 0; i < this.gameObject.transform.parent.childCount; i++)
            {
                if (this.gameObject.name.Equals(this.gameObject.transform.parent.GetChild(i).name))
                {
                    ThisUIEventIndexInStage = i;
                }
            }
        }
        
    }

    public void UIKillSelf() {
        Destroy(gameObject);
    }

}
