using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class Exp_System : MonoBehaviour {

    PlayerControl PlayerCon;
    PlayerUI PUI;
    SavePoint SP;
    public bool IsExpMenuOpen = false;
    bool DefaultSelect = false;
    Button FirstExpMenuBtn;
    Button ExpMenuBtn;
    Image BearWithCube;

    bool IsUping = false;

    int UpHpCost = 30;
    int UpAtkCost = 30;
    int UpComboCost = 999;

    Tweener ExpSystemTweener;

    Text UpHpCostValueText;
    Text UpAtkCostValueText;
    Text UpComboCostValueText;
    Text ExpInfoText;

    GameObject MenuBG;

	// Use this for initialization
	void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();
        PUI = FindObjectOfType<PlayerUI>();
        FirstExpMenuBtn = GameObject.Find("UpMaxHpBtn").GetComponent<Button>();
        ExpMenuBtn = GameObject.Find("ExpMenuBtn").GetComponent<Button>();
        SP = FindObjectOfType<SavePoint>();
        MenuBG = GameObject.Find("MenuBG");

        UpHpCostValueText = GameObject.Find("UpMaxHpCostText").GetComponent<Text>();
        UpAtkCostValueText = GameObject.Find("UpMaxAtkCostText").GetComponent<Text>();
        UpComboCostValueText = GameObject.Find("UpMaxComboCostText").GetComponent<Text>();
        ExpInfoText = GameObject.Find("ExpInfoText").GetComponent<Text>();

        BearWithCube = GameObject.Find("BearWithCube").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckIsExpMenuOpen();
        UpHpCostValueText.text = UpHpCost.ToString();
        UpAtkCostValueText.text = UpAtkCost.ToString();
        UpComboCostValueText.text = UpComboCost.ToString();
	}

    void CheckIsExpMenuOpen() {
        if (IsExpMenuOpen)
        {
            this.GetComponent<CanvasGroup>().interactable = true;
            //this.GetComponent<CanvasGroup>().alpha = 1;
            ExpSystemTweener = this.gameObject.GetComponent<CanvasGroup>().DOFade(1,0.5f).SetUpdate(true);
            //iTween.ScaleTo(this.gameObject, new Vector3(1, 5, 1), 1f);
            ExpSystemTweener = this.gameObject.transform.DOLocalMove(new Vector3(70,-10,0),1f).SetUpdate(true);

            ExpSystemTweener = BearWithCube.transform.DOLocalMove(new Vector3(70,-6,0),0.5f).SetUpdate(true);

            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
                CloseExpMenu();
            }


            if (!DefaultSelect) {
                //ExitMenuBtn.Select();
                FirstExpMenuBtn.Select();
                DefaultSelect = true;
            }
        }// open
        else {
            DefaultSelect = false;
            this.GetComponent<CanvasGroup>().interactable = false;
            //this.GetComponent<CanvasGroup>().alpha = 0;
            ExpSystemTweener = this.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true);
            ExpSystemTweener = this.gameObject.transform.DOLocalMove(new Vector3(50, -10, 0), 1f).SetUpdate(true);
            ExpSystemTweener = BearWithCube.transform.DOLocalMove(new Vector3(40, -6, 0), 0.5f).SetUpdate(true);
            ExpInfoText.text = "";
            //iTween.ScaleTo(this.gameObject, new Vector3(0, 0, 0), 1f);
        }// close

        

    }

    IEnumerator ReInteractable() {    
        yield return new WaitForSecondsRealtime(0.5f);
        ExpMenuBtn.Select();
        MenuBG.GetComponent<CanvasGroup>().interactable = true;
    }

    public void CloseExpMenu() {
        IsExpMenuOpen = false;
        SP.IsSavePointOpened = true;
        PUI.ExpButtonState.disabledSprite = PUI.ButtonDisImg;
        MenuBG.transform.GetChild(2).GetComponent<Button>().spriteState = PUI.ExpButtonState;
        StartCoroutine("ReInteractable");
    }

    IEnumerator UpMaxHpExpMinus() {
        PlayerCon.MaxHp++;

        //PUI.CottonBGArray[PlayerCon.MaxHp].SetActive(true);
        Debug.Log("yolo");
        PUI.CottonBGArray[PlayerCon.MaxHp].GetComponent<Image>().DOFade(1f, 0).SetUpdate(true);
        //PUI.CottonHPArray[PlayerCon.MaxHp].SetActive(true);

        for (int i = 0; i < UpHpCost; i++) {
            PlayerCon.ExpPoint -= 1;
            yield return new WaitForSecondsRealtime(0.001f);
            //Debug.Log("ExpMinusForHp");
            IsUping = true;
        }
        //StopCoroutine("UpMaxHpExpMinus");
        IsUping = false;
    }

    IEnumerator UpMaxAtkExpMinus() {
        PlayerCon.PlayerAtkValue++;

        for (int i = 0; i < UpAtkCost; i++)
        {
            PlayerCon.ExpPoint -= 1;
            yield return new WaitForSecondsRealtime(0.001f);
            //Debug.Log("ExpMinusForAtk");
            IsUping = true;
        }
        //StopCoroutine("UpMaxAtkExpMinus");
        IsUping = false;
    }

   

    public void UpMaxHpBtnClick() {
        if (PlayerCon.ExpPoint >= UpHpCost && !IsUping)
        {
            if (PlayerCon.MaxHp < 10)
            {               
                StartCoroutine("UpMaxHpExpMinus");
                ExpInfoText.text = "增加最大生命";
            }
            else {
                Debug.Log("已達到最大等級");
                ExpInfoText.text = "已達到最大等級";
            }
        }
        else {
            Debug.Log("經驗不足");
            ExpInfoText.text = "經驗球不足";
        }
    }//end UpMaxHpBtnClick

    public void UpMaxAtkBtnClick()
    {
        if (PlayerCon.ExpPoint >= UpAtkCost && !IsUping)
        {
            if (PlayerCon.PlayerAtkValue < 3)
            {
                StartCoroutine("UpMaxAtkExpMinus");
                ExpInfoText.text = "增加攻擊力";
            }
            else
            {
                Debug.Log("已達到最大等級");
                ExpInfoText.text = "已達到最大等級";
            }
        }
        else
        {
            Debug.Log("經驗不足");
            ExpInfoText.text = "經驗球不足";
        }
    }//end UpMaxAtkBtnClick

    public void UpMaxComboBtnClick()
    {
        if (PlayerCon.ExpPoint >= UpComboCost && !IsUping)
        {
            
        }
        else
        {
            Debug.Log("經驗不足");
            ExpInfoText.text = "經驗球不足";
        }
    }//end UpMaxComboBtnClick

}
