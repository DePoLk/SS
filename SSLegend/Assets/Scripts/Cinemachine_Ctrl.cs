using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;


public class Cinemachine_Ctrl : MonoBehaviour {
    public PlayableDirector PD;
    public PlayableDirector Dead;
    CinemachineBrain CineBrain;
    public CinemachineVirtualCameraBase v_Cam;
    public CinemachineVirtualCameraBase v_GroupCam;
    public GameObject[] ShowUpObject;
    public GameObject[] DeathObject;
    public GameObject CinemachineUI;

    private CameraControl CC;
    public bool TrackEnd = false;
    public bool TrackStart = false;
    public bool EndindStart = false;
    public bool EndingEnd = false;
    int times = 0;
    int Endtimes = 0;

    PlayerUI PUI;
    PlayerControl PlayerCon;
    GameObject MainCam;
    public GameObject Boss;
    BGMManager bgm;

    // Use this for initialization
    void Start () {
        CineBrain = GetComponent<CinemachineBrain>();
        CC = FindObjectOfType<CameraControl>();
        PUI = FindObjectOfType<PlayerUI>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        MainCam = GameObject.Find("Main Camera");
        v_Cam.VirtualCameraGameObject.SetActive(true);
        v_GroupCam.VirtualCameraGameObject.SetActive(false);
        bgm = FindObjectOfType<BGMManager>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (TrackStart && times==0) {
            v_Cam.VirtualCameraGameObject.SetActive(false);
            PD.Play();
            times += 1;
        }
        if (EndindStart && Endtimes == 0)
        {
            v_GroupCam.VirtualCameraGameObject.SetActive(false);
            bgm.cam_audi.Stop();
            DeathObject[1].SetActive(true);
            DeathObject[2].SetActive(true);
            CinemachineUI.SetActive(true);
            Dead.Play();
            Endtimes += 1;
        }
        if (PD.state == PlayState.Paused && TrackEnd == false && TrackStart == true)
        {
            
            
            for (int i = 0; i < ShowUpObject.Length; i++)
            {
                ShowUpObject[i].SetActive(false);
            }
            TrackEnd = true;
            TrackStart = false;
            CC.hasPlayTrack = false;
            CinemachineUI.SetActive(false);
            v_GroupCam.VirtualCameraGameObject.SetActive(true);

            //--- ReInGame
            PUI.gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
            PlayerCon.IsInCinema = false;
            MainCam.transform.eulerAngles = new Vector3(40,-90,0);
            Boss.SetActive(true);
            PlayerCon.gameObject.transform.localScale = new Vector3(15, 15, 15);
            //--- ReInGame
        }

        if (Dead.state == PlayState.Paused && EndingEnd == false && EndindStart == true) {

            EndingEnd = true;
            EndindStart = false;
            CC.hasPlayTrack = false;
            

        }

    }
   
   
}
