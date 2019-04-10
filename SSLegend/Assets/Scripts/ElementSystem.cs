using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSystem : MonoBehaviour {

    float EffectTime = 10f;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckState() {
        if (EffectTime > 0)
        {
            EffectTime -= Time.deltaTime;
        }
        else {
            EffectTime = 0;
        }
    }

    void CheckUseItem() {
      
    }

}
