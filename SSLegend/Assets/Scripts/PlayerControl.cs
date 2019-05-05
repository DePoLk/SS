using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.SceneManagement;
using Cinemachine;


public class PlayerControl : MonoBehaviour {
    /// <summary>
    /// 垂直輸入量
    /// </summary>
    [SerializeField]
    [Header("垂直輸入量")]
    public float input_V;

    /// <summary>
    /// 水平輸入量
    /// </summary>
    [SerializeField]
    [Header("水平輸入量")]
    public float input_H;

    /// <summary>
    /// 結果角度
    /// </summary>
    [SerializeField]
    [Header("結果角度")]
    public float angle_Sum;

    [SerializeField]
    [Header("目前道具")]

    public bool IsGetItem = false;
    public int WaterElement = 0;
    public int WindElement = 0;
    public float WindElementCoolTime = 0;
    public float DefaultWindElementCoolTime = 10f;
    public bool NearItem = false;

    public int Item_Type = 1;
    public GameObject Tornado;

    [SerializeField]
    [Header("角色速度")]
    public float DefaultSpeed;
    public float FatSpeed = -1f;
    public float speed;
    public float FatJumpSpeed;
    public float DefaultJumpSpeed;
    public float JumpSpeed;

    [Header("角色落地")]
    public bool IsGround = false;

    [Header("角色模組")]
    public GameObject[] ModelArray;
    public Avatar[] AvatarArray;
    public Sprite[] TipBG;
    
    

    [Header("角色數值")]
    public int ExpPoint = 0;
    public int Hp = 10;
    public int MaxHp = 5;
    public bool IsAtking = false;
    public bool IsHolding = false;
    public bool IsDead = false;
    public bool IsFat = false;
    public bool IsUIEventing = false;
    public bool IsInvincible = false;
    public float InvincibleTime;
    public float DefaultInvincibleTime;
    public bool IsEscMenu = false;
    public int PlayerAtkValue = 1;
    public bool IsInCannon = false;
    public bool IsInCinema = false;
    public bool OnTop = false;

    
   
    [Range(-280f,0f)]
    public float WaterValuePosY = -280;

    [Header("角色動畫數值")]
    public float AniTime = 0;
    public int AniAtkCount = 0;
    public float AniResetTime = 0.55f;
    public float RunTime = 0;
    public float FastTime = 3f;
    public bool IsFast = false;
    public bool IsJumping = false;
    public bool IsDodging = false;
    public bool IsTurning = false;
    public bool IsStriked = false;
    public bool IfPlayerAtkDetector = false;
    public bool IsDrinking = false;
    public bool IsPanting = false;
    public bool IsFating = false;
    public bool IsTeaching = false;
    public bool IsBurn = false;
    bool Item2IsColdDown = false;
    public int CurrentModelNum = 0;

    [Header("玩家死亡處理")]
    public GameObject DeadBlockSight;

    Tweener DBS;

    public Boss_Arubado BA; // Boss一開始被SET FALSE 會出現讀取錯誤

    public Animator Player_Animator;
    DataManger Data_Manger;
    SavePoint Save_Point;
    PlayerUI PUI;
    WaterValueChange WVC;
    BillBoard BB;
    CameraControl CC;
    Cinemachine_Ctrl Cinemachine_C;
    BGMManager BGMM;
    PlayerSE PSE;
    CamTrigger CT;
    GameObject DodgeDetector;

    public Scene ThisScene;

    //effect
    public ParticleSystem running_Dust;
    public ParticleSystem shower_Drop;
    public GameObject SwordLight;
    public GameObject ParticleEffect_RecoverFog;
    public ParticleSystem waterSpain;

    public GameObject BearModel;
    public GameObject FatBearModel;


    [Header("熊移動軸向")]
    public float Move_x = 1;
    public float Move_y = 0;
    public float Move_z = 1;
    public float BearModel_y = -50;
    public float FatBearModel_y = -60;
    public string NowCamTrack = "CM main";
    public float ForceFixForward = 1;
    public float ForceFixRight = 1;
    public CinemachineBrain CB;
    

    private void Start()
    {
        Player_Animator = GetComponent<Animator>();
        Data_Manger = FindObjectOfType<DataManger>();
        Save_Point = FindObjectOfType<SavePoint>();
        PUI = FindObjectOfType<PlayerUI>();
        WVC = FindObjectOfType<WaterValueChange>();
        BB = FindObjectOfType<BillBoard>();
        CC = FindObjectOfType<CameraControl>();
        DeadBlockSight = GameObject.Find("DeadBlockSight");
        BGMM = FindObjectOfType<BGMManager>();
        PSE = FindObjectOfType<PlayerSE>();
        CB = FindObjectOfType<CinemachineBrain>();
        CT = FindObjectOfType<CamTrigger>();
        DodgeDetector = GameObject.Find("DodgeDetector");


        ThisScene = SceneManager.GetActiveScene();

        DOTween.SetTweensCapacity(1750, 700);

        //Debug.Log(Application.dataPath);
        WindElementCoolTime = DefaultWindElementCoolTime;

        Data_Manger.LoadFileFunction();
        LoadPlayerData();

        CheckSceneToChangeValue();

        //Debug.Log("HPInCon:" + Hp);
        // Debug.Log(Application.dataPath);
        // Debug.Log(int.Parse(Data_Manger.InfoAll[0].ToString()));
        //我只是用來測試的

        

    }//end Start

    private void Update()
    {
        GamePauseChecker();
        EscPress();
    }// End Update TimeSalce don't Effected

    void FixedUpdate()//固定頻率重複執行
    {

        if (IsStriked) {
            Vector3 TargetPos = new Vector3(TheObjGiveStrike.transform.position.x, this.gameObject.transform.position.y,TheObjGiveStrike.transform.position.z);
            this.gameObject.transform.LookAt(TargetPos);
            this.gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
        }
       


        Move();
        CheckIsMovementEnable();//檢查能不能行動
        Select();//選擇道具
        if (IsAtking) {
            Ani_Time();//動畫重置用
            if ((input_V != 0 && input_H != 0) || input_V != 0 || input_H != 0) {
                angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);
            }
        }
        CheckPlayerDead();
        BearStateCheck();
        FixAngle();
        FatValueCheck();
        PlayerInvincibleCheck();
        WindElementGetCoolTime();
        CheatCode();
        //angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);
        //FixTurning();
        

    }//end FixedUpdate TimeScale Effected

    void GamePauseChecker() {
        if (Save_Point.IsSavePointOpened || IsEscMenu || IsUIEventing || IsTeaching)
        {
            Time.timeScale = 0f;
        }
        else if(!Save_Point.IsSavePointOpened && !IsEscMenu){
            Time.timeScale = 1f;
        }
    }

    public void CurrentModelChange(int NowBear) {
        if (NowBear == 0)
        {
            ModelArray[1].SetActive(false);
            Player_Animator.avatar = AvatarArray[0];
            var em = shower_Drop.emission;
            em.enabled = false;
            CurrentModelNum = 0;
        }
        else {
            ModelArray[0].SetActive(false);
            Player_Animator.avatar = AvatarArray[1];
            var em = shower_Drop.emission;
            em.enabled = true;
            CurrentModelNum = 1;
        }
        ModelArray[NowBear].SetActive(true);

        IsPanting = false;
        IsJumping = false;
        IsInvincible = false;
        IsHolding = false;
        IsStriked = false;
        IsDrinking = false;
        IsDodging = false;
        IsAtking = false;
     
    }

    void CheckIsMovementEnable() {
        if (IsUIEventing || IsStriked || IsDrinking || IsEscMenu)
        {
            Player_Animator.SetBool("PlayerRun", false);
        }
        else {
            AtkInteract();//攻擊或互動
            UseItem();//使用道具
            Dodge();//閃躲
            Jump();//跳躍
        }
    }// 檢查主角能不能移動

    string LastCMTrack = "CM main";

    void CamFixV() {
        if (NowCamTrack.Equals("CM main"))
        {
            Move_x = 1;
            Move_y = 0;
            Move_z = 1;
            BearModel_y = 310;
            FatBearModel_y = 300;
            ForceFixForward = 1;
            ForceFixRight = 1;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM track1"))
        {
            Move_x = 1;
            Move_y = 0;
            Move_z = -1;
            BearModel_y = 50;
            FatBearModel_y = 50;
            ForceFixForward = -1;
            ForceFixRight = 1;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM dollytrack2"))
        {
            Move_x = 1.2f;
            Move_y = 0;
            Move_z = 0.8f;
            BearModel_y = -50;
            FatBearModel_y = -60;
            ForceFixForward = 1;
            ForceFixRight = 1;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM dollytrack3"))
        {
            Move_x = 1f;
            Move_y = 0;
            Move_z = 1f;
            BearModel_y = -50;
            FatBearModel_y = -50;
            ForceFixForward = 1;
            ForceFixRight = 1;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM track4"))
        {
            Move_x = 1f;
            Move_y = 0;
            Move_z = 1f;
            BearModel_y = -50;
            FatBearModel_y = -50;
            ForceFixForward = 1;
            ForceFixRight = 1;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM track5"))
        {
            Move_x = 1.3f;
            Move_y = 0;
            Move_z = 0.7f;
            BearModel_y = -20;
            FatBearModel_y = -20;
            ForceFixForward = 0.7f;
            ForceFixRight = 1.3f;
            LastCMTrack = NowCamTrack;
        }

        if (NowCamTrack.Equals("CM dollytrack6"))
        {
            Move_x = 1.3f;
            Move_y = 0;
            Move_z = 0.7f;
            BearModel_y = -20;
            FatBearModel_y = -20;
            ForceFixForward = 0.6f;
            ForceFixRight = 1.4f;
            LastCMTrack = NowCamTrack;
        }

        //Debug.Log(BearModel.transform.eulerAngles);
            BearModel.transform.DOLocalRotate(new Vector3(0, BearModel_y, 0), 1f);
            FatBearModel.transform.DOLocalRotate(new Vector3(0, FatBearModel_y, 0), 1f);
            
        
    }


    void BearStateCheck() {



        if (ThisScene.name.EndsWith("2-1"))
        {
            NowCamTrack = CB.ActiveVirtualCamera.Name;
            if (!NowCamTrack.Equals(LastCMTrack))
            {
                CamFixV();
            }
        }
        else {

        }



        if (IsFast) {
            FastTime -= Time.deltaTime;

            if (FastTime <= 0) {
                IsFast = false;
                FastTime = 3f;
                speed = DefaultSpeed;
                Player_Animator.SetBool("PlayerFastRun", false);
            }
        }
        
        

        //Debug.Log(NowCamTrack);




        if (IsFat)
        {
            speed = FatSpeed;
            JumpSpeed = FatJumpSpeed;
            Player_Animator.SetBool("PlayerFat", true);
            CheckPlayerIsPanting();
            PlayerFatPressDownCheck();
        }
        else if(!IsFast && !IsFat){
            speed = DefaultSpeed;
            JumpSpeed = DefaultJumpSpeed;
            Player_Animator.SetBool("PlayerFat", false);
            IsPanting = false;
            RunTime = 0;
        }
        

    }

    void CheckPlayerIsPanting() {
        if (!IsJumping && IsFat && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)) {
            RunTime += Time.deltaTime;
        }
        else if ((IsFat && (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0))|| IsJumping) {
            RunTime -= Time.deltaTime*0.4f;
        }

        if (RunTime >= 3f)
        {
            Player_Animator.SetBool("PlayerPanting", true);
            IsPanting = true;
        }
        else if (RunTime <= 0) {
            RunTime = 0;
        }

    }
   
    IEnumerator LoadGameLastCheckPoint() {
       
        yield return new WaitForSeconds(8f);
        /* DeadBlockSight.GetComponent<Image>().DOFade(1, 1.5f);
         DeadBlockSight.transform.GetChild(0).GetComponent<Text>().DOFade(1, 1.5f);
         DeadBlockSight.transform.GetChild(1).GetComponent<Text>().color = new Color(255, 255, 255, 1);
         DeadBlockSight.transform.GetChild(1).GetComponent<Text>().DOFade(1, 1.5f);*/       
        DeadBlockSight.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        //IsInCinema = true;

        yield return new  WaitForSecondsRealtime(3f);

        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.K)))
        {
            LoadPlayerData();
            for (int i = 1; i <= Hp; i++)
            {
                PUI.CottonHPArray[i].GetComponent<Image>().DOFade(1, 0.5f);
            }
            CC.RecoverCamera();
            PUI.transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            Player_Animator.SetBool("PlayerDead", false);
            DeadBlockSight.transform.GetChild(0).GetComponent<Text>().text = "";
            DeadBlockSight.transform.GetChild(1).GetComponent<Text>().text = "";
            DeadBlockSight.transform.GetChild(2).GetComponent<Text>().text = "Loading...";
            DeadBlockSight.transform.GetChild(1).GetComponent<Text>().color = new Color(255,255,255,1);
            BGMM.cam_audi.Stop();
            PSE.SE.mute = true;
            
            if (ThisScene.name.Equals("K")) {
                for (int i = 1; i < MaxHp; i++)
                {
                    if (Hp < MaxHp) {
                        Hp += 1;
                    }
                    PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(1, 0f);
                    Debug.Log("+HPByReLoad");
                }
            }




            
            


            yield return new WaitForSecondsRealtime(2f);
            DeadBlockSight.GetComponent<CanvasGroup>().DOFade(0, 0.5f);

            if (ThisScene.name.Equals("2-1")) {
                BGMM.ChangeToForest();
            }
            if (ThisScene.name.Equals("K")) {
                BGMM.ChangeToBoss();
                BA.Restart_Pause = false;
            }

            PSE.SE.mute = false;
            if (ThisScene.name.Equals("2-1")) {
                CT.Recover_Cine();
            }
            NowCamTrack.Equals("CM main");
            StopCoroutine("LoadGameLastCheckPoint");
        }//在這裡撰寫玩家死亡時的行動
    }

    void LoadPlayerData()
    {
        IsPanting = false;
        IsDead = false;
        IsFat = false;
        IsFating = false;
        //IsInCinema = false;
        CurrentModelChange(0);
        ExpPoint = int.Parse(Data_Manger.InfoAll[2].ToString());//一開始讀取文檔中的數值
        Hp = int.Parse(Data_Manger.InfoAll[0].ToString());
        WaterElement = int.Parse(Data_Manger.InfoAll[3].ToString());
        WindElement = int.Parse(Data_Manger.InfoAll[4].ToString());
        //讀取位置


        if (ThisScene.name.Equals("K"))
        {
            this.gameObject.transform.position = GameObject.Find("RespawnPoint").transform.position;
        }
        else {
            this.transform.position = new Vector3(float.Parse(Data_Manger.InfoAll[5].ToString()), float.Parse(Data_Manger.InfoAll[6].ToString()), float.Parse(Data_Manger.InfoAll[7].ToString()));
        }


        //讀取位置

        //讀取ExpSystem
        MaxHp = int.Parse(Data_Manger.InfoAll[1].ToString());
        //讀取ExpSystem

    }

    /// <summary>
    /// 復活使用上方IEnumerator來復活
    /// </summary>
    /// <returns></returns>


    void CheckPlayerDead() {

        if (Hp <= 0)
        {
            IsDead = true;
            CC.Shaked = true;
            Player_Animator.SetBool("PlayerDead", true);
            WVC.IsLosingWater = false;
            if (ThisScene.name.Equals("K")) {
                BA.Restart_Pause = true;
            }
            PUI.transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.5f);
            if (IsDead)
            {
                DeadBlockSight.transform.GetChild(0).GetComponent<Text>().text = "Game Over";
                DeadBlockSight.transform.GetChild(1).GetComponent<Text>().text = "按下A鍵回到上個存檔點";
                DeadBlockSight.transform.GetChild(2).GetComponent<Text>().text = "";
                StartCoroutine("LoadGameLastCheckPoint");
            }
        }
        
    }

    void CheatCode() {
        if (ThisScene.name.Equals("2-1") && Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.U) && Input.GetKeyDown(KeyCode.N)) {
            this.gameObject.transform.position = GameObject.Find("FastPoint").transform.position;
        }

        if (ThisScene.name.Equals("2-1") && Input.GetKeyDown(KeyCode.O) && Input.GetKeyDown(KeyCode.P)) {
            this.gameObject.transform.position = GameObject.Find("RePosPoint").transform.position;
        }
        if (ThisScene.name.Equals("K") && Input.GetKeyDown(KeyCode.O) && Input.GetKeyDown(KeyCode.P))
        {
            this.gameObject.transform.position = GameObject.Find("RePosPoint").transform.position;
        }
        if (ThisScene.name.Equals("K") && Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.P))
        {
            
            for (int i = 1; i < MaxHp; i++) {
                Hp += 1;
                PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(1, 0f);
                Debug.Log("+Hp By Cheat");
            }
        }

    }

    void Ani_Time() {
        if (AniTime >= AniResetTime)
        {
            AniTime = 0;
            AniAtkCount = 0;
            Player_Animator.SetInteger("AtkCount", AniAtkCount);
            IsAtking = false;
        }
        else {
            AniTime += Time.fixedDeltaTime;
        }
    }



    void FatValueCheck()
    {
        if (IsFat) {        
            if (WVC.ImgWaterValue <= 0) {
                WVC.ImgWaterValue = 0;
                IsFat = false;
                WVC.IsLosingWater = false;
                CurrentModelChange(0);
                GameObject a = Instantiate(ParticleEffect_RecoverFog, this.transform);
                
                Destroy(a, 1.0f);
            }           
        }
       
    }

    

    bool IsCanMove() {

        if (!Player_Animator.GetBool("PlayerUseWind") && !Player_Animator.GetBool("PlayerStrikeDown") && !IsInCinema && !IsInCannon && !IsStriked && !Save_Point.IsSavePointOpened && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && !IsAtking && !IsHolding && !IsDodging && !IsDead && !IsUIEventing && !IsTurning && !IsDrinking && !IsPanting && !Player_Animator.GetBool("PlayerPress") && !IsFating && !IsEscMenu)
        {
            return true;
        }
        else{
            return false;
        }

           
    }

    void Move() {
        //接Input.GetAxis("Vertical")及("Horizontal")的回傳值
        input_V = Input.GetAxis("Vertical");
        input_H = Input.GetAxis("Horizontal");
        var em = running_Dust.emission;
        //用GetAxisRaw判斷是否按到移動鍵，是的話執行以下程式，放開可以保留角度狀態，也能避免NaN的狀況
        if (IsCanMove())
        {
            //Debug.Log(transform.eulerAngles.y);
            // eulerAngles 前45 右135 後-135 左-45
            // angle_Sum 前0 右90 後 180 左 -90

            //三角函數計算
            //angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle_Sum+45, transform.eulerAngles.z);

            //角色轉向 
            Turning();
          
            Player_Animator.SetBool("PlayerRun", true);

            if (IsGround && !IsUIEventing)
            {
                em.enabled = true;
            }
            else
            {
                em.enabled = false;
            }
            if (IsFat) {
                var Force_em = shower_Drop.forceOverLifetime;
                Force_em.enabled = true;              
            }
        }
        else
        {
            Player_Animator.SetBool("PlayerRun", false);
            // 胖時跑步雨滴+force
            em.enabled = false;
            var Force_em = shower_Drop.forceOverLifetime;
            Force_em.enabled = false;
        }
        if (input_H == 0 && input_V == 0 && IsFat)
        {
            var waterSpain_em = waterSpain.emission;
            waterSpain_em.enabled = true;
        }
        else {
            var waterSpain_em = waterSpain.emission;
            waterSpain_em.enabled = false;
        }
    }

    void CheckSceneToChangeValue() {

        if (ThisScene.name.Equals("2-1"))
        {
            BearModel.transform.eulerAngles = new Vector3(0,-60,0);
            FatBearModel.transform.eulerAngles = new Vector3(0, -60, 0);
            Hp = 3;
            PUI.MaxHP = 3;
            ExpPoint = 100;
            WaterElement = 10;
            WindElement = 3;
            this.transform.position = new Vector3(16.76f,10.8f,4.53f);
            Data_Manger.DeleteFile(Application.dataPath + "/Save", "Save.txt");
            Data_Manger.SaveFile(Application.dataPath + "/Save", "Save.txt");
        }
        else if (ThisScene.name.Equals("K")) {
            BearModel.transform.eulerAngles = new Vector3(0, -10, 0);
            FatBearModel.transform.eulerAngles = new Vector3(0, -10, 0);
            this.gameObject.transform.position = new Vector3(75, 0, 0);

            Hp = int.Parse(Data_Manger.InfoAll[0].ToString());
            PUI.MaxHP = int.Parse(Data_Manger.InfoAll[1].ToString());
            MaxHp = int.Parse(Data_Manger.InfoAll[1].ToString());
            
            

            DefaultSpeed = -3;
            speed = -3;
            Data_Manger.DeleteFile(Application.dataPath + "/Save", "Save.txt");
            Data_Manger.SaveFile(Application.dataPath + "/Save", "Save.txt");
        }

    }

    public void Turning() {
        Player_Animator.SetBool("PlayerRun", false);
        angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);

        Vector3 dir = new Vector3(input_H,0,input_V);
        Quaternion quadir = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, quadir, Time.fixedDeltaTime * 10f);
        //角色前進      

        if (ThisScene.name.Equals("2-1"))
        {

            /*transform.Translate(Vector3.forward * Time.deltaTime * speed, this.transform);
            transform.Translate(Vector3.right * Time.deltaTime * speed, this.transform);*/

            transform.Translate(new Vector3(Move_x,Move_y,Move_z) * Time.deltaTime * speed, this.transform);             

            /*this.GetComponent<Rigidbody>().AddForce(this.transform.forward*-100f);
            this.GetComponent<Rigidbody>().AddForce(this.transform.right*-100f);*/



            //this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            //transform.Translate(Vector3.right * Time.deltaTime * speed, this.transform);
        }
        else if (ThisScene.name.Equals("K")) {
            transform.Translate(Vector3.right * Time.deltaTime * speed, this.transform);
        }

    }

    public void FixTurning()
    {
       // Player_Animator.SetBool("PlayerRun", false);
        //angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);

        Vector3 dir = new Vector3(input_H, 0, input_V);
        Quaternion quadir = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, quadir, Time.fixedDeltaTime * 10f);
        //角色前進
        transform.Translate(Vector3.forward * Time.deltaTime * 0.1f, this.transform);
        transform.Translate(Vector3.right * Time.deltaTime * 0.1f, this.transform);
    }



    void FixAngle() {
        if (angle_Sum >= 180)
        {
            angle_Sum = 180;
        }
        if (angle_Sum <= -180) {
            angle_Sum = -180;
        }
    }

    void AtkInteract() {
        if (!IsInCinema && IsGround && !Save_Point.IsSavePointOpened && !IsFat && !IsDead && !NearItem && (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))) {
            Debug.Log("X或J被按下");//攻擊或互動
            if (!NearItem && AniAtkCount < 3)
            {
               
                AniTime = 0;
                AniAtkCount++;
                Player_Animator.SetInteger("AtkCount",AniAtkCount);
                Player_Animator.SetBool("PlayerAtk",true);
                IsAtking = true;

            }

        }



    }// end AtkInteract

    

    void EscPress() {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)) && !Save_Point.IsSavePointOpened)
        {
            IsEscMenu = !IsEscMenu;
            PUI.SystemIsOpen = false;
        }
        
    }// End EscPress

    void Jump()
    {
        var em = running_Dust.emission;
        
        if (!IsInCinema && !Save_Point.IsSavePointOpened && !IsDead && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.K))&& IsGround && !IsJumping && !IsHolding && !IsDodging && !IsAtking)
        {
            //GetComponent<Rigidbody>().AddForce(0, JumpSpeed, 0);
            Player_Animator.SetTrigger("PlayerJump");
            IsJumping = true;
            Debug.Log("A或K被按下");//跳躍
           
            em.enabled = false;
            
        }
    }// end Jump

    void Dodge()
    {
        var em = running_Dust.emission;
        if (!IsInCinema && IsGround && !Save_Point.IsSavePointOpened && !IsJumping && !IsFat && !IsDead && ((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.L)) || ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.L))))&& !IsDodging && !IsHolding)
        {
            Debug.Log("B或L被按下");//翻滾
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            Player_Animator.SetTrigger("PlayerDodge");
            IsDodging = true;
            IsInvincible = true;
            if (IsGround)
            {
                em.enabled = true;
            }
            else
            {
                em.enabled = false;
            }
            
        }
       

    }//end Dodge

    IEnumerator DelayIsFat() {
        IsFating = true;
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i <= 100; i++) {
            WVC.ImgWaterValue = i;
            yield return new WaitForSeconds(0.0025f);
        }
        //IsInvincible = true;
        IsFating = false;
        IsFat = true;
        CurrentModelChange(1);
        WVC.IsLosingWater = true;
        //IsInvincible = false;
    }

    IEnumerator Item2ColdDown() {
        yield return new WaitForSeconds(3f);
        Item2IsColdDown = false;
        StopCoroutine("Item2ColdDown");
    }

    void WindElementGetCoolTime() {
        if (WindElement < 3) {
            WindElementCoolTime -= Time.deltaTime;
            if (WindElementCoolTime <= 0) {
                WindElementCoolTime = DefaultWindElementCoolTime;
                WindElement++;
            }
        }
        
    }

    void UseItem() {
        if (!Save_Point.IsSavePointOpened && !IsDead && IsGetItem && !IsHolding)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Y或I被按下");
                if (Item_Type == 1 && WaterElement > 0 && !IsFat && !IsJumping && IsGround && !IsFating)
                {
                    Debug.Log("Item1_Use");
                    Player_Animator.SetBool("PlayerDrink",true);
                    if (!IsFat)
                    {
                        StartCoroutine("DelayIsFat");
                    }
                    else if (IsFat) {
                        IsFat = false;
                        StartCoroutine("DelayIsFat");
                    }
                    
                }
                else if (Item_Type == 2 && WindElement > 0 && !Item2IsColdDown && !IsFating)
                {
                    Debug.Log("Item2_Use");

                    Player_Animator.SetBool("PlayerUseWind",true);
                    WindElement -= 1;
                    //Instantiate(Tornado, - this.transform.forward - this.transform.right + this.transform.position + new Vector3(0,1f,0),Quaternion.identity);
                    //Instantiate(Tornado, DodgeDetector.transform.position - DodgeDetector.transform.forward - DodgeDetector.transform.right + new Vector3(0,1,0), Quaternion.identity);

                    Item2IsColdDown = true;                  

                    StartCoroutine("Item2ColdDown");
                }
                else
                {
                    Debug.Log("No Item");
                }

            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Y或I被按下但沒有道具可以使用");
            }           
        }

    }//end UseItem
    void Select() {
        if (!IsDead && IsGetItem)
        {
            if ((Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Q)) && !PUI.ChangingItemUI )
            {
                if (Item_Type <= 1)
                {
                    Item_Type = 2;
                }
                else {
                    Item_Type--;
                }
                PUI.ChangeItem(Item_Type,1);
               // Debug.Log("LB或Q左切道具" + "目前道具為" + Item_Type);
            }
            if ((Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.E)) && !PUI.ChangingItemUI)
            {
                if (Item_Type >= 2)
                {
                    Item_Type = 1;
                }
                else {
                    Item_Type++;
                }
                PUI.ChangeItem(Item_Type,2);
                //Debug.Log("RB或E右切道具" + "目前道具為" + Item_Type);
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("LB或Q左切道具但沒有道具可以切換");
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("RB或E右切道具但沒有道具可以切換");
            }
        }

    }//end select


    void PlayerInvincibleCheck() {
        if (IsInvincible && InvincibleTime > 0)
        {           
            InvincibleTime -= Time.fixedDeltaTime;
        }
        else if(InvincibleTime <= 0){
            IsInvincible = false;
            InvincibleTime = DefaultInvincibleTime;
        }
    }

    void PlayerFatPressDownCheck()
    {
        if (IsFat && IsJumping && (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))) {
            Player_Animator.SetBool("PlayerPress",true);
        }
    }

    /*
       Joystick1Button0 A
       Joystick1Button1 B
       Joystick1Button2 X 
       Joystick1Button3 Y
       Joystick1Button4 LB
       Joystick1Button5 RB
       Joystick1Button LT
       Joystick1Button RT
       Joystick1Button6 Back
       Joystick1Button7 Start
       Joystick1Button8 L3
       Joystick1Button9 R3
     */

    public Collider TheObjGiveStrike;

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("canon")) {
            OnTop = true;
        }

        if (other.CompareTag("WolfAtkBox") && !IsInvincible) {
            TheObjGiveStrike = other;
            Player_Animator.SetBool("PlayerPress",false);
            PlayerAnimationBoolChecker();
            if (Hp > 0) {
                PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(0,0.5f);
            }

            if (Hp > 0 && !IsStriked && Hp != 1) {
                //transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, other.transform.rotation, Time.deltaTime * 15f);
                Player_Animator.SetTrigger("PlayerStriked");
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting",false);
                RunTime = 0;              
                Hp--;
                IsInvincible = true;
                
            }
            else if (Hp > 0 && !IsStriked && Hp == 1) {
              
                Hp--;

                IsPanting = false;           
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;

                Player_Animator.SetBool("PlayerDead", true);
            }
            GameObject sl = Instantiate(SwordLight, other.gameObject.transform);
            sl.transform.SetParent(null);
            sl.transform.localScale = new Vector3(5f,5f,5f);
            Destroy(sl, 1.0f);
        }// With Wolf

        /*if (other.CompareTag("")) {
            Player_Animator.SetTrigger("PlayerStrikeDown");
        }*/

        if (other.CompareTag("Boss_Smash") && !IsInvincible)
        {
            TheObjGiveStrike = other;
            Player_Animator.SetBool("PlayerPress", false);
            PlayerAnimationBoolChecker();
            if (Hp > 0)
            {
                PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(0, 0.5f);
            }

            if (Hp > 0 && !IsStriked && Hp != 1)
            {
                Player_Animator.SetTrigger("PlayerStrikeDown");
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;
                Hp--;
                IsInvincible = true;
            }
            else if (Hp > 0 && !IsStriked && Hp == 1)
            {
                Hp--;
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;

                Player_Animator.SetBool("PlayerDead", true);
            }
        }
        if (other.CompareTag("Boss_Slash") && !IsInvincible)
        {
            TheObjGiveStrike = other;
            Player_Animator.SetBool("PlayerPress", false);
            PlayerAnimationBoolChecker();
            if (Hp > 0)
            {
                PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(0, 0.5f);
            }

            if (Hp > 0 && !IsStriked && Hp != 1)
            {
                Player_Animator.SetTrigger("PlayerStrikeDown");
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;
                Hp--;
                IsInvincible = true;
            }
            else if (Hp > 0 && !IsStriked && Hp == 1)
            {
                Hp--;
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;

                Player_Animator.SetBool("PlayerDead", true);
            }
        }
        if (other.CompareTag("Boss_Normal") && !IsInvincible)
        {
            TheObjGiveStrike = other;
            Player_Animator.SetBool("PlayerPress", false);
            PlayerAnimationBoolChecker();
            if (Hp > 0)
            {
                PUI.CottonHPArray[Hp].GetComponent<Image>().DOFade(0, 0.5f);
            }

            if (Hp > 0 && !IsStriked && Hp != 1)
            {              
                Player_Animator.SetTrigger("PlayerStriked");
                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;
                Hp--;
                IsInvincible = true;
            }
            else if (Hp > 0 && !IsStriked && Hp == 1)
            {
                Hp--;

                IsPanting = false;
                Player_Animator.SetBool("PlayerPanting", false);
                RunTime = 0;

                Player_Animator.SetBool("PlayerDead", true);
            }
        }

        if (other.gameObject.tag == "Lava" && IsBurn == false)
        {
            StartCoroutine("Burning");
        }




    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canon"))
        {
            OnTop = false;
            //BA.BackToLow = true;
        }

        if (other.gameObject.tag == "Lava" && IsBurn == false)
        {
            StopCoroutine("Burning");
            IsBurn = false;
        }

    }

    private IEnumerator Burning()
    {
        IsBurn = true;
        Hp--;
        Debug.Log(Hp);
        Player_Animator.SetTrigger("PlayerStriked");
        yield return new WaitForSeconds(2f);
        IsBurn = false;
    }

    void PlayerAnimationBoolChecker() {
        if (IsDrinking)
        {
            WVC.DefaultValue();
            StopCoroutine("DelayIsFat");
            IsFating = false;
            IsFat = false;
        }      

        

    }// handle bug situation
    
}
