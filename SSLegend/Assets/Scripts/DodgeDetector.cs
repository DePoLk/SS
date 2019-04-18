using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeDetector : MonoBehaviour {

    PlayerControl PlayerCon;


	// Use this for initialization
	void Start () {
        PlayerCon = FindObjectOfType<PlayerControl>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wolf"))
        {
            if (PlayerCon.IsDodging)
            {
                PlayerCon.gameObject.GetComponent<Rigidbody>().useGravity = false;
                PlayerCon.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                PlayerCon.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                PlayerCon.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            else
            {
                PlayerCon.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                PlayerCon.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                PlayerCon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                PlayerCon.gameObject.GetComponent<CapsuleCollider>().enabled = true;
            }
        }

    }

}
