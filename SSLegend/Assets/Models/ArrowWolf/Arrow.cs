using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    Rigidbody RB;
    private bool Istouch = false;
    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody>();
        this.GetComponentInParent<Rigidbody>().AddForce(transform.forward * 200);
    }

	// Update is called once per frame
	void Update () {
        if (Istouch == false)
        {
            transform.rotation = Quaternion.LookRotation(RB.velocity);
        }
        else this.GetComponentInParent<Rigidbody>().drag = 100;
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "player" || col.gameObject.name == "Plane") {
            Istouch = true;
        }
    }
}
