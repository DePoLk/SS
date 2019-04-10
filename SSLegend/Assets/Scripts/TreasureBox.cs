using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour {

    public GameObject ExpPoint;
    PlayerControl PlayerCon;
    BillBoard BB;
    Animator Item_Ani;

    public Sprite TreasureTip;
    Item item;
    int DropNum = 8;
    Vector3 NewPos = new Vector3(-0.1f, 0.3f, 0f);
    float ranX;
    float ranY;
    float ranZ;

    float opentime = 0.8f;

    // Use this for initialization
    void Start () {
        item = this.GetComponent<Item>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        BB = FindObjectOfType<BillBoard>();
        Item_Ani = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckBoxOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            BB.Appear(PlayerCon.gameObject,PlayerCon.TipBG[0]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BB.disAppear(PlayerCon.gameObject);
        }
    }

    void DropExpPoint()
    {
        this.gameObject.GetComponent<DSItem>().PushValueToDS();
        this.gameObject.transform.parent.GetComponent<StageEvent>().CheckIsSelfDone();
        for (int i = 0; i < DropNum; i++)
            {
                /*ranX = Random.Range(-0.1f, 0.1f);
                ranY = Random.Range(0, 0.1f);
                ranZ = Random.Range(-0.1f, 0.1f);*/
                ranX = 0;
                ranY = 0;
                ranZ = 0;
                NewPos += new Vector3(ranX, ranY, ranZ);
                // ExpPoint.GetComponent<Rigidbody>().useGravity = true;
                //ExpPoint.GetComponent<SphereCollider>().isTrigger = false;
                Instantiate(ExpPoint, this.transform.position + NewPos, ExpPoint.transform.rotation);
            }
        //Destroy(gameObject,3f);
    }

    void CheckBoxOpen() {
        if (item.IsBoxOpen)
        {
            

            this.GetComponent<Animator>().SetBool("IsBoxOpen", true);
            opentime -= Time.fixedDeltaTime;
           // Player.NearItem = false;
            if (this.GetComponent<BoxCollider>().enabled && opentime <= 0f)
            {
                this.GetComponent<BoxCollider>().enabled = false;
                BB.disAppear(PlayerCon.gameObject);
                DropExpPoint();
                PlayerCon.NearItem = false;
            }
        }
    }

}
