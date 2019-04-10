using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;


public class BearConForTest : MonoBehaviour
{
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
    [Header("角色速度")]
    public float DefaultSpeed;
    public float FatSpeed = -1f;
    public float speed;
    public float FatJumpSpeed;
    public float DefaultJumpSpeed;
    public float JumpSpeed;
    public Animator Player_Ani;

    [Header("角色落地")]
    public bool IsGround = false;


    private void Start()
    {
        //我只是用來測試的
    }//end Start

    private void Update()
    {

    }// End Update TimeSalce don't Effected

    void FixedUpdate()//固定頻率重複執行
    {
        Move();
        CheckIsMovementEnable();//檢查能不能行動
        FixAngle();
        angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);

    }//end FixedUpdate TimeScale Effected




    void CheckIsMovementEnable()
    {
            Jump();//跳躍       
    }// 檢查主角能不能移動

   

   
    
    bool IsCanMove()
    {

        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    void Move()
    {
        //接Input.GetAxis("Vertical")及("Horizontal")的回傳值
        input_V = Input.GetAxis("Vertical");
        input_H = Input.GetAxis("Horizontal");
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
    
        }
        else
        {


        }
    }

    public void Turning()
    {
        angle_Sum = Mathf.Atan2(input_H, input_V) / (Mathf.PI / 180);

        Vector3 dir = new Vector3(input_H, 0, input_V);
        Quaternion quadir = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, quadir, Time.fixedDeltaTime * 10f);
        //角色前進
        transform.Translate(Vector3.forward * Time.deltaTime * speed, this.transform);
        transform.Translate(Vector3.right * Time.deltaTime * speed, this.transform);
    }



    void FixAngle()
    {
        if (angle_Sum >= 180)
        {
            angle_Sum = 180;
        }
        if (angle_Sum <= -180)
        {
            angle_Sum = -180;
        }
    }

    



    
    void Jump()
    {

        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.K)) && IsGround )
        {
         
            Debug.Log("A或K被按下");//跳躍
            Player_Ani.SetTrigger("Jump");
            this.GetComponent<Rigidbody>().AddForce(transform.up * JumpSpeed);
            IsGround = false;

        }
    }// end Jump


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

   

}
