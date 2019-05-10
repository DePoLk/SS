using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class CannonControl : MonoBehaviour {

    PlayerControl PlayerCon;
    Transform AimZonePos;
    Animator Player_Ani;
    BillBoard BB;

    [Header("大砲部件")]
    public GameObject CannonTop;
    public GameObject CannonLittleWheel;
    public GameObject CannonBigWheel;
    public GameObject AimZone;

    Tweener CannonTweener;

    float DefaultLoadBearTime = 1f;
    float LoadBearTime;

    float FlyingTime;
    float DefaultFlyingTime = 0.5f;

    float AimSpeed = 0.01f;

    public float CannonRotateSpeed = 0;
    public float BarrelRotateSpeed = 0;

    float GetBarrelRotateRange = 0;
    private AudioSource audi;
    public AudioClip shootSE;

    // Use this for initialization
    void Start () {
        audi = GetComponent<AudioSource>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        Player_Ani = PlayerCon.GetComponent<Animator>();
        BB = FindObjectOfType<BillBoard>();
        AimZonePos = GameObject.Find("AimZone").GetComponent<Transform>();
        AimZone = GameObject.Find("AimZone");
        FlyingTime = DefaultFlyingTime;
        LoadBearTime = DefaultLoadBearTime;
        AimZonePos.gameObject.GetComponent<Projector>().enabled = false;
        AimZone.SetActive(false);

    }

    bool IsShot = false;
	// Update is called once per frame
	void Update () {
        CannonAim();
        if (IsShot) {
                PlayerCon.gameObject.transform.DOScale(15,0.5f);
            AimZone.SetActive(false);
            if (FlyingTime > 0)
            {
                FlyingTime -= Time.deltaTime;
            }
            else if (PlayerCon.IsGround) {
                /*PlayerCon.transform.DOKill();
                PlayerCon.gameObject.transform.position += new Vector3(0,3f,0);*/
                FlyingTime = DefaultFlyingTime;
                IsShot = false;
                PlayerCon.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                PlayerCon.IsInCannon = false;
                AimZonePos.gameObject.GetComponent<Projector>().enabled = false;
                AimZonePos.localPosition = new Vector3(-0.3f, 0.2f, 0.062f);

            }
            else {
                FlyingTime = DefaultFlyingTime;
                IsShot = false;
                PlayerCon.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                PlayerCon.IsInCannon = false;
                AimZonePos.gameObject.GetComponent<Projector>().enabled = false;
                AimZonePos.localPosition = new Vector3(-0.3f,0.2f,0.062f);
            }
        }//If(IsShot)

        if (PlayerCon.IsInCannon) {
            PlayerCon.IsInvincible = true;
        }
        

	}

    void CannonAim() {
        if (PlayerCon.IsInCannon)
        {
            PlayerCon.RunTime = 0;
            AimZone.SetActive(true);
            if (LoadBearTime > 0 && PlayerCon.IsInCannon)
            {
                CannonTop.transform.GetChild(0).transform.DORotate(new Vector3(0, 0, -90), 0.5f);
            }
            else {
                //CannonTop.transform.GetChild(0).transform.DORotate(new Vector3(0, 0, -50), 0.5f);
            }

            AimZonePos.gameObject.GetComponent<Projector>().enabled = true;
            LoadBearTime -= Time.deltaTime;

            if (Input.GetAxis("Horizontal") > 0 && LoadBearTime <= 0)
            {
                if (AimZonePos.localPosition.z < 0.38f) {
                    AimZonePos.localPosition += new Vector3(0, 0, AimSpeed);
                    CannonLittleWheel.transform.DOLocalRotate(new Vector3(0, 0, 90), 5f);
                    CannonBigWheel.transform.DOLocalRotate(new Vector3(0, 0, -90), 5f);
                }           
            }// right
            if (Input.GetAxis("Horizontal") < 0 && LoadBearTime <= 0)
            {
                if (AimZonePos.localPosition.z > -0.5f) {
                    AimZonePos.localPosition -= new Vector3(0, 0, AimSpeed);
                    CannonLittleWheel.transform.DOLocalRotate(new Vector3(0, 0, -90), 5f);
                    CannonBigWheel.transform.DOLocalRotate(new Vector3(0, 0, 90), 5f);
                }
            }// left
            if (Input.GetAxis("Vertical") > 0 && LoadBearTime <= 0)
            {
                if (AimZonePos.localPosition.x > -1.5f) {
                    AimZonePos.localPosition += new Vector3(-AimSpeed, 0, 0);
                    CannonBigWheel.transform.DOLocalRotate(new Vector3(0, 0, -90), 5f);
                    CannonLittleWheel.transform.DOLocalRotate(new Vector3(0, 0, 90), 5f);
                }               
            }// up
            if (Input.GetAxis("Vertical") < 0 && LoadBearTime <= 0)
            {
                if (AimZonePos.localPosition.x < -0.28f) {
                    AimZonePos.localPosition -= new Vector3(-AimSpeed, 0, 0);
                    CannonBigWheel.transform.DOLocalRotate(new Vector3(0, 0, 90), 5f);
                    CannonLittleWheel.transform.DOLocalRotate(new Vector3(0, 0, -90), 5f);
                }
            }// down

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                if (Input.GetAxis("Horizontal") > 0) {
                    //Debug.Log(CannonTop.transform.GetChild(1).transform.localEulerAngles.y);
                    if (GetBarrelRotateRange < 30f)
                    {
                        GetBarrelRotateRange++;
                        CannonTop.transform.Rotate(new Vector3(0, CannonRotateSpeed, 0));
                    }
                    else {
                        GetBarrelRotateRange = 30;
                    }
                }
                if (Input.GetAxis("Horizontal") < 0) {
                    //Debug.Log(CannonTop.transform.GetChild(1).transform.localEulerAngles.y);
                    if (GetBarrelRotateRange > -30f) {
                        GetBarrelRotateRange--;
                        CannonTop.transform.Rotate(new Vector3(0, -CannonRotateSpeed, 0));
                    }
                    else{
                        GetBarrelRotateRange = -30;
                    }

                }
                if (Input.GetAxis("Vertical") > 0) {
                    if (CannonTop.transform.GetChild(0).transform.rotation.eulerAngles.z < 335 ) {
                        Debug.Log(CannonTop.transform.GetChild(0).transform.rotation.eulerAngles.z);
                        CannonTop.transform.GetChild(0).Rotate(new Vector3(0, 0, BarrelRotateSpeed));
                    }
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (CannonTop.transform.GetChild(0).transform.rotation.eulerAngles.z > 270) {
                        Debug.Log(CannonTop.transform.GetChild(0).transform.rotation.eulerAngles.z);
                        CannonTop.transform.GetChild(0).Rotate(new Vector3(0, 0, -BarrelRotateSpeed));
                    }
                }
            }



            if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button2)) && LoadBearTime <= 0 && AimZonePos.localPosition != Vector3.zero)
            {
                PlayerCon.GetComponent<CapsuleCollider>().isTrigger = false;
                PlayerCon.GetComponent<Rigidbody>().useGravity = true;
                LoadBearTime = DefaultLoadBearTime;
                PlayerCon.IsInvincible = false;
              
                if (PlayerCon.IsFat)
                {
                    Player_Ani.SetTrigger("PlayerFatShot");
                }

                PlayerCon.transform.DOLocalJump( new Vector3(AimZonePos.position.x, 0f, AimZonePos.position.z), 5f, 1, 1f).SetEase(Ease.Linear);
                //PlayerCon.transform.DOLocalJump( AimZonePos.position, 5f, 1, 1f).SetEase(Ease.Linear);


                //PlayerCon.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.right*500f,AimZonePos.position);
                //PlayerCon.transform.DOPunchPosition(Vector3.forward,0.5f);

                PlayerCon.transform.LookAt(AimZonePos);
                PlayerCon.transform.eulerAngles += new Vector3(0, -180, 0);
                audi.PlayOneShot(shootSE);
                IsShot = true;

                AimZonePos.position = new Vector3(0, 0.01f, 0);
            }

        }
        else if(!PlayerCon.IsInCannon){
            CannonTop.transform.GetChild(0).transform.DORotate(new Vector3(0, 0, -90), 0.5f);
            CannonBigWheel.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            CannonLittleWheel.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            CannonTop.transform.DOLocalRotate(Vector3.zero, 0.5f);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {

            BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[3]);

            if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button2)) && !PlayerCon.IsInCannon) {
                PlayerCon.IsInCannon = true;
                PlayerCon.transform.DOJump(CannonTop.transform.GetChild(0).transform.position + new Vector3(0,2f,0), 2f, 1, 0.5f);
                PlayerCon.gameObject.transform.DOScale(0,0.5f);
                PlayerCon.GetComponent<Rigidbody>().velocity = Vector3.zero;
                PlayerCon.GetComponent<CapsuleCollider>().isTrigger = true;
                PlayerCon.GetComponent<Rigidbody>().useGravity = false;

            }//GetInCannon
            /*if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button2)) && PlayerCon.IsInCannon)
            {
                PlayerCon.IsInCannon = false;
            }//OutCannon*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            BB.disAppear(PlayerCon.gameObject);
        }
    }

}
