using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using DataValue;

public class PlayerUI : MonoBehaviour {

    public Text WaterValueText;
    public Text WindValueText;
    public Text Water2ValueText;
    public Text Wind2ValueText;
    public Text Hp;
    public GameObject InfoWindow;
    public Text InfoText;
    public Image WaterElementImg;
    public Image WindElementImg;
    public Image[] ElementImg; // 0:wind 1:water 2:wind2 3:water2
    public Sprite ButtonDisImg;

    //注意這上面的都要記得拉給他

    Text MaxHpText;
    Text MaxAtkText;


    Text ExpPointText;
    private Tweener UITweener;
    Image EscMenuBGLeft;
    Text SavingText;
    BGMManager BGMM;
    public GameObject MenuBG;
    Text ExpStateText;

    //注意這上面要在Start()中賦予
    GameObject Player;
    SavePoint SavePoint;
    UITextData UITextData;
    DataManger DM;




    UIEventTrigger UIEventTrigger;
    public int GetOnTriggerUIEventNum;
    public ArrayList UIDataArray;
    Animator Canvas_Ani;
    public GameObject CottonHP;
    public GameObject CottonBG;
    public GameObject[] CottonHPArray;
    public GameObject[] CottonBGArray;
    GameObject NowInsObject;
    GameObject NowInsBGObject;
    PlayerControl PlayerCon;
    public bool ChangingItemUI = false;
    Exp_System ExpS;
    DataSystem DS;



    // UI ItemIcon
    Vector3[] ItemIconLocation = new[] { new Vector3(-60, -35, 0), new Vector3(0, 15, 0), new Vector3(60, -35, 0), new Vector3(0, -180, 0) };
    int WaterNowLocationNum = 1;
    int WindNowLocationNum = 0;
    int Water2NowLocationNum = 3;
    int Wind2NowLocationNum = 2;
    float ItemIconChangeTime = 0.5f;

    public Sprite[] OutlineArray;// 0:W 1:B
    public Image WaterOutline;
    public Image WindOutline;
    public Image Water2Outline;
    public Image Wind2Outline;

    Vector3 ItemIconScale = new Vector3(1f, 1f, 1f);
    Vector3 DefaultItemIconScale = new Vector3(0.8f, 0.8f, 0.8f);
    float ItemIconScaleTime = 0.5f;

    float DefaultItemIconAlpha = 0.5f;
    float ItemIconFadeTime = 0.25f;

    // UI ItemIcon

    Hashtable MoveSetting;

    public int MaxHP;
    public int NowHP;

    GameObject LastSelect;


    // Esc Menu

    GameObject EscMenuBG;
    Button Resume;
    Button System;
    Button MainMenu;

    float EscMenuMoveY = 10;
    float EscMenuMoveYTime = 0.5f;

    int ResumeButtonNum = 2;
    int SystemButtonNum = 3;
    int MainMenuButtonNum = 4;
    float[] EscButtonYArray = { 40, 20, 0, -20, -40 };
    float[] EscButtonXArray = { 55, 62.5f, 65, 62.5f, 55 };
    float[] EscButtonRotationZArray = { 30, 15, 0, -15, -30 };

    public bool SystemIsOpen = false;

    Slider SoundSlider;
    Slider ScreenModeSlider;
    Slider ResolutionSlider;
    Button SystemBackButton;

    Text ResolutionValueText;
    // Esc Menu 


    [Header("UI對話框")]
    public Text InfoTalker;
    public Image InfoTalkerImage;
    string[] TalkerNameArray = { "貝爾", "斯歐芙忒", "薩奧莉德" };
    Image InfoWindowArrow;

    [Header("對話框用圖片")]
    public Sprite[] TalkerImage;


    // Use this for initialization
    void Start() {
        Player = GameObject.Find("Player");
        SavePoint = FindObjectOfType<SavePoint>();
        UITextData = FindObjectOfType<UITextData>();
        UIEventTrigger = FindObjectOfType<UIEventTrigger>();
        Canvas_Ani = GetComponent<Animator>();
        UITextData.LoadFileFunction();
        UIDataArray = UITextData.InfoAll;
        DM = FindObjectOfType<DataManger>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        ExpS = FindObjectOfType<Exp_System>();
        DS = FindObjectOfType<DataSystem>();

        MaxHpText = GameObject.Find("MaxHpText").GetComponent<Text>();
        MaxAtkText = GameObject.Find("MaxAtkText").GetComponent<Text>();
        ExpPointText = GameObject.Find("ExpValue").GetComponent<Text>();
        SavingText = GameObject.Find("SavingText").GetComponent<Text>();
        MenuBG = GameObject.Find("MenuBG");

        ExpStateText = GameObject.Find("ExpStateText").GetComponent<Text>();

        //Esc Menu
        EscMenuBG = GameObject.Find("EscMenuBG");
        Resume = EscMenuBG.transform.GetChild(0).GetComponent<Button>();
        System = EscMenuBG.transform.GetChild(1).GetComponent<Button>();
        MainMenu = EscMenuBG.transform.GetChild(2).GetComponent<Button>();
        EscMenuBGLeft = GameObject.Find("EscMenuBGLeft").GetComponent<Image>();
        SoundSlider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        BGMM = FindObjectOfType<BGMManager>();
        ScreenModeSlider = GameObject.Find("ScreenModeSlider").GetComponent<Slider>();
        ResolutionSlider = GameObject.Find("ResolutionSlider").GetComponent<Slider>();
        SystemBackButton = GameObject.Find("SystemBackButton").GetComponent<Button>();
        ResolutionValueText = GameObject.Find("ResolutionValueText").GetComponent<Text>();
        //Esc Menu

        //--- UI對話框

        InfoTalker = GameObject.Find("InfoTalker").GetComponent<Text>();
        InfoTalkerImage = GameObject.Find("InfoTalkerImg").GetComponent<Image>();
        InfoWindowArrow = GameObject.Find("InfoWindowArrow").GetComponent<Image>();
        //--- UI對話框


        NowHP = PlayerCon.Hp;
        //Debug.Log("HP:" + PlayerCon.Hp);
        MaxHP = PlayerCon.MaxHp;

        IniCottonHP();
        GetPerIndex();
        // Debug.Log(UITextData.InfoAll.Count);



        NowLocationNumChecker();

        LastSelect = new GameObject();

        UITweener = ElementImg[1].transform.DOScale(ItemIconScale, 0.5f);

        /*Debug.Log(UIEventNum[0]);
         Debug.Log(UIEventNum[1]);*/
        /*Debug.Log(UIEventTextAmount[0]);
        Debug.Log(UIEventTextAmount[1]);*/
    }

    // Update is called once per frame
    void Update() {
        CheckValuePerSec();//更新UI資訊       
        InfoWindowHandler(GetOnTriggerUIEventNum);//處理UIEvent
        ReFocus();// 重新聚焦最後一個聚焦的對象
        EscMenuCon();// Esc的選單控制
    }


    void CheckValuePerSec() {
        WaterValueText.text = PlayerCon.WaterElement.ToString();
        Water2ValueText.text = PlayerCon.WaterElement.ToString();
        WindValueText.text = PlayerCon.WindElement.ToString();
        Wind2ValueText.text = PlayerCon.WindElement.ToString();
        Hp.text = PlayerCon.Hp.ToString();
        ExpPointText.text = PlayerCon.ExpPoint.ToString();
        MaxHpText.text = PlayerCon.MaxHp.ToString();
        MaxAtkText.text = PlayerCon.PlayerAtkValue.ToString();
        ExpStateText.text = PlayerCon.ExpPoint.ToString();

    }




    /// <summary>
    /// 抓取UI的順序號碼
    /// </summary>

    List<int> UIEventNumIndex = new List<int>();
    List<int> UIEventTextAmount = new List<int>();
    


    


    int NowUIEventNum = 0;
    int NowUIEventTextAmount(int NowIndex,int NUIEN, int LastValue) {

        string NextStr = UITextData.InfoAll[NowIndex + LastValue].ToString();
        //Debug.Log("NextSte: " + NextStr);

        if (NextStr.Equals("--" + (NUIEN + 1) + "--"))
        {
            //Debug.Log("NUIEN: " + NUIEN);
            //Debug.Log(LastValue - 1);
            return LastValue - 1;
        }
        if (NextStr.Equals("--End--")) {

           // Debug.Log("NUIEN: " + NUIEN);
           // Debug.Log(LastValue - 1);
            return LastValue - 1;
        }
        else
        {
            return NowUIEventTextAmount(NowIndex, NUIEN, LastValue + 1);
        }
        

    }
    public int NowUIEventPlayingNum = 3;

    public void GetPerIndex() {
       
        for (int i = 0; i < UITextData.InfoAll.Count; i++) {
            if (UITextData.InfoAll[i].Equals("--" + NowUIEventNum + "--")) {

                UIEventTextAmount.Add(NowUIEventTextAmount(i, NowUIEventNum, 1));
                //Debug.Log("AddTextAmount");
                UIEventNumIndex.Add(i);
                NowUIEventNum++;
                //Debug.Log(i);
            }
        }// 抓取UI事件編號的索引值               
        NowUIEventNum = 0;

    }

 
    public void HandleInfoTalker(int GetUIInfoNum) {

        //--- TalkerName            
        
            InfoTalker.text = TalkerNameArray[int.Parse(UIDataArray[GetUIInfoNum - 2].ToString())];
           

        //--- TalkerName

        //--- TalkerImage

    
        InfoTalkerImage.sprite = TalkerImage[int.Parse(UIDataArray[GetUIInfoNum - 1].ToString())];

        //--- TalkerImage

    }


    /// <summary>
    /// 用來檢測InfoWindow現在有沒有觸發事件
    /// </summary>
    /// <param name="UIEvent">用以獲取當前事件的序號</param>

    public bool IsCanNextInfoText = true;
    bool InfoWindowArrowCanFade = true;

    IEnumerator InfoWindowArrowFade() {
        if (FindObjectOfType<PlayerControl>().IsUIEventing && InfoWindowArrowCanFade)
        {
            InfoWindowArrowCanFade = false;
            InfoWindowArrow.DOFade(0, 0.5f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.5f);
            InfoWindowArrow.DOFade(1, 0.5f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.5f);
            InfoWindowArrowCanFade = true;

        }
    }

    public void InfoWindowHandler(int UIEvent) {
        if (FindObjectOfType<PlayerControl>().IsUIEventing)
        {
            //Canvas_Ani.SetBool("InfoWindowFadeOut",false);
            // Canvas_Ani.SetBool("InfoWindowFadeIn",true);
            Time.timeScale = 0;
            InfoWindow.GetComponent<Image>().DOFade(1f, 0.5f).SetUpdate(true);
            InfoWindow.GetComponent<Image>().transform.DOScaleX(1, 0.5f).SetUpdate(true);


            if (NowUIEventPlayingNum == DataValue.UIInfoWindowValue.UITextSearchIndex)
            {
                InfoText.text = UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum].ToString();
                InfoTalker.text = TalkerNameArray[int.Parse(UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 2].ToString())];
                InfoTalkerImage.sprite = TalkerImage[int.Parse(UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 1].ToString())];

                InfoText.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true);
                InfoTalker.DOFade(1, 0.5f).SetUpdate(true);
                InfoTalkerImage.DOFade(1, 0.5f).SetUpdate(true);

                NowUIEventPlayingNum += DataValue.UIInfoWindowValue.UITextSearchIndex;
            }

            if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J)) && IsCanNextInfoText)
            {

                IsCanNextInfoText = false;

                if (NowUIEventPlayingNum > UIEventTextAmount[UIEvent])
                {                                        
                    InfoWindow.GetComponent<Image>().DOFade(0f, 0.5f).SetUpdate(true);
                    InfoWindow.GetComponent<Image>().transform.DOScaleX(0, 0.5f).SetUpdate(true);
                    InfoText.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true);
                    InfoTalker.DOFade(0, 0.5f).SetUpdate(true);
                    InfoTalkerImage.DOFade(0, 0.5f).SetUpdate(true);

                    NowUIEventPlayingNum = DataValue.UIInfoWindowValue.UITextSearchIndex;
                    IsCanNextInfoText = true;

                    FindObjectOfType<PlayerControl>().IsUIEventing = false;
                    Time.timeScale = 1f;
                }//關閉infowindow
                else
                {
                    StartCoroutine("ChangeInfoText");                   
                }


            }
        }

        StartCoroutine("InfoWindowArrowFade");

    }

    bool IsSameTalker = false;
    bool IsSameTalkerImage = false;

    IEnumerator ChangeInfoText() {
        int NowEventNumber = NowUIEventPlayingNum;
        int UIEvent = GetOnTriggerUIEventNum; // UIEvent需要先存在在場上以免報錯
        //InfoText.DOColor(new Color(255, 255, 255, 0), 0.5f).SetUpdate(true);
        
        InfoText.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true);

        if (UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 2].Equals(UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 5]))
        {
            IsSameTalker = true;
        }
        else {
            IsSameTalker = false;
        }

        if (UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 1].Equals(UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum - 4]))
        {
            IsSameTalkerImage = true;
        }
        else
        {
            IsSameTalkerImage = false;
        }

        if (!IsSameTalker) {
            InfoTalker.DOFade(0, 0.5f).SetUpdate(true);
        }
        if (!IsSameTalkerImage) {
            InfoTalkerImage.DOFade(0, 0.5f).SetUpdate(true);
        }

        yield return new WaitForSecondsRealtime(1f);

        
        InfoText.text = UIDataArray[UIEventNumIndex[UIEvent] + NowUIEventPlayingNum].ToString();
        HandleInfoTalker(UIEventNumIndex[UIEvent]+NowUIEventPlayingNum);//更換說話者圖示與名稱

        InfoText.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true);
        InfoTalker.DOFade(1, 0.5f).SetUpdate(true);
        InfoTalkerImage.DOFade(1, 0.5f).SetUpdate(true);

        NowUIEventPlayingNum += DataValue.UIInfoWindowValue.UITextSearchIndex;
        IsCanNextInfoText = true;
        
        StopCoroutine("ChangeInfoText");
    }

    

    public void ResetBoolInfoTextFade()
    {
        // Canvas_Ani.SetBool("InfoTextFade", false);
        InfoText.DOColor(new Color(255,255,255,1),0.5f);
    }


    void IniCottonHP() {
        
        CottonHPArray = new GameObject[11];
        CottonBGArray = new GameObject[11];
        for (int i = 1; i <= 10; i++)
        {
            NowInsBGObject = Instantiate(CottonBG, CottonBG.transform.parent.transform) as GameObject;
            NowInsObject = Instantiate(CottonHP,CottonHP.transform.parent.transform) as GameObject;
            
            CottonHPArray[i] = NowInsObject;
            CottonHPArray[i].transform.localPosition += new Vector3(i*30,0,0);
            CottonBGArray[i] = NowInsBGObject;
            CottonBGArray[i].transform.localPosition += new Vector3(i*30,0,0);
            if (i <= NowHP)
            {
               //CottonHPArray[i].SetActive(true);
                CottonHPArray[i].GetComponent<Image>().DOFade(1, 0f);
                //CottonBGArray[i].SetActive(true);
                CottonBGArray[i].GetComponent<Image>().DOFade(1f, 0f);

            }
            else {
                //CottonHPArray[i].SetActive(false);
                CottonHPArray[i].GetComponent<Image>().DOFade(0, 0f);
                if (i <= MaxHP)
                {
                    //CottonBGArray[i].SetActive(true);
                    CottonBGArray[i].GetComponent<Image>().DOFade(1f, 0f);
                }
                else {
                   // CottonBGArray[i].SetActive(false);
                    CottonBGArray[i].GetComponent<Image>().DOFade(0, 0f);
                }
            }
        }

        CottonHP.SetActive(false);
        CottonBG.SetActive(false);// 關閉預置物件
    
    }

    public bool IsCanSave = true;

    IEnumerator SaveCoolTime() {

        IsCanSave = false;
        yield return new WaitForSecondsRealtime(2f);
        IsCanSave = true;
        StopCoroutine("SaveCoolTime");
    }   

    public void SaveGame()
    {
        if (IsCanSave)
        {
            StartCoroutine("SaveCoolTime");
            DM.DeleteFile(Application.dataPath + "/Save", "Save.txt");
            DM.SaveFile(Application.dataPath + "/Save", "Save.txt");
            DS.DeleteFile(Application.dataPath + "/Save", "EventData.txt");
            DS.SaveFile(Application.dataPath + "/Save", "EventData.txt");
            //StartCoroutine("SavingAnimation");
            //SavingText.text = "Saving.";        
            UITweener = SavingText.DOText("Saving.", 0).SetUpdate(true);
            UITweener = SavingText.DOFade(1, 0.5f).SetDelay(0.5f).SetUpdate(true);
            UITweener = SavingText.DOFade(0, 0.5f).SetDelay(1f).SetUpdate(true);

            //SavingText.text = "Saving..";
            UITweener = SavingText.DOText("Saving..", 0.1f).SetDelay(0.9f).SetUpdate(true);
            UITweener = SavingText.DOFade(1, 0.5f).SetDelay(1.5f).SetUpdate(true);
            UITweener = SavingText.DOFade(0, 0.5f).SetDelay(2f).SetUpdate(true);

            //SavingText.text = "Saving...";
            UITweener = SavingText.DOText("Saving...", 0.1f).SetDelay(1.9f).SetUpdate(true);
            UITweener = SavingText.DOFade(1, 0.5f).SetDelay(2.5f).SetUpdate(true);
            UITweener = SavingText.DOFade(0, 0.5f).SetDelay(3f).SetUpdate(true);

            StartCoroutine("SaveCoolTime");
            Debug.Log("GameSaved");
        }
        else {
            Debug.Log("Can't Save Now");
        }
    }

   

    void NowLocationNumChecker() {
        if (WaterNowLocationNum < 0)
        {
            WaterNowLocationNum = 3;
        }
        if (WaterNowLocationNum > 3)
        {
            WaterNowLocationNum = 0;
        }
        if (Water2NowLocationNum < 0)
        {
            Water2NowLocationNum = 3;
        }
        if (Water2NowLocationNum > 3)
        {
            Water2NowLocationNum = 0;
        }
        if (WindNowLocationNum < 0)
        {
            WindNowLocationNum = 3;
        }
        if (WindNowLocationNum > 3)
        {
            WindNowLocationNum = 0;
        }
        if (Wind2NowLocationNum < 0)
        {
            Wind2NowLocationNum = 3;
        }
        if (Wind2NowLocationNum > 3)
        {
            Wind2NowLocationNum = 0;
        }

        //--- IconScale start
        if (WindNowLocationNum == 1)
        {
            UITweener = ElementImg[0].transform.DOScale(ItemIconScale, ItemIconScaleTime);
            WindOutline.sprite = OutlineArray[0];
            ElementImg[0].GetComponent<Image>().DOFade(0.7f,0.5f);
            ElementImg[0].transform.GetChild(0).GetComponent<Image>().DOFade(0.9f,0.5f);//icon
            ElementImg[0].transform.GetChild(2).GetComponent<Image>().DOFade(0.7f, 0.5f);//outline
        }
        else
        {
            UITweener = ElementImg[0].transform.DOScale(DefaultItemIconScale, ItemIconScaleTime);
            WindOutline.sprite = OutlineArray[1];
            ElementImg[0].transform.GetChild(0).GetComponent<Image>().DOFade(0.5f,0.5f);
            ElementImg[0].transform.GetChild(2).GetComponent<Image>().DOFade(0.5f, 0.5f);
        }// wind 0
        if (WaterNowLocationNum == 1)
        {
            UITweener = ElementImg[1].transform.DOScale(ItemIconScale, ItemIconScaleTime);
            WaterOutline.sprite = OutlineArray[0];
            ElementImg[1].GetComponent<Image>().DOFade(0.7f, 0.5f);
            ElementImg[1].transform.GetChild(0).GetComponent<Image>().DOFade(0.9f,0.5f);
            ElementImg[1].transform.GetChild(2).GetComponent<Image>().DOFade(0.7f, 0.5f);
        }
        else {
            UITweener = ElementImg[1].transform.DOScale(DefaultItemIconScale, ItemIconScaleTime);
            WaterOutline.sprite = OutlineArray[1];
            ElementImg[1].transform.GetChild(0).GetComponent<Image>().DOFade(0.5f,0.5f);
            ElementImg[1].transform.GetChild(2).GetComponent<Image>().DOFade(0.5f, 0.5f);
        }// water 1

        if (Wind2NowLocationNum == 1)
        {
            UITweener = ElementImg[2].transform.DOScale(ItemIconScale, ItemIconScaleTime);
            Wind2Outline.sprite = OutlineArray[0];
            ElementImg[2].GetComponent<Image>().DOFade(0.7f, 0.5f);
            ElementImg[2].transform.GetChild(0).GetComponent<Image>().DOFade(0.9f,0.5f);
            ElementImg[2].transform.GetChild(2).GetComponent<Image>().DOFade(0.7f, 0.5f);
        }
        else
        {
            UITweener = ElementImg[2].transform.DOScale(DefaultItemIconScale, ItemIconScaleTime);
            Wind2Outline.sprite = OutlineArray[1];
            ElementImg[2].transform.GetChild(0).GetComponent<Image>().DOFade(0.5f,0.5f);
            ElementImg[2].transform.GetChild(2).GetComponent<Image>().DOFade(0.5f, 0.5f);
        }// wind2 2
        if (Water2NowLocationNum == 1)
        {
            UITweener = ElementImg[3].transform.DOScale(ItemIconScale, ItemIconScaleTime);
            Water2Outline.sprite = OutlineArray[0];
            ElementImg[3].GetComponent<Image>().DOFade(0.7f, 0.5f);
            ElementImg[3].transform.GetChild(0).GetComponent<Image>().DOFade(0.9f,0.5f);
            ElementImg[3].transform.GetChild(2).GetComponent<Image>().DOFade(0.7f, 0.5f);
        }
        else
        {
            UITweener = ElementImg[3].transform.DOScale(DefaultItemIconScale, ItemIconScaleTime);
            Water2Outline.sprite = OutlineArray[1];
            ElementImg[3].transform.GetChild(0).GetComponent<Image>().DOFade(0.5f,0.5f);
            ElementImg[3].transform.GetChild(2).GetComponent<Image>().DOFade(0.5f, 0.5f);
        }// water2 3
        //--- IconScale end

        //--- alpha start
        if (WindNowLocationNum == 3)
        {
            UITweener = ElementImg[0].DOFade(0, ItemIconFadeTime);
        }
        else if(WindNowLocationNum == 0 || WindNowLocationNum == 2)
        {
            UITweener = ElementImg[0].DOFade(DefaultItemIconAlpha, ItemIconFadeTime);
        }// wind 0
        if (WaterNowLocationNum == 3)
        {
            UITweener = ElementImg[1].DOFade(0, ItemIconFadeTime);
        }
        else if(WaterNowLocationNum == 0 || WaterNowLocationNum == 2)
        {
            UITweener = ElementImg[1].DOFade(DefaultItemIconAlpha, ItemIconFadeTime);
        }// water 1

        if (Wind2NowLocationNum == 3)
        {
            UITweener = ElementImg[2].DOFade(0, ItemIconFadeTime);
        }
        else if(Wind2NowLocationNum == 0 || Wind2NowLocationNum == 2)
        {
            UITweener = ElementImg[2].DOFade(DefaultItemIconAlpha, ItemIconFadeTime);
        }// wind2 2
        if (Water2NowLocationNum == 3)
        {
            UITweener = ElementImg[3].DOFade(0, ItemIconFadeTime);
        }
        else if(Water2NowLocationNum == 0 || Water2NowLocationNum == 2)
        {
            UITweener = ElementImg[3].DOFade(DefaultItemIconAlpha, ItemIconFadeTime);
        }// water2 3
        //--- alpha end
    }

    IEnumerator WaitSecToResetChangingItemUI()
    {
        yield return new WaitForSeconds(0.5f);
        ChangingItemUI = false;
    }

    public void ChangeItem(int Item_Type,int GetButtonSide) {

        ChangingItemUI = true;

        StartCoroutine("WaitSecToResetChangingItemUI");

        if (GetButtonSide == 1) {
            WindNowLocationNum--;
            Wind2NowLocationNum--;
            WaterNowLocationNum--;
            Water2NowLocationNum--;
        }// Left
        else if (GetButtonSide == 2) {
            WindNowLocationNum++;
            Wind2NowLocationNum++;
            WaterNowLocationNum++;
            Water2NowLocationNum++;
        }// Right
        NowLocationNumChecker();       

        UITweener = ElementImg[0].transform.DOLocalMove(ItemIconLocation[WindNowLocationNum], ItemIconChangeTime);// WIND
        UITweener = ElementImg[1].transform.DOLocalMove(ItemIconLocation[WaterNowLocationNum], ItemIconChangeTime);//WATER
        UITweener = ElementImg[2].transform.DOLocalMove(ItemIconLocation[Wind2NowLocationNum], ItemIconChangeTime);//WIND2
        UITweener = ElementImg[3].transform.DOLocalMove(ItemIconLocation[Water2NowLocationNum], ItemIconChangeTime);//WATER2

    }// end ChangeItem

    void ResetItemUI(Image GetImg ,float x) {
      
    }// end ResetItemUI

    public SpriteState ExpButtonState = new SpriteState();
    public void OpenExpSystem() {
        ExpButtonState.disabledSprite = ButtonDisImg;
        ExpButtonState.highlightedSprite = ButtonDisImg;
        MenuBG.GetComponent<CanvasGroup>().interactable = false;
        MenuBG.transform.GetChild(2).GetComponent<Button>().spriteState = ExpButtonState;
        ExpS.IsExpMenuOpen = true;
    }


    public void ReFocus() {
       

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(LastSelect);
        }
        else {
            LastSelect = EventSystem.current.currentSelectedGameObject;
        }

    }// ReFocus 重新聚焦上一個聚焦目標


    //--- EscMenu
    bool DefalutSelectResume = false;
    float GetV_AxisTime = 0;

    public void EscMenuCon() {
        if (PlayerCon.IsEscMenu)
        {
            if (!DefalutSelectResume) {
                Resume.Select();
                DefalutSelectResume = true;
            }

            
            

            EscMenuButtonSelectChecker(ResumeButtonNum, SystemButtonNum, MainMenuButtonNum);

            UITweener = EscMenuBG.transform.DOLocalMove(new Vector3(0, 0, 0), 1f).SetUpdate(true);
            Resume.interactable = true;
            System.interactable = true;
            MainMenu.interactable = true;

            if (Input.GetAxis("Vertical") == -1 || Input.GetAxis("Vertical") == 1) {
                GetV_AxisTime += Time.unscaledDeltaTime;
            }

            if ((Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") == -1 && GetV_AxisTime >= 0.15f)) && !SystemIsOpen)
            {
                ResumeButtonNum--;
                SystemButtonNum--;
                MainMenuButtonNum--;

                if (ResumeButtonNum < 0) {
                    ResumeButtonNum = 0;
                    SystemButtonNum = 1;
                    MainMenuButtonNum = 2;
                }

                EscMenuButtonAni();
                GetV_AxisTime = 0f;
                Debug.Log("R: " + ResumeButtonNum + " S: " + SystemButtonNum + " M: " + MainMenuButtonNum);
            }
            if ((Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") == 1 && GetV_AxisTime >= 0.15f)) && !SystemIsOpen) {
                ResumeButtonNum++;
                SystemButtonNum++;
                MainMenuButtonNum++;

                if (MainMenuButtonNum > 4) {
                    ResumeButtonNum = 2;
                    SystemButtonNum = 3;
                    MainMenuButtonNum = 4;
                }

                EscMenuButtonAni();
                GetV_AxisTime = 0f;
                Debug.Log("R: " + ResumeButtonNum + " S: " + SystemButtonNum + " M: " + MainMenuButtonNum);
            }



        }// EscMenu Open
        else {
            UITweener = EscMenuBG.transform.DOLocalMove(new Vector3(800, 0, 0), 0.5f).SetUpdate(true);
            Resume.interactable = false;
            System.interactable = false;
            MainMenu.interactable = false;
            DefalutSelectResume = false;
            ResumeButtonNum = 2;
            SystemButtonNum = 3;
            MainMenuButtonNum = 4;


            /*Resume.gameObject.transform.localPosition = new Vector3(0, EscButtonYArray[ResumeButtonYNum],0);
            System.gameObject.transform.localPosition = new Vector3(0, EscButtonYArray[SystemButtonYNum], 0);
            MainMenu.gameObject.transform.localPosition = new Vector3(0, EscButtonYArray[MainMenuButtonYNum], 0);*/

            EscMenuButtonAni();

        }// EscMenu Close

    }// EscMenuCon end

    void EscMenuButtonAni() {
        UITweener = Resume.gameObject.transform.DOLocalMove(new Vector3(EscButtonXArray[ResumeButtonNum], EscButtonYArray[ResumeButtonNum], 0), EscMenuMoveYTime).SetUpdate(true);
        UITweener = Resume.gameObject.transform.DOLocalRotate(new Vector3(0, 0, EscButtonRotationZArray[ResumeButtonNum]), EscMenuMoveYTime).SetUpdate(true);
        if (ResumeButtonNum == 2)
        {
            UITweener = Resume.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.8f), 0.5f).SetUpdate(true);
        }
        else {
            UITweener = Resume.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.6f), 0.5f).SetUpdate(true);
        }

        UITweener = System.gameObject.transform.DOLocalMove(new Vector3(EscButtonXArray[SystemButtonNum], EscButtonYArray[SystemButtonNum], 0), EscMenuMoveYTime).SetUpdate(true);
        UITweener = System.gameObject.transform.DOLocalRotate(new Vector3(0, 0, EscButtonRotationZArray[SystemButtonNum]), EscMenuMoveYTime).SetUpdate(true);
        if (SystemButtonNum == 2)
        {
            UITweener = System.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.8f), 0.5f).SetUpdate(true);
        }
        else
        {
            UITweener = System.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.6f), 0.5f).SetUpdate(true);
        }

        UITweener = MainMenu.gameObject.transform.DOLocalMove(new Vector3(EscButtonXArray[MainMenuButtonNum], EscButtonYArray[MainMenuButtonNum], 0), EscMenuMoveYTime).SetUpdate(true);
        UITweener = MainMenu.gameObject.transform.DOLocalRotate(new Vector3(0, 0, EscButtonRotationZArray[MainMenuButtonNum]), EscMenuMoveYTime).SetUpdate(true);
        if (MainMenuButtonNum == 2)
        {
            UITweener = MainMenu.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.8f), 0.5f).SetUpdate(true);
        }
        else
        {
            UITweener = MainMenu.transform.GetChild(0).GetComponent<Text>().DOColor(new Color(0, 0, 0, 0.6f), 0.5f).SetUpdate(true);
        }
    }

    float GetH_AxisTime = 0;
    bool IsDefalutSelectSystemBack = false;
    public int SystemNowSelectNum = 4;
    int ScreenWidth = 1920;
    int ScreenHeight = 1080;
    bool IsFullScreen = true;

    void EscMenuButtonSelectChecker(int RNum, int SNum, int MNum) {

        if (Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1) {
            GetH_AxisTime += Time.unscaledDeltaTime;
        }

        if (RNum == 2)
        {
            Resume.Select();
            UITweener = EscMenuBGLeft.transform.DOLocalMove(new Vector3(-1280, 0, 0), 0.5f).SetUpdate(true);// default -1280
            SystemIsOpen = false;
            SoundSlider.interactable = false;

        }
        else if (SNum == 2)
        {
            
            if (SystemIsOpen)
            {              
                Resume.interactable = false;
                MainMenu.interactable = false;
                SoundSlider.interactable = true;
                ScreenModeSlider.interactable = true;
                ResolutionSlider.interactable = true;
                SystemBackButton.interactable = true;

                if ((Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Joystick1Button1))) {
                    SystemIsOpen = false;
                    UITweener = EscMenuBGLeft.transform.DOLocalMove(new Vector3(-1280, 0, 0), 0.5f).SetUpdate(true);// default -1280
                    SystemBackClick();
                }

                if (!IsDefalutSelectSystemBack) {
                    SystemBackButton.Select();
                    IsDefalutSelectSystemBack = true;
                }

                if (SystemNowSelectNum == 1)
                {
                    ScreenModeSlider.Select();
                }
                else if (SystemNowSelectNum == 2) {
                    ResolutionSlider.Select();
                }
                else if (SystemNowSelectNum == 3)
                {
                    SoundSlider.Select();
                }
                else if (SystemNowSelectNum == 4)
                {
                    SystemBackButton.Select();
                }

                if (Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") == -1 && GetV_AxisTime >= 0.15f)) {
                    if (SystemNowSelectNum >= 4)
                    {
                        SystemNowSelectNum = 4;
                    }
                    else
                    {
                        SystemNowSelectNum++;
                        GetV_AxisTime = 0f;
                    }
                }
                if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") == 1 && GetV_AxisTime >= 0.15f))
                {
                    if (SystemNowSelectNum <= 1)
                    {
                        SystemNowSelectNum = 1;
                    }
                    else
                    {
                        SystemNowSelectNum--;
                        GetV_AxisTime = 0f;
                    }
                }

                if (SystemNowSelectNum == 1) {
                    if (ScreenModeSlider.value == 0)
                    {
                        IsFullScreen = true;
                    }
                    else if (ScreenModeSlider.value == 1) {
                        IsFullScreen = false;
                    }
                }// Screen Mode
                if (SystemNowSelectNum == 2)
                {
                    if (ResolutionSlider.value == 0) {
                        ScreenWidth = 1920;
                        ScreenHeight = 1080;
                        ResolutionValueText.text = "1920*1080";
                    }
                    if (ResolutionSlider.value == 1)
                    {
                        ScreenWidth = 1600;
                        ScreenHeight = 900;
                        ResolutionValueText.text = "1600*900";
                    }
                    if (ResolutionSlider.value == 2)
                    {
                        ScreenWidth = 800;
                        ScreenHeight = 600;
                        ResolutionValueText.text = "800*600";
                    }
                }// Resolution
                

                if ((Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < 0 && GetH_AxisTime > 0.5f) && SystemNowSelectNum == 3)
                {
                    SoundSlider.value -= 0.1f;
                    GetH_AxisTime = 0;
                }// Left
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0 && GetH_AxisTime > 0.5f) && SystemNowSelectNum == 3)
                {
                    SoundSlider.value += 0.1f;
                    GetH_AxisTime = 0;
                }// Right
                BGMM.gameObject.GetComponent<AudioSource>().volume = SoundSlider.value;//set sound volume
            }
            else {
                Resume.interactable = true;
                MainMenu.interactable = true;
                SoundSlider.interactable = false;
                ScreenModeSlider.interactable = false;
                ResolutionSlider.interactable = false;
                SystemBackButton.interactable = false;
                System.Select();
                IsDefalutSelectSystemBack = false;
            }
        }
        else if (MNum == 2) {
            MainMenu.Select();
            UITweener = EscMenuBGLeft.transform.DOLocalMove(new Vector3(-1280, 0, 0), 0.5f).SetUpdate(true);// default -1280
            SystemIsOpen = false;
            SoundSlider.interactable = false;

        }

    }// EscMenuButtonSelectChecker end

    public void EscResumeClick() {
        UITweener = EscMenuBG.transform.DOLocalMove(new Vector3(1400, 0, 0), 0.5f).SetUpdate(true);
        Resume.interactable = false;
        System.interactable = false;
        MainMenu.interactable = false;
        PlayerCon.IsEscMenu = false;
        SoundSlider.interactable = false;
    }

    public void EscSystemClick() {
        UITweener = EscMenuBGLeft.transform.DOLocalMove(new Vector3(0,0,0),0.5f).SetUpdate(true);// default -1280
        Resume.interactable = false;
        System.interactable = false;
        MainMenu.interactable = false;

        SystemIsOpen = true;
        SoundSlider.interactable = true;
    }

    public void EscMainMenuClick() { }

    public void SystemBackClick() {
        SystemIsOpen = false;
        UITweener = EscMenuBGLeft.transform.DOLocalMove(new Vector3(-1280, 0, 0), 0.5f).SetUpdate(true);
        Screen.SetResolution(ScreenWidth,ScreenHeight,IsFullScreen);
    }

    //--- EscMenu

}
