using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Cinemachine;


public class Cinemachine_Ctrl : MonoBehaviour {
    public PlayableDirector PD;
    CinemachineBrain CineBrain;
    public GameObject[] EndToDelete;

    private CameraControl CC;
    public bool TrackEnd = false;
    public bool TrackStart = false;
    int times = 0;

    PlayerUI PUI;
    PlayerControl PlayerCon;
    GameObject MainCam;
    public GameObject Boss;

    // Use this for initialization
    void Start () {
        CineBrain = GetComponent<CinemachineBrain>();
        CC = FindObjectOfType<CameraControl>();
        PUI = FindObjectOfType<PlayerUI>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        MainCam = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {
        
        if (TrackStart && times==0) {
            CineBrain.enabled = true;
            PD.Play();
            times += 1;
        }
        if (PD.state == PlayState.Paused && TrackEnd == false && TrackStart == true)
        {
            CineBrain.enabled = false;
            Debug.Log("complete");
            for (int i = 0; i < EndToDelete.Length; i++)
            {
                EndToDelete[i].SetActive(false);
            }
            TrackEnd = true;
            TrackStart = false;
            CC.hasPlayTrack = false;


            //--- ReInGame
            PUI.gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
            PlayerCon.IsInCinema = false;
            MainCam.transform.eulerAngles = new Vector3(40,-90,0);
            Boss.SetActive(true);
            PlayerCon.gameObject.transform.localScale = new Vector3(15, 15, 15);
            //--- ReInGame
        }

    }
}
