using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataValue;
public class DSItem : MonoBehaviour {

    DataSystem DS;

    [Header("DS數據用")]
    public int StageEventNum = 0;
    public int ThisItemIndexInStage = 0;
    public bool CanPushValue = false;
	// Use this for initialization
	void Start () {
        DS = FindObjectOfType<DataSystem>();
        GetThisItemIndex();
        GetThisItemStageEventNum();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PushValueToDS() {

        DS.DataSystemValueInfoAll_Temp[DS.StageEventIndex[StageEventNum] + 2 + ThisItemIndexInStage] = ("I: " + true);

    }

    void GetThisItemIndex() {
        for (int i = 0; i < this.gameObject.transform.parent.childCount; i++)
        {
            if (this.gameObject.name.Equals(this.gameObject.transform.parent.GetChild(i).name))
            {
                ThisItemIndexInStage = i;
            }
        }
    }

    void GetThisItemStageEventNum() {

        for (int i = 0; i < DataValue.EventValue.StageEventMax; i++) {
            if (this.gameObject.transform.parent.name.Equals("StageEvent" + i)) {
                StageEventNum = i;
            }
        }

    }


}
