using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTrigger : MonoBehaviour {

    private Cinemachine_Ctrl cinemachineCtrl;
    private CameraControl CC;
    public GameObject bear;
    public GameObject player;
    public GameObject UI;
    public GameObject Flamelight;

    PlayerUI PUI;
    PlayerControl PlayerCon;
    BGMManager BGMM;
	// Use this for initialization
	void Start () {
        cinemachineCtrl = FindObjectOfType<Cinemachine_Ctrl>();
        CC = FindObjectOfType<CameraControl>();
        //bear.SetActive(false);
        UI.SetActive(false);
        PUI = FindObjectOfType<PlayerUI>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        BGMM = FindObjectOfType<BGMManager>();
        Flamelight.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider a)
    {
        if (a.CompareTag("Player")) {
            CC.hasPlayTrack = true;
            bear.SetActive(true);
            UI.SetActive(true);
            Flamelight.SetActive(true);
            cinemachineCtrl.TrackStart = true;
           
            Destroy(this,1.0f);

            PUI.gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
            PlayerCon.IsInCinema = true;
            BGMM.ChangeToBoss();
            player.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
