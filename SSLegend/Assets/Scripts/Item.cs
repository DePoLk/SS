using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour {

    PlayerControl Player;
    public bool IsBoxOpen = false;
    private float alpha = 255.0f;
    public float DeadTime = 5.0f;
    public float CoolTime = 15f;
    public bool IsCoolDown = false;

    // Use this for initialization
    void Start () {
        Player = FindObjectOfType<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {

        /* DeadTime -= Time.deltaTime;
         if (DeadTime > 0) {
             Color ren = GetComponent<Renderer>().material.color;
             alpha = alpha - 10.0f;
             ren.a = alpha / 255.0f;
             GetComponent<Renderer>().material.SetColor("_Color",ren);
         }

         if (GetComponent<Renderer>().material.color.a <= 0) { }
         { Destroy(gameObject); }*/
        if (IsCoolDown) {
            if (CoolTime > 0) {
                CoolTime -= Time.deltaTime;
            }
            else if (CoolTime <= 0) {
                CoolTime = 15f;
                IsCoolDown = false;
            }
        }
        
    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            //Debug.Log("ReachItem");
            Player.NearItem = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("LeaveItem");
            Player.NearItem = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Player.NearItem) {

           

            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Pick");

                if (this.CompareTag("WaterElement") && !IsCoolDown)
                {          
                    Player.WaterElement += 3;
                    Player.IsGetItem = true;
                    //Destroy(gameObject);
                    IsCoolDown = true;
                }
                if (this.CompareTag("WindElement") && !IsCoolDown)
                {
                    /*Player.WindElement += 3;
                    Player.IsGetItem = true;
                    //Destroy(gameObject);
                    IsCoolDown = true;*/
                }
                if (this.CompareTag("TreasureBox")) {
                    
                    IsBoxOpen = true;
                    
                }

            }
            
            
        }
    }
}
