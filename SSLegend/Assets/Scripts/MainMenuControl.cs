using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour {

    public Button StartBtn;
    public Button LoadBtn;
    public Button OptionBtn;
    public Button ExitBtn;

    public int SelectNum = 0;
    public float PressTime = 0;
    public float input_V = 0;
    float MaxPressTime = 0.5f;

    LoadingControl Load;

	// Use this for initialization
	void Start () {
        StartBtn.Select();
        Load = FindObjectOfType<LoadingControl>();

    }
	
	// Update is called once per frame
	void Update () {
        input_V = Input.GetAxis("Vertical");
        Checker();
    }

    void Checker() {

        if (SelectNum == 0) {
            StartBtn.Select();
            StartBtn.transform.GetChild(0).transform.DOScale(2, 0.5f);
            LoadBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            OptionBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            ExitBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
        }
        if (SelectNum == 1)
        {
            LoadBtn.Select();
            StartBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            LoadBtn.transform.GetChild(0).transform.DOScale(2, 0.5f);
            OptionBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            ExitBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
        }
        if (SelectNum == 2)
        {
            OptionBtn.Select();
            StartBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            LoadBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            OptionBtn.transform.GetChild(0).transform.DOScale(2, 0.5f);
            ExitBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
        }
        if (SelectNum == 3)
        {
            ExitBtn.Select();
            StartBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            LoadBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            OptionBtn.transform.GetChild(0).transform.DOScale(1, 0.5f);
            ExitBtn.transform.GetChild(0).transform.DOScale(2, 0.5f);
        }

        if (Input.GetAxis("Vertical") != 0) {
            PressTime += Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0 && SelectNum < 3 && PressTime > MaxPressTime) {
            SelectNum += 1;
            PressTime = 0;
        }
        else if(Input.GetAxis("Vertical") > 0 && SelectNum > 0 && PressTime > MaxPressTime)
        {
            SelectNum -= 1;
            PressTime = 0;
        }

    }


    public void StartClick() {
        Load.ChangeScene();
    }

}
