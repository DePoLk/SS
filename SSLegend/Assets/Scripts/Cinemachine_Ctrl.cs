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
    public GameObject[] Obstacles;

    private CameraControl CC;
    public bool TrackEnd = false;
    public bool TrackStart = false;
    public bool EndingStart = false;
    public bool EndingEnd = false;
    int times = 0;
    int Endtimes = 0;

    PlayerUI PUI;
    PlayerControl PlayerCon;
    GameObject MainCam;
    public GameObject Boss;
    BGMManager bgm;
    DataManger DM;

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
        DM = FindObjectOfType<DataManger>();
    }

    // Update is called once per frame
    void Update()
    {

        if (TrackStart && times == 0)
        {
            v_Cam.VirtualCameraGameObject.SetActive(false);
            PD.Play();
            times += 1;
        }
        if (EndingStart && Endtimes == 0)
        {
            v_GroupCam.VirtualCameraGameObject.SetActive(false);
            bgm.cam_audi.Stop();
            DeathObject[1].SetActive(true);
            DeathObject[2].SetActive(true);
            CinemachineUI.SetActive(true);
            Obstacles[0].SetActive(false);
            Obstacles[1].SetActive(false);
            PlayerCon.IsInCinema = true;
            //PlayerCon.gameObject.transform.localScale = new Vector3(0, 0, 0);
            PlayerCon.BearModel.SetActive(false);
            PlayerCon.FatBearModel.SetActive(false);
            PUI.gameObject.SetActive(false);
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
            Obstacles[0].SetActive(true);
            Obstacles[1].SetActive(true);
            //--- ReInGame
            PUI.gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
            PlayerCon.IsInCinema = false;
            MainCam.transform.eulerAngles = new Vector3(40, -90, 0);
            Boss.SetActive(true);
            PlayerCon.BearModel.SetActive(true);
            PlayerCon.transform.position = new Vector3(14, 0, 1);
            DM.DeleteFile(Application.dataPath + "/Save", "Save.txt");
            DM.SaveFile(Application.dataPath + "/Save", "Save.txt");
            PUI.BossHPUI.GetComponent<CanvasGroup>().alpha = 1;
            PUI.BossGetHp();
            PUI.GetInAniIsDone = true;
            

            //PlayerCon.FatBearModel.SetActive(true);
            //--- ReInGame
        }

        if (Dead.state == PlayState.Paused && EndingEnd == false && EndingStart == true)
        {
            Debug.Log("1");
            EndingEnd = true;
            EndingStart = false;
            CC.hasPlayTrack = false;
            Invoke("ChangeToMenu",5.0f);

        }

    }
    public void ChangeToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
