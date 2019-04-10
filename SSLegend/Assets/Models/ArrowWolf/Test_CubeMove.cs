using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CubeMove : MonoBehaviour {
    float speed = 10;
    public bool OnTop = false;
    // Use this for initialization
    void Start () {
        
	}
	// Update is called once per frame
	void Update () {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var posx = v * speed * Time.deltaTime;
        var posy = h * speed *Time.deltaTime*20;
        transform.Translate(0, 0, posx);
        transform.Rotate(0, posy, 0);

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "canon") {
            OnTop = true;
            Debug.Log(OnTop);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "canon")
        {
            OnTop = false;
            Debug.Log(OnTop);
        }
    }
}
