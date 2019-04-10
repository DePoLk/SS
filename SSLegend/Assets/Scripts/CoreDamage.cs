using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreDamage : MonoBehaviour {
    private Boss_Arubado BA;
	// Use this for initialization
	void Start () {
        BA = FindObjectOfType<Boss_Arubado>();
        Debug.Log(BA.NewHP);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AtkBox" ) {
            BA.NewHP--;
            Debug.Log(BA.NewHP);
        }
    }
}
