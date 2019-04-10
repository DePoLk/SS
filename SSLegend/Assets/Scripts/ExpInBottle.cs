using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class ExpInBottle : MonoBehaviour
{

    PlayerControl PlayerCon;
    PlayerSE SE;
    float smoothSpeed = 0.065f;//0.055f
    float GetDis = 10f;
    float radian = 0;
    float perRadian = 0.03f;
    float radius = 0.5f;
    float WaitTime = 2f;//1.415f
    float WaitPopTime = 0.1f;
    float RanX;
    float RanY;
    float RanZ;

    bool IsResetY = false;
    bool IsFloating = false;
    bool IsSlerpY = false;

    public Vector3 OriPos;
    public Vector3 MonsterPos;

    Vector3 PlayerFixPos;

    GameObject player;
    GameObject monster;

    private AudioSource ExpSE;
    public AudioClip[] ExpSE_Clip;


    // Use this for initialization
    void Start()
    {
        PlayerCon = FindObjectOfType<PlayerControl>();
        SE = FindObjectOfType<PlayerSE>();
        player = GameObject.FindWithTag("Player");
        monster = GameObject.FindWithTag("Monster");
        OriPos = transform.position;
        RanX = Random.Range(-5f, 5f);
        RanY = Random.Range(1, 5);
        RanZ = Random.Range(-5f, 5f);
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        this.GetComponent<SphereCollider>().isTrigger = true;
        Invoke("PopExpPoint", WaitPopTime);
        //this.GetComponent<Rigidbody>().AddForce(ExplosionPos * 3f);
        //this.GetComponent<Rigidbody>().AddExplosionForce(5f, ExplosionPos, 150f,-0.5f,ForceMode.Impulse);

    }

    void PopExpPoint()
    {
        //this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //this.GetComponent<SphereCollider>().isTrigger = false;
        Vector3 Force = new Vector3(RanX, RanY, RanZ);
        Vector3 ExplosionPos = this.transform.position + new Vector3(0, RanY, 45);
        this.GetComponent<Rigidbody>().AddForce(transform.up * 100f);
        this.GetComponent<Rigidbody>().AddForceAtPosition(Force * 20f, this.transform.position);
        ExpSE = GetComponent<AudioSource>();
        ExpSE.PlayOneShot(ExpSE_Clip[0]);
        Invoke("UseG", 0.25f);
    }

    void UseG()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<SphereCollider>().isTrigger = false;
    }

    void FixedUpdate()
    {
        GetExp();
    }


    void GetExp()
    {
        if (WaitTime <= 0)
        {


            WaitTime = 0;
            
                //this.GetComponent<Rigidbody>().useGravity = false;
                //this.GetComponent<SphereCollider>().isTrigger = true;
                PlayerFixPos = player.transform.position + new Vector3(0f, 1.25f, 0f);
            //CheckPlayerPos();
            //Vector3 move = Vector3.Slerp(this.transform.position, PlayerFixPos, smoothSpeed);
            //this.transform.position = move;
            this.transform.DOLocalMove(PlayerFixPos, 0.25f);
          
            //this.GetComponent<SphereCollider>().isTrigger = true;


            if (Vector3.Distance(player.transform.position, this.transform.position) < 1.5f)
                {
                    PlayerCon.ExpPoint += 1;
                    SE.IsExpPick++;
                    Destroy(gameObject);
                }
                 
        }
        else
        {
            WaitTime -= Time.deltaTime;
        }


        Destroy(gameObject, 10f);
    }

    void CheckPointY()
    {
        OriPos = transform.position;
    }



    void CheckPlayerPos()
    {
        if (this.transform.position.x - player.transform.position.x > 0)
        {
            PlayerFixPos = player.transform.position + new Vector3(-0.5f, 0.2f, 0f);
            if (this.transform.position.z - player.transform.position.z > 0)
            {
                PlayerFixPos = player.transform.position + new Vector3(0f, 0.2f, -1f);
            }
            else if (this.transform.position.x - player.transform.position.x < 0)
            {
                PlayerFixPos = player.transform.position + new Vector3(0f, 0.2f, 1f);
            }
        }
        else if (this.transform.position.x - player.transform.position.x < 0)
        {
            PlayerFixPos = player.transform.position + new Vector3(0.5f, 0.2f, 0f);
            if (this.transform.position.z - player.transform.position.z > 0)
            {
                PlayerFixPos = player.transform.position + new Vector3(0f, 0.2f, -1f);
            }
            else if (this.transform.position.x - player.transform.position.x < 0)
            {
                PlayerFixPos = player.transform.position + new Vector3(0f, 0.2f, 1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

}
