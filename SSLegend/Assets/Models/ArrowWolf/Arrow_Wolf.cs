using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Wolf : MonoBehaviour {
    private bool IsDeadFade = false;
    private bool IsAlive = true;
    private bool Aim = true;
    private bool Retreat = false;
    private bool idle = true;
    private bool ArrivePlace2 = false;
    private bool IsShooting = false;
    private int HP = 3;
    private ArrowWolf_Anim AA;
    private Vector3 RotationRecoder;
    private float a;
    private float b;
    private float i = 1;
    private AudioSource audi;
    public Material trans_mat;
    public AudioClip DeathSE;
    public AudioClip hitSE;
    public float forcr;
    public GameObject mat_render;
    public Transform T;
    public GameObject Arrow;
    public Transform player;
    //public Transform RetreatPos;
    public GameObject deadMon;
    public Transform particleSpot;
    public GameObject Wolf;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        AA = GetComponentInChildren<ArrowWolf_Anim>();
        audi = GetComponent<AudioSource>();
        GameObject dm = Instantiate(deadMon, particleSpot.transform.position, particleSpot.transform.rotation);
    }
	// Update is called once per frame
	void FixedUpdate () {
        // if(transform.rotation.eulerAngles.y-RotationRecoder.y)                    ↓&& Retreat==false
        if (Vector3.Distance(player.transform.position, this.transform.position) < 20  && IsAlive==true)
        {

            Vector3 direction = player.transform.position - this.transform.position;
            direction.y = 0;
            //Debug.Log(direction.magnitude);
            if (direction.magnitude > 10) {
                if (idle == true)
                {
                    StartCoroutine("Wolf_Idle");
                }
                AA.anim.SetBool("Rotatewalk", false);
                AA.anim.SetBool("attack", false);
                StopCoroutine("Shoot");
                AA.anim.SetBool("WalkAim", false);
                IsShooting = false;
            }
                  //↓direction.magnitude > 5 &&
            else if (direction.magnitude <= 10)
            {
                AA.anim.SetBool("search", false);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                if (IsShooting == false) {
                    StartCoroutine("Shoot");
                }
            }
            /* else if (direction.magnitude <= 5 && ArrivePlace2 == false)
            {
                idle = false;
                AA.anim.SetBool("WalkAim", false);
                AA.anim.SetBool("search", false);
                AA.anim.SetBool("attack", false);
                StopCoroutine("Wolf_idle");
                StopCoroutine("Shoot");
               // StartCoroutine("retreat");
            }*/
            //Debug.Log(direction.magnitude);
        }
        //Retreat
        /*if (Retreat == true) {
            Vector3 direction = RetreatPos.position - this.transform.position;
            if (direction.magnitude > 10)
            {
                AA.anim.SetBool("Rotatewalk", true);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                this.transform.Translate(0, 0, 0.1f);
            }
        }*/
        if (IsDeadFade == true) {
            if (i >= 0)
            {
                i -= 0.05f;
            }
            else if (i <= 0) {
                Invoke("closeWolf", 1);
            }
            trans_mat.color = new Color(1, 1, 1, i);
            AA.closeshadows();
        }
    }
    private void closeWolf() {
        Wolf.SetActive(false);
        Destroy(Wolf, 0.5f);
    }
    IEnumerator Wolf_Idle() {
        idle = false;
        int Isidle = Random.Range(1,3);
        if (Isidle==2) {
            AA.anim.SetBool("search",true);
        }
        yield return new WaitForSeconds(4.0f);
        AA.anim.SetBool("search", false);
        idle = true;
        StopCoroutine("Wolf_Idle");
    }
    IEnumerator Shoot() {
        IsShooting = true;
        AA.anim.SetBool("WalkAim",true);
        yield return new WaitForSeconds(2.0f);
        AA.anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        AA.anim.SetBool("attack", false);
        yield return new WaitForSeconds(2.0f);
        IsShooting = false;
        StopCoroutine("Shoot");
    }
   /* IEnumerator retreat() {
        Retreat = true;
        AA.anim.SetBool("Rotatewalk", true);
        yield return new WaitForSeconds(4.0f);
        Retreat = false;
        AA.anim.SetBool("Rotatewalk", false);
        ArrivePlace2 = true;
        idle = true;
        IsShooting = false;
        StopCoroutine("retreat");
    }*/
    public void ArrowShoot() {
        Destroy(Instantiate(Arrow, T.position, T.rotation), 3);
    }
    private void OnTriggerEnter(Collider weapon)
    {
        if (weapon.gameObject.tag == "AtkBox" && HP == 3){
            audi.PlayOneShot(hitSE);
            Vector3 temp = weapon.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            HP--;
            AA.anim.SetTrigger("Strike");
            StartCoroutine("Flash");
        }
        else if (weapon.gameObject.tag == "AtkBox" && HP == 2)
        {
            audi.PlayOneShot(hitSE);
            Vector3 temp = weapon.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            HP--;
            AA.anim.SetTrigger("Strike");
            StartCoroutine("Flash");
        }
        else if (weapon.gameObject.tag == "AtkBox" && HP == 1)
        {
            Vector3 temp = weapon.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            audi.PlayOneShot(DeathSE);
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            TimeSlowDown_Final();
            this.GetComponent<BoxCollider>().enabled = false;
            GameObject dm = Instantiate(deadMon, particleSpot.transform.position, particleSpot.transform.rotation);
            mat_render.GetComponent<SkinnedMeshRenderer>().material = trans_mat;
            IsAlive = false;
            IsDeadFade = true;
            AA.anim.SetTrigger("Dead");
        }
    }
    private IEnumerator Flash()
    {
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        //sword.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        yield return new WaitForSeconds(0.075f);
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
        //sword.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
    }
    public void TimeSlowDown_Final()
    {
        Time.timeScale = 0.1f;
        Invoke("TimeBoostUp", 0.03f);
    }
    public void TimeBoostUp()
    {
        Time.timeScale = 1.0f;//回復世界時間
    }
}
