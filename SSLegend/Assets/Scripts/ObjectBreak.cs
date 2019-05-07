using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreak : MonoBehaviour {

    [Header("破壞的物件")]
    public GameObject BrokenObject;

    public AudioClip Clip;
    AudioSource AS;


	// Use this for initialization
	void Start () {
        AS = this.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AtkBox")) {
            AS.PlayOneShot(Clip);           
            Debug.Log("發聲音");
            BrokenObject.transform.parent = null;
            BrokenObject.SetActive(true);
            //this.gameObject.SetActive(false);         
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(BrokenObject,2f);
            Destroy(this.gameObject,3f);
        }
    }
}
