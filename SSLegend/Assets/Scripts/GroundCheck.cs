using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    PlayerControl PlayerCon;
    GameObject PlayerGO;
    PlayerSE playerSE;
    public GameObject jumping_Dust;
    public GameObject jumpDustspot;
    public bool OnTop = false;
    Vector3 TempPos;
    Vector3 InForce;

    

    // Use this for initialization
    void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();
        PlayerGO = GameObject.Find("Player");
        playerSE = FindObjectOfType<PlayerSE>();
    }

    private void Update()
    {
        if (PlayerCon.IsGround)
        {
            PlayerGO.GetComponent<Animator>().SetBool("PlayerIsGround", true);
            PlayerGO.GetComponent<Animator>().SetBool("PlayerIsInAir", false);

        }
        else {
            PlayerGO.GetComponent<Animator>().SetBool("PlayerIsGround", false);
            PlayerGO.GetComponent<Animator>().SetBool("PlayerIsInAir", true);

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("FixColli"))
        {
            PlayerCon.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            TempPos = PlayerCon.transform.position;
            PlayerCon.transform.position = TempPos + new Vector3(0,0.8f,0);
            /*InForce = PlayerCon.gameObject.GetComponent<Rigidbody>().velocity;
            PlayerCon.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            PlayerCon.gameObject.GetComponent<Rigidbody>().velocity = -InForce * 50f;*/
        }

        if (other.CompareTag("Ground") || other.CompareTag("Slope") || other.CompareTag("Box") || other.CompareTag("Bottle"))
        {
            GameObject a = Instantiate(jumping_Dust,jumpDustspot.transform);
            a.transform.SetParent(null);
            playerSE.Landing_SE();
            Destroy(a, 3.0f);

            PlayerGO.GetComponent<Animator>().SetBool("PlayerJump", false);
            PlayerCon.IsJumping = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("canon")) {
            OnTop = true;
        }

        if (other.CompareTag("Ground") || other.CompareTag("Box") || other.CompareTag("Bottle")) {
            PlayerCon.IsGround = true;
            PlayerCon.JumpSpeed = 250f;
            PlayerCon.DefaultJumpSpeed = 250f;
        }
        if (other.CompareTag("Slope")) {
            PlayerCon.IsGround = true;
            PlayerCon.JumpSpeed = 350f;
            PlayerCon.DefaultJumpSpeed = 350f;
            if (PlayerCon.IsAtking || PlayerCon.IsDodging)
            {
                PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
               
            }
            /*else
            {
                PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                //PlayerGO.GetComponent<Rigidbody>().useGravity = false;
            }*/
            else if (PlayerCon.input_H == 0 && PlayerCon.input_V == 0)
             {
                PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                PlayerGO.GetComponent<Rigidbody>().drag = 0f;
            }
            else if (PlayerCon.input_H != 0 && PlayerCon.input_V != 0) {
                PlayerGO.GetComponent<Rigidbody>().drag = 5f;
             }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canon"))
        {
            OnTop = false;
        }

        

        if (other.CompareTag("Ground") || other.CompareTag("Box") || other.CompareTag("Bottle")) {
            PlayerCon.IsGround = false;
            if (PlayerCon.IsDodging) {
                PlayerCon.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        if (other.CompareTag("Slope"))
        {
            PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
           // PlayerGO.GetComponent<Rigidbody>().useGravity = true;
            PlayerGO.GetComponent<Rigidbody>().drag = 0f;
            PlayerCon.IsGround = false;
        }
    }
}
