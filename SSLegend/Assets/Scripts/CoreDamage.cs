using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreDamage : MonoBehaviour {
    private Boss_Arubado BA;
    public Material core_mat;
    public Texture coreRGB;
    private Shader shader_m;
    PlayerUI PUI;
    private Boss_ArubadoEffect BAF;
	// Use this for initialization
	void Start () {
        BA = FindObjectOfType<Boss_Arubado>();
        BAF = FindObjectOfType<Boss_ArubadoEffect>();
        PUI = FindObjectOfType<PlayerUI>();
        Debug.Log(BA.NewHP);
        core_mat.shader = Shader.Find("Standard (Roughness setup)");
        shader_m = core_mat.shader;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AtkBox" ) {
            StartCoroutine("Flash");
            BA.NewHP-=1;
            BAF.Play_SE_getHit();
            Debug.Log(BA.NewHP);
            PUI.BossLoseHP();
            
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
