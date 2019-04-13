using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterExpDrop : MonoBehaviour {

    public GameObject ExpPoint;
    public int DropNum = 3;
    public float F = 50f; // 噴經驗力量
    public bool MonsterIsDead = false;
    Vector3 NewPos = new Vector3(0f, 0.01f, 0f);
    float ranX;
    float ranY;
    float ranZ;
    //public GameObject deadParticle;

    Karohth Karohth;
    NormalMonster Wolf;
    Arrow_Wolf ArrowWolf;

    public int MonsterHP;
    public string MonName;
	// Use this for initialization
	void Start () {


        CheckWhichMonster();

    }
	
    void CheckWhichMonster (){
        if (this.tag.Equals("Karohth"))
        {
            Karohth = this.gameObject.GetComponent<Karohth>();

            MonsterHP = Karohth.HP;
            MonName = "Karohth";
        }
        if (this.tag.Equals("Wolf")) {
            Wolf = this.gameObject.GetComponent<NormalMonster>();

            MonsterHP = Wolf.HP;
            MonName = "Wolf";
        }
        if (this.tag.Equals("ArrowWolf"))
        {
            ArrowWolf = this.gameObject.GetComponent<Arrow_Wolf>();

            MonsterHP = ArrowWolf.HP;
            MonName = "ArrowWolf";
        }

    }


	// Update is called once per frame
	void Update () {
        //Destroy(gameObject);//刪除怪物

        if (MonsterHP <= 0) {
            MonsterIsDead = true;
        }

            DropExpPoint();      
    }

    void DropExpPoint() {
        if (MonsterIsDead && DropNum != 0)
        {
            //Instantiate(deadParticle, this.transform.position, this.transform.rotation);
            for (int i = 0; i < DropNum; i++)
            {
                ranX = Random.Range(-450, 450);
                ranY = Random.Range(0, 200);
                ranZ = Random.Range(-450, 450);
                NewPos += new Vector3(ranX / 700, ranY / 700, ranZ / 700);
                // ExpPoint.GetComponent<Rigidbody>().useGravity = true;
                //ExpPoint.GetComponent<SphereCollider>().isTrigger = false;
                Instantiate(ExpPoint, this.transform.position + NewPos, ExpPoint.transform.rotation);
            }
            MonsterIsDead = false;
            DropNum = 0;
        }    
        
    }


    void Reset_G() {
       // ExpPoint.GetComponent<Rigidbody>().useGravity = false;
       // ExpPoint.GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AtkBox") && this.CompareTag(MonName)) {
            if (MonsterHP >= 1)
            {
                MonsterHP--;
            }
            
        }
    }

}
