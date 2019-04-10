using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Debug.Log(gameObject.transform.parent.GetChild(1).GetChild(0).name);
    }

    // Update is called once per frame
    void Update () {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FatPressBox")) {
           
            for (int i = 0; i < gameObject.transform.parent.GetChild(1).childCount; i++) {               
                gameObject.transform.parent.GetChild(1).GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                gameObject.transform.parent.GetChild(1).GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                gameObject.transform.parent.GetChild(1).GetChild(i).GetComponent<MeshRenderer>().enabled = true;              
            }
            


            //gameObject.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            Destroy(gameObject,500f);
        }
    }
}
