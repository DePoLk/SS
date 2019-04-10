using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBearGroundCheck : MonoBehaviour
{

    BearConForTest Player;
    GameObject PlayerGO;
    
    // Use this for initialization
    void Start()
    {
        Player = FindObjectOfType<BearConForTest>();
        PlayerGO = GameObject.Find("TestBear");
       
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Player.IsGround = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Box"))
        {
            Player.IsGround = true;
        }
        if (other.CompareTag("Slope"))
        {
            Player.IsGround = true;
          
                PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Box"))
        {
            Player.IsGround = false;

        }
        if (other.CompareTag("Slope"))
        {
            PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            PlayerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;



            Player.IsGround = false;
        }
    }
}

