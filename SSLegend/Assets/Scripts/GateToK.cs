using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateToK : MonoBehaviour {

    LoadingControl Load;
	// Use this for initialization
	void Start () {
        Load = FindObjectOfType<LoadingControl>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Load.ChangeScene();
        }
    }
}
