using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : MonoBehaviour
{
    private float movespeed = -1f;
    public float rotatespeed = 80f;
    public float time_set;
    private Transform player;
    public GameObject Detector;
    public Animator anim;
    public GameObject Wolf;
    public AudioClip hitSE;
    public AudioClip DeathSE;

    //特效// 
    public Material trans_mat;
    public Material Knife_transMat;
    public GameObject mat_render;
    public GameObject deadMon;
    public Transform particleSpot;
    public GameObject SwordLight;
    public GameObject Swordspot;
    public GameObject sword;
    public float forcr;
    public ParticleSystem runningDust;

    private Rigidbody m_Rigidbody;
    private CapsuleCollider Cap;
    private float CDTimer = 0f;
    public int HP = 3;
    private bool Relax = false;
    private bool Iswandering = false;
    private bool IsRotateRight = false;
    private bool IsRotateLeft = false;
    private bool Iswalking = false;
    public bool cooldown = false;
    private bool bikulishida = false;
    private bool TOF;
    private bool IsAlive = true;
    private bool resetmode = true;
    private bool First_attack = true;
    private bool isnear = false;
    private bool Face = true;
    private int RoL;
    private CameraControl _Cctrl;
    private PlayerControl _Pctrl;
    private AudioSource audi;
    private ExcalamationMark EXM;
    private BoxCollider AtkBox;
    // Use this for initialization
    void Start()
    {
        // anim = GetComponent<Animator>();
        Cap = GetComponent<CapsuleCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        _Cctrl = FindObjectOfType<CameraControl>();
        _Pctrl = FindObjectOfType<PlayerControl>();
        audi = GetComponentInChildren<AudioSource>();
        EXM = GetComponentInChildren<ExcalamationMark>();
        AtkBox = GetComponentInChildren<BoxCollider>();
        StartCoroutine("wander");
        time_set = Time.time;
        player = GameObject.FindWithTag("Player").transform;
        particleSpot.position = this.transform.position + new Vector3(0, 1.4f, 0);
        Swordspot = GameObject.Find("End");
        GameObject dm = Instantiate(deadMon, particleSpot.transform.position, particleSpot.transform.rotation);
        Destroy(dm, 1.0f);
    }
    // Update is called once per frame
    void FixedUpdate()
    //判定旋轉

    {   //怪物隨機移動
        if (Iswandering == false)
        {
            StartCoroutine("wander");
        }
        //追擊怪物判定

        if (Vector3.Distance(player.position, this.transform.position) < 4 && IsAlive == true && Relax==false)
        {
            if (bikulishida == false)
            {
                EXM.startMark = true;
                bikulishida = true;
            }
            var em = runningDust.emission;
            StopCoroutine("wander");
            anim.SetBool("detect", false);
            resetmode = true;
            Iswalking = false;
            IsRotateRight = false;
            IsRotateLeft = false;

            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            if (Face == true)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            }
            if (direction.magnitude > 2 && Time.time - time_set > 2.5f)
            {
                anim.SetBool("walk", false);
                isnear = false;
                First_attack = true;
                anim.SetBool("run", true);
                anim.SetBool("ExMark", true);
                anim.SetBool("attack1", false);
                anim.SetBool("attack2", false);
                this.transform.Translate(0, 0, 0.05f);
                em.enabled = true;
                StopCoroutine("rotatewalk");
                cooldown = false;
            }
            else if (direction.magnitude <= 2 && cooldown == false)
            {
                isnear = true;
                em.enabled = false;
                if (First_attack == true)
                {
                    anim.SetBool("attack1", true);
                    First_attack = false;
                }
                else if (First_attack == false && isnear == true)
                {
                    CDTimer += Time.deltaTime;
                    if (CDTimer > 2)
                    {
                        anim.SetBool("attack2", true);
                    }
                }
                anim.SetBool("run", false);
                anim.SetBool("ExMark", false);
                anim.SetInteger("ExTime", 2);
                time_set = Time.time;
            }
            //Debug.Log(direction.magnitude);
        }
        else
        {
            bikulishida = false;
            anim.SetBool("attack1", false);
            anim.SetBool("run", false);
            if (resetmode == true)
            {
                ResetAllState();
            }
        }
        if (TOF == true)
        {
            if (RoL == 1)
            {
                this.transform.Rotate(0, 6f, 0);
            }
            else if (RoL == 2)
            {
                this.transform.Rotate(0, -6f, 0);
            }
        }
        else if (TOF == false)
        {
        }
        if (IsRotateLeft == true)
        {
            transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
            anim.SetBool("walk", true);
        }
        if (IsRotateRight == true)
        {
            transform.Rotate(0, -rotatespeed * Time.deltaTime, 0);
            anim.SetBool("walk", true);
        }
        if (Iswalking == true && IsAlive == true)
        {
            transform.position += transform.forward * -movespeed * Time.deltaTime;
            anim.SetBool("walk", true);
        }
    }

    public void DetectObstacle(bool result, int a)
    {
        TOF = result;
        RoL = a;
        //Debug.Log(RoL);
    }
    IEnumerator wander()
    {
        resetmode = false;
        int rotateTime = Random.Range(1, 2);
        int rotateWait = Random.Range(1, 6);
        int rotateLorR = Random.Range(1, 3);
        int walkTime = Random.Range(2, 5);
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
            anim.SetBool("detect", true);
        }
        yield return new WaitForSeconds(rotateWait);
        anim.SetBool("detect", false);
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
    }
    private void OnTriggerEnter(Collider weapon)
    {
        if (weapon.gameObject.tag == "AtkBox" && HP >= 1)
        {
            audi.PlayOneShot(hitSE);
            Debug.Log(HP);
            Vector3 temp = weapon.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            HP -= 1;
            anim.SetTrigger("hurt");
            anim.SetBool("attack", false);
            GameObject sl = Instantiate(SwordLight, Swordspot.transform);
            sl.transform.SetParent(null);
            Destroy(sl, 1.0f);
            TimeSlowDown();
            StartCoroutine("Flash");
        }
        else if (weapon.gameObject.tag == "AtkBox" && HP == 0)
        {
            Cap.enabled = false;
            m_Rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            audi.PlayOneShot(DeathSE);
            Vector3 temp = weapon.gameObject.transform.position;
            Vector3 dis = temp - this.gameObject.transform.position;
            dis.x = -(dis.x);
            dis.z = -(dis.z);
            GetComponent<Rigidbody>().AddForce(new Vector3(dis.x * forcr, 0, dis.z * forcr));
            HP -= 1;
            Debug.Log(HP);
            anim.SetTrigger("Dead");
            mat_render.GetComponent<SkinnedMeshRenderer>().material = trans_mat;
            sword.GetComponent<MeshRenderer>().material = Knife_transMat;
            IsAlive = false;
            TimeSlowDown_Final();
            Detector.GetComponent<SphereCollider>().enabled = false;
            GameObject dm = Instantiate(deadMon, particleSpot.transform.position, particleSpot.transform.rotation);
            Destroy(dm, 1.0f);
            Destroy(Wolf, 3.0f);
        }
        else if (HP < 0)
        {
            IsAlive = false;
        }
    }
    private IEnumerator Flash()
    {
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        sword.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        yield return new WaitForSeconds(0.075f);
        mat_render.GetComponent<SkinnedMeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
        sword.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/PlayerDiffuse");
    }
    public void TimeSlowDown()
    {
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
    public void ResetAllState()
    {
        StartCoroutine("wander");
    }
    public void ResetAtk1()
    {
        anim.SetBool("attack1", false);
    }

    public void resetatk2()
    {
        anim.SetBool("attack2", false);
        CDTimer = 0f;
        Debug.Log(CDTimer);
    }
    public IEnumerator Atk()
    {
        AtkBox.enabled = true;
        yield return new WaitForSeconds(0.1f);
        AtkBox.enabled = false;
        StopCoroutine("Atk");
    }
    public void JumpForce()
    {
        this.GetComponentInParent<Rigidbody>().AddForce(transform.up * 500);
        this.GetComponentInParent<Rigidbody>().AddForce(transform.forward*300);
    }
    public IEnumerator AfterAtk() {
        Relax = true;
        yield return new WaitForSeconds(3);
        Relax = false;
    }
}