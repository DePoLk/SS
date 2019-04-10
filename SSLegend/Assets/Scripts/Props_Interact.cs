using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props_Interact : MonoBehaviour {

    PlayerControl PlayerCon;
    Animator PlayerAni;
    bool HoldBox = false;
    BillBoard BB;
    public Sprite[] BoxTip;
    public float PushForce = -8;
    public float PullForce = 11;

    [Header("Debug")]
    public Vector3 MoveForce;
    float MaxValue = 100f;
    
    GameObject PlayerObj;
	// Use this for initialization
	void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();
        PlayerObj = GameObject.Find("Player");
        PlayerAni = PlayerObj.GetComponent<Animator>();
        BB = FindObjectOfType<BillBoard>();
	}
	
	// Update is called once per frame
	void Update () {
        PushAndPull();

        MoveForce = this.GetComponent<Rigidbody>().velocity;
        Vector3 v = this.GetComponent<Rigidbody>().velocity;
        if (this.GetComponent<Rigidbody>().velocity.x > MaxValue) {
            v.x = MaxValue;           
        }
        if (this.GetComponent<Rigidbody>().velocity.z > MaxValue)
        {
            v.z = MaxValue;
        }
        if (this.GetComponent<Rigidbody>().velocity.x < -MaxValue)
        {
            v.x = -MaxValue;
        }
        if (this.GetComponent<Rigidbody>().velocity.z < -MaxValue)
        {
            v.z = -MaxValue;
        }
        this.GetComponent<Rigidbody>().velocity =v;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            PlayerCon.NearItem = true;
            BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[1]);//拖動
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))) {
            PlayerCon.NearItem = true;        
            if (!HoldBox && !PlayerCon.IsFat)
            {
                
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                PlayerObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                HoldBox = true;
                PlayerCon.IsHolding = true;
                BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[2]);//放下             
            }
            else {
                PlayerObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                HoldBox = false;
                PlayerCon.IsHolding = false;
                PlayerAni.SetBool("PlayerPush", false);
                PlayerAni.SetBool("PlayerPull", false);
                BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[1]);
            }    
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BB.disAppear(PlayerCon.gameObject);
            PlayerCon.NearItem = false;
            PlayerCon.IsHolding = false;
            HoldBox = false;
            PlayerAni.SetBool("PlayerPush", false);
            PlayerAni.SetBool("PlayerPull", false);
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }  


    }



    void PushAndPull() {
        if (HoldBox && Input.GetAxisRaw("Vertical") > 0)
        {
            PlayerAni.SetBool("PlayerPush", true);
            this.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.forward* PushForce);
            this.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.right * PushForce);
            PlayerObj.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.forward * PushForce);
            PlayerObj.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.right * PushForce);
        }
        if (HoldBox && Input.GetAxisRaw("Vertical") < 0)
        {
            PlayerAni.SetBool("PlayerPull", true);
            this.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.forward * PullForce);
            this.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.right * PullForce);
            PlayerObj.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.forward * (PullForce - 6));
            PlayerObj.GetComponent<Rigidbody>().AddForce(PlayerObj.transform.right * (PullForce - 6));
        }
        if (HoldBox && Input.GetAxisRaw("Vertical") == 0) {
            PlayerAni.SetBool("PlayerPush", false);
            PlayerAni.SetBool("PlayerPull", false);
            this.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,0);
            PlayerObj.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }

    }
}
