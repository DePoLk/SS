using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karohth : MonoBehaviour
{
    public float movespeed = 1f;
    public float rotatespeed = 80f;
    public Animator anim;
    public Animator Canvas_anim;
    public Transform player;
    public int HP = 3;
    public GameObject cutton;
    public int DetectDistance = 2;
    BillBoard BB;
    //public Sprite Mark;
    private AudioSource audi;
    public AudioClip yell;
    public AudioClip deadSE;
    public AudioClip hitSE;
    private bool Iswandering = false;
    private bool IsRotateRight = false;
    private bool IsRotateLeft = false;
    private bool Iswalking = false;
    private bool Detect = true;
    private bool Rewander = false;
    private bool Escape = false;
    private bool IsDead = false;
    private bool bikulishita = false;
    private bool Exk;
    private bool Run = false;

    //特效// 
    public Material trans_mat;
    public GameObject mat_render;
    public GameObject deadMon;
    public GameObject particleSpot;
    public GameObject SwordLight;
    public GameObject Swordspot;
    public GameObject MarkSpot;
    public GameObject Exk_k;
    //public GameObject MarkSpot;
    //public Sprite Exmark;

    //===//


    [SerializeField]
    private bool startDead = false;
    private float tmp_time;
    public float forcr;
    // Use this for initialization
    void Start()
    {
        BB = FindObjectOfType<BillBoard>();
        audi = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startDead)
        {

        }
        //站起來
        if (Vector3.Distance(player.position, this.transform.position) <= DetectDistance && Detect == true && IsDead == false)
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            StopCoroutine("wander");
            Iswandering = true;
            Iswalking = false;
            IsRotateLeft = false;
            IsRotateRight = false;
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(-direction), 0.05f);
            anim.SetBool("Find", true);
            //MarkSpot.GetComponent<SpriteRenderer>().sprite = Exmark;
            Rewander = true;
            if (bikulishita == false)
            {
                GameObject a = Instantiate(Exk_k, MarkSpot.transform);
                Exk = a.GetComponent<ExcalamationMark_K>().startMark_K;
                Exk = true;
                a.GetComponent<ExcalamationMark_K>().startMark_K = Exk;
                bikulishita = true;
                //Debug.Log(bikulishita);
            }
            StartCoroutine("EscapeMode");

        }
        //else if (Vector3.Distance(player.position, this.transform.position) > 10 && Rewander == true)
        //{
        //    anim.SetBool("Find", false);
        //    MarkSpot.GetComponent<SpriteRenderer>().sprite = null;
        //    StartCoroutine("wander");
        //    Rewander = false ;
        //}
        //Escape
       if (Escape == true && Run == false)
        {
            //BB.disAppear(gameObject);
            //MarkSpot.GetComponent<SpriteRenderer>().sprite = null;
            anim.SetBool("Find", false);
           // anim.SetBool("walk", true);
            Vector3 direction = player.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            //transform.position += transform.forward * 2 * -movespeed * Time.deltaTime;
        }
        if (IsDead == true) {
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);
            anim.SetBool("down", true);
        }
        // Debug.Log(Iswandering);
        if (Run == true && IsDead == false)
        {
            anim.SetBool("Find", false);
            anim.SetBool("walk", true);
            Detect = false;
            transform.position += transform.forward * 3 * -movespeed * Time.deltaTime;
        }
        if (Iswandering == false && Escape == false)
        {
            StartCoroutine("wander");
        }
        if (IsRotateLeft == true)
        {
            transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
            anim.SetBool("walk", true);
            //Debug.Log ("!!!");
        }
        if (IsRotateRight == true)
        {
            transform.Rotate(0, -rotatespeed * Time.deltaTime, 0);
            anim.SetBool("walk", true);
            //Debug.Log ("!!!!");
        }
        if (Iswalking == true && IsDead==false)
        {
            transform.position += transform.forward * -movespeed * Time.deltaTime;
            anim.SetBool("walk", true);
        }
        if (HP == 0) { StopCoroutine(wander()); anim.SetBool("Find", false); }
    }
    IEnumerator wander()
    {

        int rotateTime = Random.Range(1, 2);
        int rotateWait = Random.Range(1, 5);
        int rotateLorR = Random.Range(1, 3);
        int walkTime = Random.Range(1, 4);
        int walkWait = Random.Range(1, 5);

        Iswandering = true;
        yield return new WaitForSeconds(walkWait);
        Iswalking = true;
        yield return new WaitForSeconds(walkTime);
        Iswalking = false;
        anim.SetBool("walk", false);
        //Debug.Log(rotateWait);
        if (rotateWait >= 4)
        {
            anim.SetBool("idle", true);
        }
        yield return new WaitForSeconds(rotateWait);
        anim.SetBool("idle", false);
        if (rotateLorR == 1)
        {
            IsRotateRight = true;
            yield return new WaitForSeconds(rotateTime);
            IsRotateRight = false;
            anim.SetBool("walk", false);
        }
        if (rotateLorR == 2)
        {
            IsRotateLeft = true;
            yield return new WaitForSeconds(rotateTime);
            IsRotateLeft = false;
            anim.SetBool("walk", false);
        }

        Iswandering = false;
        Rewander = false;
    }
    void OnTriggerEnter(Collider w)
    {

        if (w.gameObject.tag == "AtkBox" && HP > 1)
        {
            audi.PlayOneShot(hitSE);
            Vector3 temp = w.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr / 2, 0, dis.z * forcr / 2));
            HP--;
            anim.SetTrigger("hurt_T");
            GameObject sl = Instantiate(SwordLight, Swordspot.transform);
            sl.transform.SetParent(null);
            Destroy(sl, 1.0f);
            TimeSlowDown();
            StartCoroutine("Flash");

        }
        else if (w.gameObject.tag == "AtkBox" && HP == 1)
        {
            audi.PlayOneShot(hitSE);
            IsDead = true;
            Vector3 temp = w.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            StopCoroutine("wander");
            Iswalking = false;
            IsRotateLeft = false;
            IsRotateRight = false;
            StopCoroutine("EscapeMode");
            Escape = false;
            Detect = false;
            Debug.Log(Escape);
            Debug.Log(Detect);
            mat_render.GetComponent<SkinnedMeshRenderer>().material = trans_mat;
            startDead = true;
            //cutton.SetActive(false);

            GameObject sl = Instantiate(SwordLight, Swordspot.transform);
            sl.transform.SetParent(null);
            Destroy(sl, 1.0f);
            // this.GetComponent<BoxCollider>().enabled = false;
            GameObject dm = Instantiate(deadMon, particleSpot.transform.position, particleSpot.transform.rotation);
            Destroy(dm, 1.0f);
            HP--;
            TimeSlowDown_Final();
            Destroy(cutton);
        }
        if (HP <= 0) { }

    }
    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "wepon") {
            TimeBoostUp();
        }
    }*/
    public void Reset()
    {
        anim.SetBool("hurt", false);

    }
    public void destoryer()
    {
        Destroy(this.gameObject);
    }

    public void TimeSlowDown()
    {
        //Time.timeScale = 0.25f;//世界時間1秒變成4秒
        //Invoke("TimeBoostUp", 0.2f);//變慢後的0.8秒回復世界時間
        Time.timeScale = 0.1f;
        Invoke("TimeBoostUp", 0.015f);
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

    private IEnumerator Flash()
    {
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        cutton.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        yield return new WaitForSeconds(0.075f);
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
        cutton.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
        /*yield return new WaitForSeconds(0.075f);
         mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
         cutton.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
         yield return new WaitForSeconds(0.075f);
         mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
         cutton.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");*/
    }
    private IEnumerator EscapeMode()
    {
        yield return new WaitForSeconds(0.8f);
        Escape = true;
        yield return new WaitForSeconds(0.1f);
        Run = true;
        yield return new WaitForSeconds(5f);
        ResetMode();
    }
    private void ResetMode()
    {
        StopCoroutine("EscapeMode");
        anim.SetBool("walk", false);
        Run = false;
        Escape = false;
        StartCoroutine("wander");
        Invoke("ReDetect", 3);
    }
    private void ReDetect()
    {
        Detect = true;
    }

    public void Play_SE_K()
    {
        audi.PlayOneShot(yell);
    }
    public void Play_SE_KDead()
    {
        audi.PlayOneShot(deadSE);
    }
    /*public void warning() {
        Canvas_anim.SetBool("warning",true);
    }*/

}