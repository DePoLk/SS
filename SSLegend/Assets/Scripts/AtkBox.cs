using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBox : MonoBehaviour {

    
    PlayerSE PSE;
	// Use this for initialization
	void Start () {
        PSE = FindObjectOfType<PlayerSE>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Ground")) {
            PSE.HitGround_SE();
        } else if (other.CompareTag("Plants")){

            PSE.HitPlants_SE();
        }
    
    }
}
