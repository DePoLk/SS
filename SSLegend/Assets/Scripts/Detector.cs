using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
    private bool dodge = false;
    private NormalMonster NM;
    private int RoL;
	// Use this for initialization
	void Start () {
        NM = GetComponentInParent<NormalMonster>();
    }
	
	// Update is called once per frame
	void Update () {
        if (dodge == true) {
            if (RoL == 1) { NM.DetectObstacle(true, 1); }
            else if (RoL == 2) { NM.DetectObstacle(true, 2); }
           
           
        }else if (dodge == false)
        {
            NM.DetectObstacle(false,0);
        }
    }
    void OnTriggerEnter(Collider w) {
        if (w.CompareTag("obstacle")) {
            Vector3 direction = w.transform.position - this.transform.position;
            var cross = Vector3.Cross(this.transform.forward, direction);
            if (cross.y < 0) {
                RoL = 1;
                Debug.Log("Left");
            }
            else if (cross.y > 0)
            {
                RoL = 2;
                Debug.Log("Right");
            }
            dodge = true;
        }
    }
   void OnTriggerExit(Collider q) {
        if (q.CompareTag("obstacle"))
        {
            dodge = false;
           // Debug.Log("!!");
        }
    }

}
