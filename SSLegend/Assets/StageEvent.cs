using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using DataValue;

public class StageEvent : MonoBehaviour {
    [Header("抓取此事件編號")]
    public int GetThisEventNum;

    [Header("各式物件事件範圍編號")]
    public int GetEventUIEventCountStart;
    public int GetEventUIEventCountEnd;
    public int GetEventMonsterCountStart;
    public int GetEventMonsterCountEnd;
    public int GetEventItemCountStart;
    public int GetEventItemCountEnd;

    [Header("Stage完成事件數")]
    public int DoneEventCount = 0;
    public int GetStageEventCount;

    DataSystem DS;



    // Use this for initialization
    void Start () {
        DS = FindObjectOfType<DataSystem>();
        GetStageEventCount = this.gameObject.transform.childCount;
        //StageCheckSelfIsDone();

        for (int i = 0; i < DataValue.EventValue.StageEventMax; i++) {
            if (this.gameObject.name.Equals("StageEvent" + i)) {
                GetThisEventNum = i;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void CheckIsSelfDone() {
        DoneEventCount++;
        if (DoneEventCount >= GetStageEventCount) {
            DS.DataSystemValueInfoAll_Temp[DS.StageEventIndex[GetThisEventNum] + 1] = true;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {   
        for (int i = 0; i < DataValue.EventValue.StageEventMax; i++) {

            if (other.CompareTag("Player") && this.gameObject.name.Equals("StageEvent" + i)) {
                //Debug.Log(i);
              
            }
               
         }
    }

    public void StageEventKillSelf() {
        Destroy(gameObject);
    }

}

namespace DataValue {
    class EventValue {
        public static int UIEventMax = 4;
        public static int StageEventMax = 2;
        public static int MonsterMax = 17;
        public static int ItemMax = 30;
    }//當總數有差異時要記得來修改此處數值
}
