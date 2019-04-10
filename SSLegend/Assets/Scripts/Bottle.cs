using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour {

    public GameObject BrokenObject;
    AudioSource Audio_Source;
    public AudioClip Audio_Clip;
    public GameObject ExpPoint;


    bool IsDropExp = false;

    public int DropNum;
    Vector3 NewPos = new Vector3(-0.1f, 0.3f, 0f);
    float ranX;
    float ranY;
    float ranZ;

    


    // Use this for initialization
    void Start () {
        Audio_Source = this.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
       
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AtkBox") && this.tag == "Bottle") {
            Audio_Source.PlayOneShot(Audio_Clip);
            Audio_Clip = null;
            if (!IsDropExp) {
                DropExpPoint();
            }
            BrokenObject.SetActive(true);
            BrokenObject.GetComponentInChildren<BoxCollider>().size = new Vector3(0.1f,0.1f,0.1f);
            //BrokenObject.GetComponentInChildren<Rigidbody>().AddForceAtPosition(ExplosionPos, transform.position);
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().isTrigger = true;

            this.gameObject.transform.parent.GetComponent<DSItem>().PushValueToDS();
            

            Destroy(gameObject.transform.parent.gameObject,3f);
        }
    }
    void DropExpPoint()
    {
        this.gameObject.transform.parent.parent.GetComponent<StageEvent>().CheckIsSelfDone();
        for (int i = 0; i < DropNum; i++)
        {          
            ranX = 0;
            ranY = 0;
            ranZ = 0;
            NewPos += new Vector3(ranX, ranY, ranZ);
            
            Instantiate(ExpPoint, this.transform.position + NewPos, ExpPoint.transform.rotation);
        }
        IsDropExp = true;
    }



}
