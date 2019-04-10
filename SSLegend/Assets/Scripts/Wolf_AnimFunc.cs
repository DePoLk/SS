using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_AnimFunc : MonoBehaviour {

    private Animator anim;
    private MeleeWeaponTrail_wolf MWT_w;
    private NormalMonster NM;
    void Start()
    {
        NM = GetComponentInParent<NormalMonster>();
        MWT_w = GetComponentInChildren<MeleeWeaponTrail_wolf>();
        anim = GetComponent<Animator>();
    }

    public void ResetHurt()
    {
        anim.SetBool("hurt", false);
        NM.StartCoroutine("AfterAtk");
    }
    //swordLight
    public void Start_swordLight()
    {
        MWT_w.Emit = true;
    }
    public void End_swordLight()
    {
        MWT_w.Emit = false;
    }
    public void JumpForce()
    {
        NM.JumpForce();
    }
    public void Force()
    {
        this.GetComponentInParent<Rigidbody>().AddForce(transform.right * 5);
    }
    public void resetatk1() {
        NM.ResetAtk1();
    }
    public void resetatk2() {
        NM.resetatk2();
    }
    public void WolfAtkBox() {
        NM.StartCoroutine("Atk");
    }
}
