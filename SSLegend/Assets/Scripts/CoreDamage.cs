using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreDamage : MonoBehaviour {
    private Boss_Arubado BA;
    public Material core_mat;
    public Texture coreRGB;
    private Shader shader_m;

    private Boss_ArubadoEffect BAF;
	// Use this for initialization
	void Start () {
        BA = FindObjectOfType<Boss_Arubado>();
        BAF = FindObjectOfType<Boss_ArubadoEffect>();
        Debug.Log(BA.NewHP);
        shader_m = core_mat.shader;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AtkBox" ) {
            StartCoroutine("Flash");
            BA.NewHP--;
            BAF.Play_SE_getHit();
            Debug.Log(BA.NewHP);
            
        }
    }
    private IEnumerator Flash()
    {
       
        //core_mat.SetTexture("_MainTex", null);
        core_mat.shader = Shader.Find("Custom/FlashLight");
        
        //sword.GetComponent<MeshRenderer>().material.shader = Shader.Find("Custom/FlashLight");
        yield return new WaitForSeconds(0.075f);
        //core_mat.SetTexture("_MainTex", coreRGB);
        core_mat.shader = shader_m;
    }
}
