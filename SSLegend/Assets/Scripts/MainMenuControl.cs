﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour {


    public GameObject PressToStart;
    public GameObject AllStuff;

    public Button StartBtn;
    public Button LoadBtn;
    public Button OptionBtn;
    public Button ExitBtn;

    public GameObject Arrow;
    public GameObject Left;
    public GameObject Right;

    //--- Option

    public GameObject OptionPage;
    public GameObject OptionPointer;

    public GameObject ScreenPointer;

    public Text ResText;
    public Text SoundText;
    public GameObject ResSlider;
    public GameObject SoundSlider;
    public AudioSource BGM;
   

    bool FullScreenMode = true;
    int ScreenWidth = 1920;
    int ScreenHeight = 1080;
    int ScreenResMode = 2;
    float SoundValue = 100;

    [Header("設定頁面")]
    public bool IsOptionOpen = false;
    public float OPressTime = 0;
    float OMaxPressTime = 0.2f;
    int OSelectNum = 0;

    [Header("任意繼續")]
    public bool IsPressStart = false;
    
    float input_H = 0;

    [Header("音效")]
    public AudioClip[] UISE;
    public AudioSource UIAS;

    //--- Option

    public int SelectNum = 0;
    public float PressTime = 0;
    public float input_V = 0;
    float MaxPressTime = 0.2f;

    LoadingControl Load;
    DataManger DM;
    PlayerControl PlayerCon;

    public float OptionCoolTime;
    public float DefaultOptionCoolTime;

    public float WaitCanPressTime = 1f;

    public bool IsCanPressStart = false;

    // Use this for initialization
    void Start () {
        StartBtn.Select();
        Load = FindObjectOfType<LoadingControl>();
        Arrow = GameObject.Find("Arrow");
        Left = Arrow.transform.GetChild(0).gameObject;
        Right = Arrow.transform.GetChild(1).gameObject;
        DM = FindObjectOfType<DataManger>();
        UIAS = this.gameObject.GetComponent<AudioSource>();
        PlayerCon = FindObjectOfType<PlayerControl>();

        OptionPage = GameObject.Find("OptionPage");
        OptionPointer = GameObject.Find("OptionPointer");
        ScreenPointer = GameObject.Find("ScreenPointer");
        ResText = GameObject.Find("ResolutionText").GetComponent<Text>();
        ResSlider = GameObject.Find("R_SettingBarSlider");
        SoundText = GameObject.Find("SoundText").GetComponent<Text>();
        SoundSlider = GameObject.Find("S_SettingBarSlider");
        PressToStart = GameObject.Find("PressToStart");
        AllStuff = GameObject.Find("AllStuff");



        DM.LoadFileFunction();

        OptionCoolTime = DefaultOptionCoolTime;

    }

    // Update is called once per frame
    void Update () {
        input_V = Input.GetAxis("Vertical");
        input_H = Input.GetAxis("Horizontal");
        if (!IsOptionOpen && IsPressStart) {
            Checker();
        }
        OptionPageCon();
       //Debug.Log(ExitBtn.transform.position.y);
        PressStart();

    }

    void Checker() {

        if (SelectNum == 0) {
            StartBtn.Select();
            StartBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0.5f);
            LoadBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            OptionBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            ExitBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            Arrow.transform.DOLocalMove(new Vector3(17,23,0),0.5f);
            Left.transform.DOLocalMove(new Vector3(-59, 0, 0),0.5f);
            Right.transform.DOLocalMove(new Vector3(55, 0, 0), 0.5f);

        }
        if (SelectNum == 1)
        {
            LoadBtn.Select();
            StartBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            LoadBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f);
            OptionBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            ExitBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            Arrow.transform.DOLocalMove(new Vector3(17, -16, 0), 0.5f);
            Left.transform.DOLocalMove(new Vector3(-50, 0, 0), 0.5f);
            Right.transform.DOLocalMove(new Vector3(50, 0, 0), 0.5f);
        }
        if (SelectNum == 2)
        {
            OptionBtn.Select();
            StartBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            LoadBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            OptionBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f);
            ExitBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            Arrow.transform.DOLocalMove(new Vector3(17, -50, 0), 0.5f);
            Left.transform.DOLocalMove(new Vector3(-58, 0, 0), 0.5f);
            Right.transform.DOLocalMove(new Vector3(56, 0, 0), 0.5f);
        }
        if (SelectNum == 3)
        {
            ExitBtn.Select();
            StartBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            LoadBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            OptionBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
            ExitBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f);
            Arrow.transform.DOLocalMove(new Vector3(17, -85, 0), 0.5f);
            Left.transform.DOLocalMove(new Vector3(-45, 0, 0), 0.5f);
            Right.transform.DOLocalMove(new Vector3(42, 0, 0), 0.5f);
        }

        if (Input.GetAxis("Vertical") != 0) {
            PressTime += Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0 && SelectNum < 3 && PressTime > MaxPressTime) {
            SelectNum += 1;
            PressTime = 0;
            UIAS.PlayOneShot(UISE[0]);
        }
        else if(Input.GetAxis("Vertical") > 0 && SelectNum > 0 && PressTime > MaxPressTime)
        {
            SelectNum -= 1;
            PressTime = 0;
            UIAS.PlayOneShot(UISE[0]);
        }

    }

    IEnumerator PressToStartAnimation() {
        PressAni = true;
        PressToStart.GetComponent<CanvasGroup>().DOFade(0, 1);
        yield return new WaitForSeconds(1);
        PressToStart.GetComponent<CanvasGroup>().DOFade(1, 1);
        yield return new WaitForSeconds(1);
        PressAni = false;

    }

    IEnumerator DelayCanPress() {
        yield return new WaitForSeconds(2f);
        AllStuff.GetComponent<CanvasGroup>().interactable = true;
    }

    bool PressAni = false;

    void PressStart() {

        if (!IsPressStart && IsCanPressStart)
        {
            if (Input.anyKeyDown)
            {
                StopCoroutine("PressToStartAnimation");
                PressAni = true;
                PressToStart.GetComponent<CanvasGroup>().DOKill();
                AllStuff.GetComponent<CanvasGroup>().DOFade(1, 1f);
                PressToStart.GetComponent<CanvasGroup>().DOFade(0,1f);
                IsPressStart = true;
                StartCoroutine("DelayCanPress");
            }
        }
        
        if (!PressAni && !IsPressStart) {
                StartCoroutine("PressToStartAnimation");
         }
        

    }

    IEnumerator WaitToVideo()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);

    }

    public void StartClick() {
        //Debug.Log("222");
        UIAS.PlayOneShot(UISE[1]);       
        DM.DeleteFile(Application.dataPath + "/Save", "Save.txt");
        DM.SaveFile(Application.dataPath + "/Save", "Save.txt");
        
        StartCoroutine("WaitToVideo");
    }

    public void LoadGameClick() {

        SceneManager.LoadScene(DM.InfoAll[8].ToString());
        //Debug.Log(DM.InfoAll[8]);
    }

    public void OptionClick() {

        IsOptionOpen = true;
        ExitBtn.gameObject.GetComponent<RectTransform>().DOLocalMoveY(-175f,0.5f);
        OptionPage.GetComponent<CanvasGroup>().DOFade(1, 1.5f);
    }

    

    void OptionPageCon() {
        if (IsOptionOpen)
        {
            StartBtn.enabled = false;
            LoadBtn.enabled = false;
            OptionBtn.enabled = false;
            ExitBtn.enabled = false;

            if (OptionCoolTime > 0)
            {
                OptionCoolTime -= Time.deltaTime;
            }
            else if (OptionCoolTime == 0) {
                OptionCoolTime = 0;
            }

            if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && OptionCoolTime <= 0)
            {
                StartBtn.enabled = true;
                LoadBtn.enabled = true;
                OptionBtn.enabled = true;
                ExitBtn.enabled = true;
                IsOptionOpen = false;
                ExitBtn.gameObject.GetComponent<RectTransform>().DOLocalMoveY(-105f, 0.5f);
                OptionPage.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
                OptionCoolTime = DefaultOptionCoolTime;
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                OPressTime += Time.deltaTime;
            }
            if (Input.GetAxis("Vertical") < 0 && OSelectNum < 2 && OPressTime > OMaxPressTime)
            {
                OSelectNum += 1;
                OPressTime = 0;
            }
            else if (Input.GetAxis("Vertical") > 0 && OSelectNum > 0 && OPressTime > OMaxPressTime)
            {
                OSelectNum -= 1;
                OPressTime = 0;
            }

            if (OSelectNum == 0)
            {
                OptionPointer.transform.DOLocalMoveY(-21, 0.5f);

                if (Input.GetAxis("Horizontal") > 0 && FullScreenMode)
                {
                    FullScreenMode = false;// window
                    ScreenPointer.transform.localPosition = new Vector3(858, -5, 0);
                    Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreenMode);
                    OPressTime = 0;
                }
                else if (Input.GetAxis("Horizontal") < 0 && !FullScreenMode)
                {
                    FullScreenMode = true;// full
                    ScreenPointer.transform.localPosition = new Vector3(487, -5, 0);
                    Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreenMode);
                    OPressTime = 0;
                }

            }// screen mode
            if (OSelectNum == 1)
            {
                OptionPointer.transform.DOLocalMoveY(-45, 0.5f);
                if (Input.GetAxis("Horizontal") > 0 && OPressTime > OMaxPressTime && ScreenResMode < 2)
                {
                    ScreenResMode++;
                    OPressTime = 0;
                }
                else if (Input.GetAxis("Horizontal") < 0 && OPressTime > OMaxPressTime && ScreenResMode > 0)
                {
                    ScreenResMode--;
                    OPressTime = 0;
                }

                if (ScreenResMode == 0)
                {
                    ScreenWidth = 800;
                    ScreenHeight = 600;
                    ResSlider.transform.DOLocalMoveX(988, 0.5f);
                }
                if (ScreenResMode == 1)
                {
                    ScreenWidth = 1600;
                    ScreenHeight = 900;
                    ResSlider.transform.DOLocalMoveX(1238, 0.5f);
                }
                if (ScreenResMode == 2)
                {
                    ScreenWidth = 1920;
                    ScreenHeight = 1080;
                    ResSlider.transform.DOLocalMoveX(1472, 0.5f);
                }
                Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreenMode);
                ResText.text = (ScreenWidth + "x" + ScreenHeight);

            }// res
            if (OSelectNum == 2)
            {
                OptionPointer.transform.DOLocalMoveY(-66, 0.5f);

                if (Input.GetAxis("Horizontal") > 0 && OPressTime > OMaxPressTime && SoundValue < 100)
                {
                    SoundValue += 10f;
                    SoundSlider.transform.localPosition += new Vector3(48.8f, 0, 0);
                    OPressTime = 0;
                }
                else if (Input.GetAxis("Horizontal") < 0 && OPressTime > OMaxPressTime && SoundValue > 0)
                {
                    SoundValue -= 10f;
                    SoundSlider.transform.localPosition -= new Vector3(48.8f, 0, 0);
                    OPressTime = 0;
                }

                SoundText.text = SoundValue.ToString();
                BGM.volume = SoundValue;

            }// sound


        }
        else {

        }
    }

    public void ExitClick() {
        Application.Quit();
    }

}
