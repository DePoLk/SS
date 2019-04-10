using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonAim : MonoBehaviour {

    public float UpSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(0, 0, -UpSpeed * Time.deltaTime));
	}
}
