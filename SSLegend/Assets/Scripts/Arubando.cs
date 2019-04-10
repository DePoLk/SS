using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arubando : MonoBehaviour {
    public Transform player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(player.position, this.transform.position) <= 30 )
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            
        }
    }
}
