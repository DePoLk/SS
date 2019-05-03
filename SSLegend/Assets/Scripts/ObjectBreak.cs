using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreak : MonoBehaviour {

    [Header("破壞的物件")]
    public GameObject BrokenObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AtkBox")) {
            BrokenObject.transform.parent = null;
            BrokenObject.SetActive(true);
            this.gameObject.SetActive(false);
            Destroy(BrokenObject,2f);
            Destroy(this.gameObject,3f);
        }
    }
}
