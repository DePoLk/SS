using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class Cutton : MonoBehaviour
{

    float RanX;
    float RanY;
    float RanZ;

    float WaitTime = 1.2f;
    Vector3 PlayerFixPos;

    GameObject PlayerObj;
    PlayerControl PlayerCon;
    PlayerUI PUI;
    Material bear_mat;
    PlayerSE PSE;

    float smoothSpeed = 0.1f;
    float dis;

    float floatingForce = 0.01f;
    float Ani_Time;
    float Default_Time = 1f;

    bool stopForce = false;
    // Use this for initialization
    void Start()
    {
        CreateCutton();
        PSE = FindObjectOfType<PlayerSE>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        PlayerObj = GameObject.Find("Player");
        PUI = FindObjectOfType<PlayerUI>();
        Ani_Time = Default_Time;
        bear_mat = Resources.Load("Bear", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetExp();

        if (stopForce) {
            floating();
        }

        
    }


    void CreateCutton()
    {
        RanX = Random.Range(-25, 25);
        RanY = Random.Range(-25, 25);
        RanZ = Random.Range(-25, 25);
        Vector3 Force = new Vector3(RanX, RanY, RanZ);
        //this.GetComponent<BoxCollider>().isTrigger = true;
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.1f);              
        this.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up * 0.3f, this.transform.position);
        
    }

    void GetExp()
    {
        if (WaitTime <= 0)
        {
            if (!stopForce)
            {
                //this.GetComponent<BoxCollider>().isTrigger = false;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                stopForce = true;
            }


            PlayerFixPos = PlayerCon.transform.position + new Vector3(0f, 1f, 0f);
            dis = Vector3.Distance(this.transform.position, PlayerFixPos);
            if (dis < 1.5f)
            {
                Vector3 move = Vector3.Slerp(this.transform.position, PlayerFixPos, smoothSpeed);
                this.transform.position = move;
            }



            if (Vector3.Distance(PlayerCon.transform.position, this.transform.position) < 1.5f)
            {
                
                if (PlayerCon.Hp < PlayerCon.MaxHp)
                {
                    PlayerCon.Hp += 1;
                    
                }
                else{
                    PlayerCon.Hp = PlayerCon.MaxHp;
                }
                
                //SE.IsExpPick++;
                PUI.CottonHPArray[PlayerCon.Hp].SetActive(true);
                PUI.CottonHPArray[PlayerCon.Hp].GetComponent<Image>().DOFade(1, 0.25f);

                StartCoroutine("Flash");
                //Destroy(gameObject);
            }
        }
        else
        {
            WaitTime -= Time.deltaTime;
        }


        Destroy(gameObject, 10f);
    }

    void floating() {
        if (Ani_Time <= 0)
        {
            //this.GetComponent<Rigidbody>().velocity = new Vector3(0,floatingForce,0);
            floatingForce = -floatingForce;
            Ani_Time = Default_Time*2f;
        }
        else {
            Ani_Time -= Time.deltaTime;
        }
        //Debug.Log(floatingForce);
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * floatingForce);    
    }

    private IEnumerator Flash()
    {
        PSE.Cutton_SE();
        bear_mat.shader = Shader.Find("Custom/FlashLight_Health");
        bear_mat.SetColor("_Color",new Color32(170,250,200,255));
        yield return new WaitForSeconds(0.075f);
        bear_mat.shader = Shader.Find("Custom/PlayerDiffuse");
        bear_mat.SetColor("_Color", Color.white);
        Destroy(gameObject);

    }

}
