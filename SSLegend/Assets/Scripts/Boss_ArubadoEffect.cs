using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ArubadoEffect : MonoBehaviour {
    public GameObject Charge_Dust;
    public GameObject Charge_StopDust;
    public GameObject EarthCrack;
    public GameObject EarthCrack_Trail;
    public GameObject ShootFire;
    public GameObject ShootChargeEmber;
    public GameObject Slash_Dust;
    public GameObject Slash_Trail;
    public Material Arubado_mat;
    public Material ArubadoLeftHand_mat;
    public Material ArubadoRightHand_mat;
    public Material ArubadoCore_mat;
    public GameObject EarthCrack_Spot;
    public GameObject EarthTrail_Spot;
    public GameObject ShootFire_Spot;
    public GameObject ShootEmber_Spot;
    public GameObject Slash_Spot;
    public GameObject SlashTrail_Spot;
    private bool StartCharge = false;
    private float EmissionColor_value = 0.5f;
    private bool StartLeftCharge = false;
    private float EmissionColor_valueL = 0.5f;

    private bool StartRightCharge = false;
    private float EmissionColor_valueR = 0.5f;

    private AudioSource audi;
    public AudioClip[] SoundEffect;
    private CameraControl CC;

    // Use this for initialization
    void Start () {
        audi = GetComponent<AudioSource>();
        CC = FindObjectOfType<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () {
        //全身
        if (StartCharge)
        {
            EmissionColor_value += 0.055f;
            if (EmissionColor_value <= 3.0f) {
                Arubado_mat.SetColor("_EmissionColor", Color.white * EmissionColor_value);
                ArubadoCore_mat.SetColor("_EmissionColor", Color.white * EmissionColor_value);
            }
                
           
        }
        else {
            
            if (EmissionColor_value >= 0.5f)
            {
                EmissionColor_value -= 0.04f;
                Arubado_mat.SetColor("_EmissionColor", Color.white * EmissionColor_value);
                ArubadoCore_mat.SetColor("_EmissionColor", Color.white * EmissionColor_value);
            }

        }
        //////左手
        if (StartLeftCharge)
        {
            EmissionColor_valueL += 0.07f;
            if (EmissionColor_valueL <= 2.5f)
            {
                ArubadoLeftHand_mat.SetColor("_EmissionColor", Color.white * EmissionColor_valueL);
            }


        }
        else
        {

            if (EmissionColor_valueL >= 0.5f)
            {
                EmissionColor_valueL -= 0.1f;
                ArubadoLeftHand_mat.SetColor("_EmissionColor", Color.white * EmissionColor_valueL);
            }

        }
        //右手
        if (StartRightCharge)
        {
            EmissionColor_valueR += 0.055f;
            if (EmissionColor_valueR <= 3.0f)
            {
                ArubadoRightHand_mat.SetColor("_EmissionColor", Color.white * EmissionColor_valueR);
            }


        }
        else
        {

            if (EmissionColor_valueR >= 0.5f)
            {
                EmissionColor_valueR -= 0.055f;
                ArubadoRightHand_mat.SetColor("_EmissionColor", Color.white * EmissionColor_valueR);
            }

        }
    }
    public void StartChargetoDash() {

        StartCharge = true;
    }
    public void ChargeDust() {
        Charge_Dust.SetActive(true);
        StartCharge = false;
        StartLeftCharge = false;
        StartRightCharge = false;
    }
    
    public void ChargeEndDust() {
        
        Charge_Dust.SetActive(false);
        GameObject a = Instantiate(Charge_StopDust,transform.position,Charge_Dust.transform.rotation);
        
        Destroy(a, 2.0f);
    }

    public void ChargeToEarthcrack() {
        StartLeftCharge = true;
    }
    public void Earthcrack() {
        GameObject a = Instantiate(EarthCrack, EarthCrack_Spot.transform);
        a.transform.SetParent(EarthTrail_Spot.transform.parent);
        Destroy(a, 5.0f);
        CC.shake();
    }
    public void Earthcrack_Trail() {
        
        GameObject a = Instantiate(EarthCrack_Trail, EarthTrail_Spot.transform);
        a.transform.SetParent(EarthTrail_Spot.transform.parent);
        Destroy(a, 1.0f);

    }
    public void EndToEarthCrack()
    {
        StartLeftCharge = false;

    }
    public void ChargeToShoot() {
        StartRightCharge = true;
    }
    public void EndToShoot() {
        StartRightCharge = false;
    }

    public void Shoot_Fire() {
        GameObject a = Instantiate(ShootFire, ShootFire_Spot.transform);
        a.transform.SetParent(ShootFire_Spot.transform.parent);
        Destroy(a, 3.0f);

    }
    public void Shoot_Ember() {
        GameObject a = Instantiate(ShootChargeEmber, ShootEmber_Spot.transform);
        a.transform.SetParent(ShootEmber_Spot.transform.parent);
        Destroy(a, 3.0f);

    }
    public void Slash_dust() {
        GameObject a = Instantiate(Slash_Dust, Slash_Spot.transform);
        a.transform.SetParent(Slash_Spot.transform.parent);
        Destroy(a, 3.0f);

    }
    public void Slash_trail() {
        GameObject a = Instantiate(Slash_Trail, SlashTrail_Spot.transform);
        a.transform.SetParent(SlashTrail_Spot.transform.parent);
        Destroy(a,1.0f);

    }
    public void Play_SE_Attack() {
        audi.PlayOneShot(SoundEffect[0]);
        
    }
    public void Play_SE_Slash()
    {
        audi.PlayOneShot(SoundEffect[1]);
    }
    public void Play_SE_ChargeMAGIC()
    {
        audi.PlayOneShot(SoundEffect[2]);
    }
    public void Play_SE_ShootMAGIC()
    {
        audi.PlayOneShot(SoundEffect[3]);
    }
    public void Play_SE_Down()
    {
        audi.PlayOneShot(SoundEffect[4]);
    }
    public void Play_SE_Charge()
    {
        audi.PlayOneShot(SoundEffect[5]);
    }
    public void Play_SE_getHit()
    {
        audi.PlayOneShot(SoundEffect[6]);
    }


}
