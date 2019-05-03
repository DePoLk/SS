using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.SceneManagement;

public class AnimationFunction : MonoBehaviour {

    public GameObject Player;
    PlayerControl PlayerCon;
    PlayerSE PSE;
    Rigidbody rb;
    WaterValueChange WVC;
    public GameObject AtkBox;
    public GameObject DodgeFart;
    public GameObject Dodgefart_spot;
    MeleeWeaponTrail MWT;
    Transform Player_Transform;
    public GameObject ParticleEffect_Drink;
    public GameObject ParticleEffect_waterEmber;
    public GameObject FatPressBox;
    public GameObject FatAirAtkBox;
    public GameObject footsprint;
    public GameObject footSpotL;
    public GameObject footSpotR;
    public GameObject potion;
    public GameObject potion_Body;

    Animator Player_Ani;

    Scene ThisScene;

    // Use this for initialization
    void Start() {
        rb = Player.GetComponent<Rigidbody>();
        PlayerCon = FindObjectOfType<PlayerControl>();
        PSE = FindObjectOfType<PlayerSE>();
        MWT = GetComponentInChildren<MeleeWeaponTrail>();
        Player_Transform = Player.GetComponent<Transform>();
        ThisScene = SceneManager.GetActiveScene();
        Player_Ani = PlayerCon.gameObject.GetComponent<Animator>();
        WVC = FindObjectOfType<WaterValueChange>();
        potion_Body.SetActive(true);
        potion.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    public void PosPlus1() {
        

        if (ThisScene.name.Equals("2-1"))
        {
            if (PlayerCon.IsGround)
            {
                rb.AddForce(transform.forward * -20f * PlayerCon.ForceFixForward + transform.right * -20f * PlayerCon.ForceFixRight);
            }
        }
        else if(ThisScene.name.Equals("K")){
            if (PlayerCon.IsGround)
            {
                rb.AddForce(transform.right * -30f);
            }
        }

    }
    public void PosPlus2() {
       

        if (ThisScene.name.Equals("2-1"))
        {
            if (PlayerCon.IsGround)
            {
                rb.AddForce(transform.forward * -200f * PlayerCon.ForceFixForward + transform.right * -200f * PlayerCon.ForceFixRight);
            }
        }
        else if (ThisScene.name.Equals("K"))
        {
            if (PlayerCon.IsGround)
            {
                rb.AddForce( transform.right * -200f);
            }
        }
    }
    public void PosPlusStop() {
        if (PlayerCon.IsGround) {
            //rb.AddForce(transform.forward * 90f + transform.right * 90f);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void PosGetStriked() {
        
        PlayerCon.IsStriked = true;
        PlayerCon.GetComponent<Animator>().SetBool("PlayerPush", false);
        PlayerCon.GetComponent<Animator>().SetBool("PlayerPull", false);
        PlayerCon.AniAtkCount = 0;
        AtkBox.SetActive(false);
        WVC.ImgWaterValue = 0;

        if (ThisScene.name.Equals("2-1"))
        {
            //rb.AddForce(transform.forward * 100f * PlayerCon.ForceFixForward + transform.right * 100f * PlayerCon.ForceFixRight);
            rb.AddForce(PlayerCon.TheObjGiveStrike.transform.forward * 100f);
        }
        else if (ThisScene.name.Equals("K"))
        {
            rb.AddForce(transform.right * 100f);
        }

        FatPressBoxClose();

        
    }

    public void ResetIsStriked()
    {
        PlayerCon.IsHolding = false;
        PlayerCon.IsStriked = false;
        PlayerCon.IsJumping = false;
        PlayerCon.IsAtking = false;
        PlayerCon.IsDodging = false;
        PlayerCon.IsFating = false;
        PlayerCon.GetComponent<Animator>().SetBool("PlayerUseWind", false);
        PlayerCon.GetComponent<Animator>().SetBool("PlayerFastRun", false);

        PlayerCon.AniAtkCount = 0;
        
        Time.timeScale = 1;
    }

    public void LookatGiveStrikeObj() {
        //Player.transform.rotation = Quaternion.Slerp(transform.rotation,PlayerCon.TheObjGiveStrike.transform.rotation,Time.deltaTime*25f);
        //Player.transform.DOLocalRotate(new Vector3(0, PlayerCon.TheObjGiveStrike.transform.position.y, 0),1f);
        /*Vector3 TargetPos = new Vector3(PlayerCon.TheObjGiveStrike.transform.position.x,Player.transform.position.y,PlayerCon.TheObjGiveStrike.transform.position.z);
        Player.transform.LookAt(TargetPos);                
        Player.transform.eulerAngles += new Vector3(0,-90,0);*/
        PlayerCon.IsDrinking = false;
        PlayerCon.StopCoroutine("DelayIsFat");
    }//被扁時看向受擊面

    public void ResetAniTime() {
        PlayerCon.AniTime = 0;
        PlayerCon.GetComponent<Animator>().SetBool("PlayerAtk",false);
    }

    public void AtkSEIsAtk() {
        PSE.IsAtk++;
    }
    public void StartweaponTrail() {
        MWT.Emit = true;
       
    }
    
    
    public void EndweaponTrail() {
        MWT.Emit = false;
        
    }

    public void ResetDodge() {
        PlayerCon.IsDodging = false;
        PlayerCon.IsInvincible = false;
        PlayerCon.InvincibleTime = PlayerCon.DefaultInvincibleTime;
    }

    public void ResetJump() {
        PlayerCon.IsJumping = false;
    }

    public void PosPlusDodge() {
        if (PlayerCon.IsGround)
        {
            AtkBox.SetActive(false);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x,PlayerCon.angle_Sum,transform.eulerAngles.z);
            //transform.DORotate(new Vector3(transform.eulerAngles.x, PlayerCon.angle_Sum, transform.eulerAngles.z), 0.25f);
           

            if (ThisScene.name.Equals("2-1"))
            {
                 rb.AddForce(transform.forward * -100f * PlayerCon.ForceFixForward + transform.right * -100f * PlayerCon.ForceFixRight);
            }
            else if (ThisScene.name.Equals("K"))
            {
                rb.AddForce(transform.right * -150f);
            }

        }
    }

    public void PosPlusJump() {
        if (PlayerCon.IsGround)
        {
            rb.AddForce(transform.up * PlayerCon.JumpSpeed);
        }
    }

    public void OpenAtkBox() {
        AtkBox.SetActive(true);
    }

    public void CloseAtkBox() {
        AtkBox.SetActive(false);
    }
    //---

    public void PlayerTurning() {
        Player.GetComponent<Animator>().SetTrigger("PlayerTurn");
        PlayerCon.IsTurning = true;
    }

    public void PlayerIsTurningReset() {
        PlayerCon.IsTurning = false;
    }

 

    public void PlayerTurnAddforce() {
        rb.AddForce(transform.forward * -125f);
    }

    //---
    public void PullAndPushPosReset() {

    }
    public void footprintL() {
        GameObject a = Instantiate(footsprint, footSpotL.transform.position,Player.transform.rotation);
        Destroy(a, 2.0f);

    }
    public void footprintR()
    {
        GameObject a = Instantiate(footsprint, footSpotR.transform.position, Player.transform.rotation);
        Destroy(a, 2.0f);
    }
    public void DodgeFarting() {
       GameObject a = Instantiate(DodgeFart, Dodgefart_spot.transform.position, Dodgefart_spot.transform.rotation);
        Destroy(a, 2.0f);
        //PSE.Dodge_SE();
    }
    public void DodgeFarting_SE()
    {
       
        PSE.Dodge_SE();
    }

    //---

    public void PlayerAtkDetectorOpen() {
        PlayerCon.IfPlayerAtkDetector = true;
    }

    public void PlayerAtkDetectorClose() {
        PlayerCon.IfPlayerAtkDetector = false;
    }

    //---

    public void DeathForce() {
        
       

        if (ThisScene.name.Equals("2-1"))
        {
            //Player.transform.DOLocalRotate(PlayerCon.TheObjGiveStrike.transform.position + new Vector3(0,0,0), 0f);
            //rb.AddForce(transform.forward * 200f * PlayerCon.ForceFixForward + transform.right * 200f * PlayerCon.ForceFixRight);
            Vector3 TargetPos = new Vector3(PlayerCon.TheObjGiveStrike.transform.position.x, Player.gameObject.transform.position.y,PlayerCon.TheObjGiveStrike.transform.position.z);
            Player.gameObject.transform.LookAt(TargetPos);
            Player.gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
            rb.AddForce(PlayerCon.TheObjGiveStrike.transform.forward * 200f);

        }
        else if (ThisScene.name.Equals("K"))
        {
            Player.transform.DOLocalRotate(PlayerCon.TheObjGiveStrike.transform.position + new Vector3(0, 90, 0), 0f);
            rb.AddForce(transform.right * 200f);
        }
        AtkBox.SetActive(false);
    }

    //---

    public void PlayerIsDrink() {
        PlayerCon.IsDrinking = true;
        
    }

    public void PlayerResetDrink() {
        PlayerCon.IsDrinking = false;
        PlayerCon.WaterElement -= 1;
        //PlayerCon.CurrentModelChange(1);
        Vector3 tmp = Player.transform.position;
        tmp.y += 1f;
        GameObject a = Instantiate(ParticleEffect_waterEmber, tmp, Player.transform.rotation);
        Destroy(a, 3.0f);
        
    }
    void playDrinkingAffect() {
        Vector3 tmp = Player.transform.position;
        tmp.y += 1.5f;
       // tmp.z += 0.5f;
        GameObject a = Instantiate(ParticleEffect_Drink,tmp,Player.transform.rotation);
        Destroy(a, 4.0f);
    }

    //--- 

    public void PlayerResetPanting() {
        PlayerCon.IsPanting = false;
        PlayerCon.IsJumping = false;
        Player.GetComponent<Animator>().SetBool("PlayerPanting", false);
        PlayerCon.RunTime = 0;
    }

    //--- 

    public void PlayerResetFatPressDown() {
        Player.GetComponent<Animator>().SetBool("PlayerPress", false);
        PlayerCon.IsJumping = false;
    }

    public void PlayerFatPressDownPosPlusUp() {
        rb.AddForce(transform.up * 250f);
    }

    public void PlayerFatPressDownPosPlusDown() {
        rb.AddForce(transform.up * -350f);
    }


    //--- 

    public void FatPressBoxOpen() {
        FatPressBox.SetActive(true);
    }

    public void FatPressBoxClose() {
        FatPressBox.SetActive(false);
    }

    //---

    public void FatAirAtkBoxOpen() {
        FatAirAtkBox.SetActive(true);
    }
    public void FatAirAtkBoxClose()
    {
        FatAirAtkBox.SetActive(false);
    }

    //---

    public void IdleDeBug() {
        PlayerCon.AniAtkCount = 0;
    }

    //---

    public void GetStrikeDownForce() {
        rb.AddForce(transform.right * 200f);
    }

    public void ResetIsStrikeDown() {
        Player_Ani.SetBool("PlayerStrikeDown",false);
    }

    public void PotionOn() {
        potion.SetActive(true);
        potion_Body.SetActive(false);
    }
    public void PotionOff()
    {
        potion.SetActive(false);
        potion_Body.SetActive(true); 
    }
    
    public void ResetUseWind()
    {
        PlayerCon.GetComponent<Animator>().SetBool("PlayerUseWind",false);
    }

}
