using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterValueChange : MonoBehaviour {

    Animator Player_Ani;

    public Text WaterValue;
    public Text BackWaterValue;
    public Image TextMask;
    public Image ImageFillValue;
    
    public bool IsLosingWater = false;

    public float WaterChangePerSec = 0; // 5.5

    [Range(0,100)]
    public float ImgWaterValue = 100;
    public float TextWaterValue = 100;

	// Use this for initialization
	void Start () {
        Player_Ani = GameObject.Find("Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckValueFunc();
        if (IsLosingWater)
        {
            StartCoroutine("WaterChange");
        }
        

    }



    IEnumerator WaterChange() {              
        if (ImgWaterValue > 0)
        {
            
            if (Player_Ani.GetBool("PlayerRun")) {
                ImgWaterValue -= 0.25f;
            }
            else {
                ImgWaterValue -= 0.1f;
            }
            yield return new WaitForSeconds(1f);
        }
        if (ImgWaterValue <= 0)
        {
            IsLosingWater = false;
        }
    }

    void CheckValueFunc() {
        if (ImgWaterValue <= 58 && ImgWaterValue >= 42) {

            if (ImgWaterValue <= 47) {
                WaterChangePerSec = 1;
            }
            if (ImgWaterValue >= 47 && ImgWaterValue <= 50) {
                WaterChangePerSec = 0.9f;
            }
            if (ImgWaterValue >= 50 && ImgWaterValue <= 58) {
                WaterChangePerSec = 0.8f;
            }
            TextWaterValue = (100/13)*(ImgWaterValue - 42)*WaterChangePerSec;
        }

        // 42-48 5.5
        
        // --- 變更數值

        ImageFillValue.fillAmount = ImgWaterValue / 100;
        TextMask.fillAmount = TextWaterValue / 100;
        WaterValue.text = Mathf.Floor(ImgWaterValue).ToString() + "%";
        BackWaterValue.text = Mathf.Floor(ImgWaterValue).ToString() + "%";
        // --- 判定
        if (ImgWaterValue <= 0) {
            ImgWaterValue = 0;
        }
        if (TextWaterValue <= 0) {
            TextWaterValue = 0;
        }
        if (ImgWaterValue > 56) {
            TextWaterValue = 100;
        }
        if (ImgWaterValue < 39) {
            TextWaterValue = 0;
        }
    }

    public void DefaultValue() {
        ImgWaterValue = 0;
        TextWaterValue = 0;
        ImageFillValue.fillAmount = ImgWaterValue;
        TextMask.fillAmount = TextWaterValue;
    }

}
