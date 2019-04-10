using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowWolf_Anim : MonoBehaviour {
    public Animator anim;
    private Arrow_Wolf AW;
	// Use this for initialization
	void Start () {
        AW = GetComponentInParent<Arrow_Wolf>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Calling() {
        AW.ArrowShoot();
    }
    public void closeshadows() {
        this.GetComponentInChildren<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
}
