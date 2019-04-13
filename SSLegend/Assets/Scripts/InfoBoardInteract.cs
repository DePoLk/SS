using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class InfoBoardInteract : MonoBehaviour {

    PlayerUI PUI;
    BillBoard BB;
    PlayerControl PlayerCon;
    [Header("告示牌開啟狀態")]
    public bool IsBoardOpen = false;
    [Header("抓取的UI物件")]
    public GameObject InfoBoard;

    [TextArea]
    public string InfoContent;

	// Use this for initialization
	void Start () {
        PUI = FindObjectOfType<PlayerUI>();
        BB = FindObjectOfType<BillBoard>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        InfoBoard = GameObject.Find("InfoBoard");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.CompareTag("EnemyDetector")) {
            BB.Appear(PlayerCon.gameObject, PlayerCon.TipBG[0]);
            PlayerCon.NearItem = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player") && !other.CompareTag("EnemyDetector"))
        {
            BB.Appear(PlayerCon.gameObject, PlayerCon.TipBG[0]);
            PlayerCon.NearItem = true;
        }
        

        if (other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button2))) {
            
            InfoBoard.transform.DOScaleX(1,0.5f).SetUpdate(true);
            InfoBoard.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = InfoContent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCon.NearItem = false;

        if (other.CompareTag("Player")) {
            InfoBoard.transform.DOScaleX(0, 0.5f).SetUpdate(true);          
            BB.disAppear(PlayerCon.gameObject);
        }
    }

}
