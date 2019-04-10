using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAround : MonoBehaviour {
    public GameObject Target;
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (this.transform.position.z > 30) {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
	}
}
