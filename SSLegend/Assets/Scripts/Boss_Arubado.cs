using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Arubado : MonoBehaviour
{
    private GroundCheck GC;
    private PlayerControl PC;
    WaterValueChange WVC;
    private Cinemachine_Ctrl CC;
    public GameObject MagicAtk;
    public GameObject MagicAtk_Range;
    public Transform FirePos;
    public Transform player;
    public Animator anim;
    public GameObject SmashCol;
    public GameObject core;
    public GameObject Normal_AtkBox;
    public GameObject Slash_AtkBox;
    public GameObject WallTrigger;
    public bool Restart_Pause = false;
    private Vector3 pos;
    private Magic_Atk MA;
    private bool TopAtking = false;
    public int NewHP = 3;
    public bool Down = false;
    private bool SmashAiming = false;
    private bool IsLook = false;
    private bool Atking = false;
    private bool OnGround = true;
    private bool TopCorrection = false;
    private bool StageSmash = false;
    private bool Smash_Force = false;
    private bool resistance = false;
    private bool ReTop = false;
    private bool NormalAim = false;
    private bool IsCloseToWall = false;
    private bool IsDead = false;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        GC = FindObjectOfType<GroundCheck>();
        PC = FindObjectOfType<PlayerControl>();
        MA = FindObjectOfType<Magic_Atk>();
        WVC = FindObjectOfType<WaterValueChange>();
        CC = FindObjectOfType<Cinemachine_Ctrl>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(Down);
        Debug.Log(GC.OnTop);
        //Debug.Log(TC.OnTop);                             
        if (IsLook == false && Atking == false && Down == false && GC.OnTop == false&& Restart_Pause == false)
        {
            StartCoroutine("FaceEnemy");
        }
        if (Vector3.Distance(player.position, this.transform.position) < 30 && IsLook == true && Atking == false && OnGround == true && TopAtking == false && Down == false && Restart_Pause == false)
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            if (direction.magnitude > 4)
            {
                anim.SetBool("Move", true);
                this.transform.Translate(0, 0, 0.05f);
            }
            else if (direction.magnitude <= 4)
            {
                StartCoroutine("NormalAtk");
            }
        }
        if (SmashAiming == true)
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
        //高台 關其他技能,實施砲台掃蕩
        if (GC.OnTop == true)
        {
            WallTrigger.SetActive(false);
            ReTop = true;
            OnGround = false;
            IsLook = false;
            SmashAiming = false;
            StageSmash = false;
            StopCoroutine("FaceEnemy");
            StopCoroutine("SmashAim");
            StopCoroutine("FarAtk");
            StopCoroutine("NormalAtk");
            anim.SetBool("FarAtk", false);
            anim.SetBool("Normal", false);
            anim.SetBool("Smash", false);
            Vector3 TopDistance = player.transform.position - this.transform.position;
            TopDistance.y = 0;
            if (TopDistance.magnitude > 7.5 && Down == false && TopAtking == false)
            {
                Debug.Log(TopDistance.magnitude);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TopDistance), 0.1f);
                anim.SetBool("Move", true);
                this.transform.Translate(0, 0, 0.1f);

            }
            else if (TopDistance.magnitude <= 7.5 && TopAtking == false)
            {
                anim.SetBool("Move", false);
                StartCoroutine("TopAtk");
            }
        }
        else if (GC.OnTop == false && Down == false && ReTop == true)
        {
            //anim.SetBool("Move", false);
            anim.SetBool("TopAtk", false);
            StartCoroutine("ReturnToNormal");
        }
        //RotateToPlayer
        if (TopCorrection == true || NormalAim == true)
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
        //Addforce
        if (Smash_Force == true)
        {
            this.GetComponent<Rigidbody>().AddForce(transform.forward * 12000);
        }
        //ForceResistance
        if (resistance == true)
        {
            this.GetComponent<Rigidbody>().AddForce(transform.forward * 5000);
        }
        if (NewHP <= 0) {
            Dead();
        }
    }
    IEnumerator ReturnToNormal()
    {
        ReTop = false;
        Debug.Log("!");
        OnGround = true;
        TopAtking = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("FaceEnemy");
        yield return new WaitForSeconds(0.5f);
        StopCoroutine("ReturnToNormal");
    }
    //LongRangeAttack
    public void Aim()
    {
        MagicAtk_Range.transform.position = player.position;
        Destroy(Instantiate(MagicAtk_Range, player.position, Quaternion.Euler(90f, 0, 0)), 2.5f);
        StartCoroutine("Fire");
    }
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(MagicAtk, FirePos.position, FirePos.rotation);
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator TopAtk()
    {
        TopAtking = true;
        TopCorrection = true;
        yield return new WaitForSeconds(0.1f);
        TopCorrection = false;
        IsLook = false;
        anim.SetBool("TopAtk", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("TopAtk", false);
        yield return new WaitForSeconds(6f);
        StopCoroutine("TopAtk");
        TopAtking = false;
    }
    //SmashAttack
    IEnumerator SmashAim()
    {
        WallTrigger.SetActive(true);
        anim.SetBool("Move", false);
        IsLook = false;
        Atking = true;
        SmashAiming = true;
        anim.SetBool("Smash", true);
        yield return new WaitForSeconds(4f);
        SmashAiming = false;
        yield return new WaitForSeconds(2f);
        WallTrigger.SetActive(false);
        anim.SetBool("Smash", false);
        Atking = false;
        yield return new WaitForSeconds(1f);
        StopCoroutine("SmashAim");
    }
    IEnumerator FarAtk()
    {
        anim.SetBool("Move", false);
        IsLook = false;
        Atking = true;
        anim.SetBool("FarAtk", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("FarAtk", false);
        yield return new WaitForSeconds(6f);
        Atking = false;
        StopCoroutine("FarAtk");
    }
    IEnumerator FaceEnemy()
    {
        resistance = false;
        Smash_Force = false;
        IsLook = true;
        int a = Random.Range(1, 3);
        Debug.Log(a);
        yield return new WaitForSeconds(5f);
        if (a == 1 && Down == false)
        {
            StartCoroutine("SmashAim");
            StopCoroutine("FaceEnemy");
        }
        else if (a == 2 && Down == false)
        {
            StartCoroutine("FarAtk");
            StopCoroutine("FaceEnemy");
        }
    }
    IEnumerator NormalAtk()
    {
        NormalAim = true;
        yield return new WaitForSeconds(0.5f);
        NormalAim = false;
        Atking = true;
        anim.SetBool("Move", false);
        anim.SetBool("Normal", true);
        StopCoroutine("FaceEnemy");
        yield return new WaitForSeconds(1f);
        anim.SetBool("Normal", false);
        yield return new WaitForSeconds(3f);
        Atking = false;
        IsLook = false;
        StopCoroutine("NormalAtk");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AirAtk" && Down == false)
        {
            Down = true;
            StartCoroutine("Normal_LayDown");
            WVC.ImgWaterValue = 0;
            PC.gameObject.GetComponent<AnimationFunction>().FatAirAtkBox.SetActive(false);
        }                                    //&& StageSmash == true && Down == false
        if (other.gameObject.tag == "obstacle" && Down == false)
        {
            WallTrigger.SetActive(false);
            Down = true;
            StartCoroutine("LayDown");
            //this.GetComponentInParent<Rigidbody>().AddForce(transform.forward * -1500);
        }
    }
    public void NA_Strength()
    {
    }
    IEnumerator LayDown()
    {
        SmashCol.GetComponent<BoxCollider>().enabled = false;
        Atking = false;
        resistance = false;
        Smash_Force = false;
        anim.SetBool("Move", false);
        anim.SetTrigger("Charge_Down");
        core.GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(8f);
        core.GetComponent<SphereCollider>().enabled = false;
        anim.SetTrigger("Rewake");
        yield return new WaitForSeconds(3f);
        Down = false;
    }
    IEnumerator Normal_LayDown()//Bear
    {
        anim.SetBool("Move", false);
        anim.SetTrigger("Normal_Down");
        core.GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(8f);
        core.GetComponent<SphereCollider>().enabled = false;
        anim.SetTrigger("Rewake");
        yield return new WaitForSeconds(3f);
        Down = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine("ReturnToNormal");
    }
    public void NorAtkBox()
    {
        Normal_AtkBox.GetComponent<BoxCollider>().enabled = true;
    }
    public void CloseNorAtkBox()
    {
        Normal_AtkBox.GetComponent<BoxCollider>().enabled = false;
    }
    public void SlashAtkBox()
    {
        Slash_AtkBox.GetComponent<BoxCollider>().enabled = true;
    }
    public void CloseSlashAtkBox()
    {
        Slash_AtkBox.GetComponent<BoxCollider>().enabled = false;
    }
    public void Open_SmachForce()
    {
        Smash_Force = true;
    }
    public void Close_SmashForce()
    {
        Smash_Force = false;
    }
    public void Open_Resistance()
    {
        resistance = true;
    }
    public void close_Resistance()
    {
        resistance = false;
    }
    public void OpenSmashAtkBox()
    {
        SmashCol.GetComponent<BoxCollider>().enabled = true;
    }
    public void CloseSmashAtkBox()
    {
        SmashCol.GetComponent<BoxCollider>().enabled = false;
    }
    public void Dead() {
        StopAllCoroutines();
        CC.EndingStart = true;
        this.gameObject.SetActive(false);
    }
}

