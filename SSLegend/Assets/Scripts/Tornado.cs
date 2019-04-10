using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public float TornadoTime = 7f;
    public float TornadoEffectTime = 3f;
     float TornadoForce = 650f;
    Animator Player_Ani;

	// Use this for initialization
	void Start () {
        Player_Ani = GameObject.Find("Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Timer();
	}

    void Timer() {
        TornadoTime -= Time.deltaTime;
        TornadoEffectTime -= Time.deltaTime;
        if (TornadoTime <= 0) {
            Destroy(this.gameObject);
        }
        if (TornadoEffectTime <= 0) {
            this.GetComponent<BoxCollider>().enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Player_Ani.SetTrigger("PlayerJump");
            Player_Ani.SetBool("PlayerIsInAir",true);
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            other.GetComponent<Rigidbody>().AddForce(transform.up*TornadoForce);
        }
    }
}
