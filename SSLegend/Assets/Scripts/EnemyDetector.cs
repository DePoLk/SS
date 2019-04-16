using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System;

public class EnemyDetector : MonoBehaviour {

    GameObject EnemyInfo;
    Text EnemyTalkerName;
    Text EnemyTalkText;
    Image EnemyTalkerImage;
    GameObject Player;
    GameObject DetectedObj;
    SpriteRenderer SR;
    [Header("偵測到的敵人圖片")]
    public Sprite[] EnemyImgArray;

    Image TalkingEnemyImage;


    [Header("檢測有無")]
    public bool IsDetect = false;
    public bool IsCanPlay = true;
    [Header("Fix")]
    public float FixAngle = 0;
    [Header("台詞")]
    private string[] KarohthText = {
        "吱! 啾!",
        "吱! 啾啾!",
        "吱! 啾啾啾!"
    };
    private string[] WolfText = {
        "嘎! 嗚!",
        "嘎! 嗚嗚!",
        "嘎! 嗚嗚嗚!"
    };
    private string[] BossText = {
         "轟! 隆!",
        "轟! 隆隆!",
        "轟! 隆隆隆!"
    };

    private int TalkNum = 0;

	// Use this for initialization
	void Start () {
        EnemyInfo = GameObject.Find("EnemyInfo");
        EnemyTalkerName = EnemyInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        EnemyTalkText = EnemyInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        EnemyTalkerImage = EnemyInfo.transform.GetChild(2).GetComponent<Image>();
        Player = GameObject.Find("Player");
        SR = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TalkingEnemyImage = GameObject.Find("EnemyTalker").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        if (DetectedObj != null) {
            Vector3 TargetPos = new Vector3(DetectedObj.transform.position.x, this.gameObject.transform.position.y, DetectedObj.transform.position.z);
            this.gameObject.transform.LookAt(TargetPos);
            this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y + FixAngle, 0);
        }
       // Debug.Log(this.transform.eulerAngles.y);
       // DangerCenter.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0, this.transform.eulerAngles.y+FixAngle);
    }




    IEnumerator ChangeEnemyInfo(Collider other) {
        IsCanPlay = false;
        SR.DOFade(1,0.5f);
        EnemyInfo.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(2f);
        SR.DOFade(0, 0.5f);
        EnemyInfo.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);
        IsCanPlay = true;

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("WolfAtkBox")) {

        }

        if (other.CompareTag("Wolf") || other.CompareTag("Karohth") || other.CompareTag("ArrowWolf") || other.CompareTag("Boss")) {
            IsDetect = true;
            DetectedObj = other.gameObject;
            TalkNum = UnityEngine.Random.Range(0, 3);

            if (other.CompareTag("Wolf") || other.CompareTag("ArrowWolf")) {
                SR.color = new Color32(198,59,60,255);
                EnemyTalkerImage.sprite = EnemyImgArray[0];
            }
            if (other.CompareTag("Karohth")) {
                SR.color = new Color32(68,245,128,255);
                EnemyTalkerImage.sprite = EnemyImgArray[1];
            }
            if (other.CompareTag("Boss")) {
                SR.color = new Color32(198, 59, 60, 255);
                EnemyTalkerImage.sprite = EnemyImgArray[2];
            }

            if (IsCanPlay) {
                StartCoroutine("ChangeEnemyInfo",other);
            }

        }     

        if (other.CompareTag("Wolf") || other.CompareTag("ArrowWolf")) {
            EnemyTalkerName.text = "削狼";
            EnemyTalkText.text = WolfText[TalkNum];
        }

        if (other.CompareTag("Karohth")) {
            EnemyTalkerName.text = "棉鼠";
            EnemyTalkText.text = KarohthText[TalkNum];
        }
        if (other.CompareTag("Boss")) {
            EnemyTalkerName.text = "石巨人";
            EnemyTalkText.text = BossText[TalkNum];
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wolf") || other.CompareTag("Karohth"))
        {
            IsDetect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Wolf") || other.CompareTag("Karohth"))
        {
            IsDetect = false;
        }
    }

}
