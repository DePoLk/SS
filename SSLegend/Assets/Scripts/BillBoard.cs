using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BillBoard : MonoBehaviour
{
    public GameObject cam;
   
   // public Sprite a;
    
    // Use this for initialization
    void Start()
    {

        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
     
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
    public void Appear(GameObject which  ,Sprite AppearImg) {
        which.GetComponentInChildren<SpriteRenderer>().sprite = AppearImg;
    }

    public void disAppear(GameObject which) {
        which.GetComponentInChildren<SpriteRenderer>().sprite = null;
    }
}
