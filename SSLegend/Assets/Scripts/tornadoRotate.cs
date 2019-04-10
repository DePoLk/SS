using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoRotate : MonoBehaviour {

    public Vector3 speed;
    public Vector3 Ura_speed;
    private GameObject UraTornado;
	// Use this for initialization
	void Start () {
        
        UraTornado = GameObject.Find("Tronado_vfx URA");

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(speed * Time.deltaTime);
        if (UraTornado == null)
        {
            UraTornado = GameObject.Find("Tronado_vfx URA");
            UraTornado.transform.Rotate(Ura_speed * Time.deltaTime);
        }
        else {
            UraTornado.transform.Rotate(Ura_speed * Time.deltaTime);
        }
       

    }
}
