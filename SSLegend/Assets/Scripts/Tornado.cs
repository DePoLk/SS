using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tornado : MonoBehaviour {

    public float TornadoTime = 7f;
    public float TornadoEffectTime = 7f;
    public float FastSpeed;
    float TornadoForce = 650f;
    Animator Player_Ani;
    PlayerControl PlayerCon;
    private GameObject DodgeDetector;
    Scene ThisScene;


	// Use this for initialization
	void Start () {
        Player_Ani = GameObject.Find("Player").GetComponent<Animator>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        DodgeDetector = GameObject.Find("DodgeDetector");
        ThisScene = SceneManager.GetActiveScene();


        if (ThisScene.name.Equals("2-1")) {
            FastSpeed = -4;
        }
        if (ThisScene.name.Equals("K"))
        {
            FastSpeed = -6;
        }

    }
	
	// Update is called once per frame
	void Update () {
        Timer();
        transform.Translate(DodgeDetector.transform.forward * -1 * Time.deltaTime * 0.2f);
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

        /* if (other.CompareTag("Player") && PlayerCon.IsGround)
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
         }*/

        if (other.CompareTag("Player")) {
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
