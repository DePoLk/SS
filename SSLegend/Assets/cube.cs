using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {
    public float m_speed = 5f;
    public GameObject swordLight;
    public GameObject sword_spot;
    private ParticleSystem ps;
    // Use this for initialization
    void Start () {
        ps = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        var em = ps.emission;
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) //前
        {
            this.transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
            em.enabled = true;
        }
        
        if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow)) //后
        {
            this.transform.Translate(Vector3.forward * -m_speed * Time.deltaTime);
            em.enabled = true;
        }
        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
        {
            this.transform.Translate(Vector3.right * -m_speed * Time.deltaTime);
            em.enabled = true;
        }
        if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
        {
            this.transform.Translate(Vector3.right * m_speed * Time.deltaTime);
            em.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject a =  Instantiate(swordLight, sword_spot.transform);
        Destroy(a, 1.0f);
    }
}
