using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_wolf : MonoBehaviour {
    public GameObject Wolf_Model;
    public Transform createspot;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
	}
    public void CreateWolf() {

        GameObject wolf = Instantiate(Wolf_Model, createspot.transform.position, createspot.transform.rotation);
    }
  
    
}
