using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveScript : MonoBehaviour {

    private float v;
    private float h;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        v = Input.GetAxis("Horizontal");
        h = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(v * Time.deltaTime * 10f, 0, 0));
        transform.Translate(new Vector3(0, 0, h * Time.deltaTime * 10f));


    }
}
