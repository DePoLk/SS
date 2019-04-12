using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSE : MonoBehaviour {

    private AudioSource audi;
    public AudioClip hitGround;
    bool onetime = true;

    CameraControl CC;
 	// Use this for initialization
	void Start () {
        audi = GetComponent<AudioSource>();
        CC = FindObjectOfType<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && onetime) {
            onetime = false;
            audi.PlayOneShot(hitGround);
            Debug.Log("G");
            CC.shake();
        }
    }
}
