using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public float TornadoTime = 7f;
    public float TornadoEffectTime = 7f;
    public float FastSpeed;
     float TornadoForce = 650f;
    Animator Player_Ani;
    PlayerControl PlayerCon;

	// Use this for initialization
	void Start () {
        Player_Ani = GameObject.Find("Player").GetComponent<Animator>();
        PlayerCon = FindObjectOfType<PlayerControl>();
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

        if (other.CompareTag("Player") && PlayerCon.IsGround)
        {
            Player_Ani.SetBool("PlayerFastRun", true);
            PlayerCon.IsFast = true;
            PlayerCon.speed = FastSpeed;
        }
        else if (other.CompareTag("Player") && !PlayerCon.IsGround)
        {
            Player_Ani.SetBool("PlayerFastRun", false);
            Player_Ani.SetTrigger("PlayerJump");
            Player_Ani.SetBool("PlayerIsInAir", true);
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            other.GetComponent<Rigidbody>().AddForce(transform.up * TornadoForce);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
