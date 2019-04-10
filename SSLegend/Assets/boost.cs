using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour {

    public float speed=5.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.z > 30)
        {
            transform.position = new Vector3(0, 6.0f,0);

        }
        else {
            transform.Translate(new Vector3(0,0, speed * Time.deltaTime));
        }
	}
}
