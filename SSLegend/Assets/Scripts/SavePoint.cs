using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class SavePoint : MonoBehaviour {

    PlayerControl PlayerCon;
    PlayerUI PUI;
    Exp_System ExpS;
    public bool IsSavePointOpened = false;
    BillBoard BB;
    public Sprite SaveTip;
    public Image SaveMenu;
    public Button SaveGameBtn;
    bool DefaultSelect = false;
    Hashtable MenuSetting;

    float PressTime = 0;

    Tweener MenuTweener;

    //SaveMenu UI

    float DefaultSaveMenuPosX = 0;
    float CloseSaveMenuPosX = -1920;

    //SaveMenu UI

    // Use this for initialization
    void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();
        BB = FindObjectOfType<BillBoard>();
        PUI = FindObjectOfType<PlayerUI>();
        ExpS = FindObjectOfType<Exp_System>();
        MenuTweener.SetAutoKill(true);
        SaveMenu = GameObject.Find("MenuBG").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        OpenMenu();
        CheckPressTime();
    }

    /// <summary>
    /// 在這處理Menu的開啟及關閉事件
    /// </summary>

    void CheckPressTime() {
        if (PressTime >= 0.5f) {
            IsSavePointOpened = true;
            PressTime = 0;
        }
        //Debug.Log(PressTime);
    }

    void OpenMenu()
    {
        if (IsSavePointOpened)
        {

            if (!DefaultSelect) {
                SaveGameBtn.Select();
                DefaultSelect = true;
            }
            //SaveMenu.GetComponent<CanvasGroup>().alpha = 1f;
            //iTween.ScaleTo(SaveMenu.gameObject,new Vector3( 10, 10, 10), 1f);
            SaveMenu.transform.GetChild(1).GetComponent<Button>().interactable = true;
            SaveMenu.transform.GetChild(2).GetComponent<Button>().interactable = true;
            SaveMenu.transform.GetChild(3).GetComponent<Button>().interactable = true;

            //MenuTweener = SaveMenu.gameObject.transform.DOLocalMove(new Vector3(490,0,0),1);//open
            MenuTweener = SaveMenu.gameObject.transform.DOLocalMove(new Vector3(0,0,0),0.5f).SetUpdate(true);
            MenuTweener = SaveMenu.gameObject.GetComponent<CanvasGroup>().DOFade(1f,1f).SetUpdate(true);

            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Joystick1Button1) && !ExpS.IsExpMenuOpen)
            {

                if (SaveMenu.GetComponent<CanvasGroup>().interactable) {
                    ExitMenu();
                }
            }
            
        }
        else if(!IsSavePointOpened){
            //SaveMenu.GetComponent<CanvasGroup>().alpha = 0f;
            SaveMenu.transform.GetChild(1).GetComponent<Button>().interactable = false;
            SaveMenu.transform.GetChild(2).GetComponent<Button>().interactable = false;
            SaveMenu.transform.GetChild(3).GetComponent<Button>().interactable = false;

            DefaultSelect = false;
            //MenuTweener = SaveMenu.gameObject.transform.DOLocalMove(new Vector3(1500, 0, 0), 1);//close
            MenuTweener = SaveMenu.gameObject.transform.DOLocalMove(new Vector3(-900, 0, 0), 0.5f).SetUpdate(true);
            MenuTweener = SaveMenu.gameObject.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetUpdate(true);

            //iTween.ScaleTo(SaveMenu.gameObject, new Vector3(0, 0, 0), 1f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
           // Debug.Log("NearSavePoint");
            PlayerCon.NearItem = true;
            BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[0]);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerCon.NearItem)
        {

            if (Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.J))
            {
                // Debug.Log("OpenSavePoint");
                PressTime += Time.deltaTime;
            }
            else {
                PressTime = 0;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Debug.Log("LeaveSavePoint");
            PlayerCon.NearItem = false;
            IsSavePointOpened = false;
            BB.disAppear(PlayerCon.gameObject);
        }
    }

    public void ExitMenu() {
        IsSavePointOpened = false;
        PUI.ExpButtonState.disabledSprite = null;
        PUI.ExpButtonState.highlightedSprite = PUI.ButtonDisImg;
        PUI.MenuBG.transform.GetChild(2).GetComponent<Button>().spriteState = PUI.ExpButtonState;
    }
   

}
