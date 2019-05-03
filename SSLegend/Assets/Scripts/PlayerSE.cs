
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour {

    BGMManager BGM;
    PlayerControl Player;
    public AudioSource SE;
    public AudioClip ExpSE;
    public AudioClip Running;
    public AudioClip AtkSE;
    public AudioClip DodgeSE;
    public AudioClip landingSE;
    public AudioClip HurtSE;
    public AudioClip SplashSE;
    public AudioClip DieSE;
    public AudioClip DrinkSE;
    public AudioClip HitStoneSE;
    public AudioClip HitTreeSE;

    public int IsExpPick;
    public int IsAtk;

    // Use this for initialization
    void Start() {
        SE = GetComponent<AudioSource>();
        BGM = FindObjectOfType<BGMManager>();
    }

    // Update is called once per frame
    void Update() {
        ExpPick();
        Atk_SE();
        //在此新增要判定的音效
    }

    public void ExpPick()
    {
        if (IsExpPick > 0)
        {
            SE.volume = 0.8f;
            SE.PlayOneShot(ExpSE);
            IsExpPick = 0;
        }//當獲得exp時IsExpPick會有變動，便播出一次音效
    }
    public void run_SE() {
       
        SE.volume = 0.5f;
        SE.PlayOneShot(Running);

    }

    public void Atk_SE() {
        if (IsAtk > 0) {
            SE.volume = 0.5f;
            SE.PlayOneShot(AtkSE);
            IsAtk = 0;
        }
    }
    public void Dodge_SE() {
        SE.volume = 0.3f;
        SE.PlayOneShot(DodgeSE);
    }
    public void Landing_SE() {
        SE.volume = 0.6f;
        SE.PlayOneShot(landingSE);
    }
    public void Hurt_SE() {
        SE.volume = 0.6f;
        SE.PlayOneShot(HurtSE);
        SE.PlayOneShot(SplashSE);
    }
    public void Die_SE()
    {
        Debug.Log("die");
        SE.volume = 1.0f;
        SE.PlayOneShot(SplashSE);
        SE.PlayOneShot(DieSE);
        BGM.ChangeToDeath();
    }

    public void Cutton_SE() {
        SE.PlayOneShot(ExpSE);
    }

    public void Drink_SE() {
        SE.PlayOneShot(DrinkSE);
    }

    public void HitGround_SE() {
        SE.volume = 1.0f;
        SE.PlayOneShot(HitStoneSE);

    }
    public void HitPlants_SE() {
        SE.volume = 1.0f;
        SE.PlayOneShot(HitTreeSE);
    }
}

