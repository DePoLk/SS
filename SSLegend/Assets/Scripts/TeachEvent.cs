using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class TeachEvent : MonoBehaviour {

    PlayerControl PlayerCon;

    Image TeachImage;

    [Header("事件數據")]
    public int EventStartIndex;
    public int EventEndIndex;
    public int NowTeachEventNum;
    public Sprite[] TeachImageArray;
    bool IsCanChange = true;
    public GameObject WaitEvent;

    // Use this for initialization
    void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();
        TeachImage = GameObject.Find("TeachImage").GetComponent<Image>();
        NowTeachEventNum = EventStartIndex;
	}
	
	// Update is called once per frame
	void Update () {
        TeachEventHandler();
	}


    IEnumerator ChangeImage() {
        IsCanChange = false;
        TeachImage.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.75f);
        TeachImage.sprite = TeachImageArray[NowTeachEventNum];
        yield return new WaitForSecondsRealtime(0.25f);
        TeachImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true);
        IsCanChange = true;
    }

    void TeachEventHandler() {

        if (PlayerCon.IsTeaching) {

            if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button0)) && IsCanChange && NowTeachEventNum == EventEndIndex) {
                TeachImage.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true);
                PlayerCon.IsTeaching = false;
                Destroy(this.gameObject, 1f);
            }

            if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button0)) && IsCanChange) {
                
                if (NowTeachEventNum < EventEndIndex)
                {
                    NowTeachEventNum++;
                    StartCoroutine("ChangeImage");
                }
              
            }

            if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && IsCanChange)
            {
                if (NowTeachEventNum > EventStartIndex)
                {
                    NowTeachEventNum--;
                    StartCoroutine("ChangeImage");
                }
               
            }


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerCon.IsUIEventing && WaitEvent == null) {

            PlayerCon.IsTeaching = true;
            TeachImage.sprite = TeachImageArray[EventStartIndex];
            TeachImage.gameObject.GetComponent<CanvasGroup>().DOFade(1,0.5f).SetUpdate(true);

        }
    }


}
