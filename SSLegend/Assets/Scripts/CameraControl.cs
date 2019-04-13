using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;



public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float rotatesmooth = 0.125f;
    float timeScale_limit = 0.0f;
    bool touchLimit = false;
    float timetoGray = 3.0f;
    public float[] levels;  
    //private float targetY;
    public Vector3 offset_b;
    public Vector3 offset_k;
    private bool Shaked = false;
    public bool hasPlayTrack;
    // Use this for initialization
    private PlayerControl _Pctrl;
    PostProcessVolume pp_Volume;
    Vignette p_Vignette;
    ColorGrading p_ColorGrading;
    DepthOfField p_depthOfField;
    private Cinemachine_Ctrl cinemachineCtrl;
    

    void Start()
    {
        _Pctrl = FindObjectOfType<PlayerControl>();
        pp_Volume = gameObject.GetComponent<PostProcessVolume>();
        pp_Volume.profile.TryGetSettings(out p_Vignette);
        pp_Volume.profile.TryGetSettings(out p_ColorGrading);
        pp_Volume.profile.TryGetSettings(out p_depthOfField);
        cinemachineCtrl = FindObjectOfType<Cinemachine_Ctrl>();
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            offset = new Vector3(7.5f, 9.0f, 0.5f);
        }
        else
        {
            offset = new Vector3(1.5f, 4.1f, 2.5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //    hasPlayTrack = true;
        //    cinemachineCtrl.TrackStart = true;
        //}

        if (target.gameObject.tag == "Player" && !hasPlayTrack)
        {
            if (_Pctrl.IsDead)
            {
                Died();
            }
            else
            {
                if (_Pctrl.IsStriked || _Pctrl.Player_Animator.GetBool("PlayerStrikeDown"))
                {
                    GetHurt();
                }
                else
                {
                    CommonMove();
                }
            }
            
        }
    }
    public void RecoverCamera()
    {
        float a = p_Vignette.intensity.value;
        a = 0f;
        p_Vignette.intensity.value = a;
        p_ColorGrading.saturation.value = 0f;
        p_depthOfField.focalLength.value = 0f;
    }
    void GetHurt()
    {
        if (!Shaked)
        {
            StartCoroutine(CameraShake(0.05f, 0.1f));
        }
        float a = p_Vignette.intensity.value;
        /*if (a < 0.5f)
        {
            a += 0.07f;
        }*/
        a = 0.5f;
        p_Vignette.intensity.value = a;

        float limit = 0.7f;

        if (Time.timeScale > limit)
        {
            Time.timeScale -= 0.05f;
        }

        
    }
    void Died()
    {

        //--shake screen
        if (!Shaked)
        {
            StartCoroutine(CameraShake(0.05f, 0.1f));
            float a = p_Vignette.intensity.value;
            a = 0.5f;
            p_Vignette.intensity.value = a;
            timeScale_limit = 0.5f;
        }
        //-- Vignette--
        float b = p_Vignette.intensity.value;
        if (b > 0)
        {
            b -= 0.1f * Time.deltaTime;
        }
        p_Vignette.intensity.value = b;
        //--delay world time--


        if (touchLimit)
        {
            if (Time.timeScale < 1.0f)
            {
                Time.timeScale += 0.05f;
            }
        }
        else
        {
            if (Time.timeScale > timeScale_limit)
            {
                Time.timeScale -= 0.05f;
            }
            else
            {
                touchLimit = true;
            }
        }

        //--Color grading-> Gray--
        timetoGray -= Time.deltaTime;
        if (timetoGray <= 0)
        {
            //-- Start depthOfField 
            p_depthOfField.active = true;
            if (p_ColorGrading.saturation.value > -100f)
            {

                p_ColorGrading.saturation.value -= 1f;
            }
            if (p_depthOfField.focalLength.value < 297f)
            {
                p_depthOfField.focalLength.value += 10.0f * Time.deltaTime;

            }
        }

    }
    void CommonMove()
    {
        //Vector3 tmp;
        Shaked = false;
        //if (target.position.y > levels[2])
        //{
        //    tmp = offset * 2 + offset / 2;
        //}
        //else if (target.position.y > levels[1])
        //{
        //    tmp = offset + offset / 2;
        //}
        //else
        //{
        //    tmp = offset;
        //}
        //LOOKAT
        //Vector3 deraction = target.position - this.transform.position;
        //this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (deraction),rotatesmooth);

        
        //Vector3 desiredPosition = target.position + tmp + offset / 2;
        //Vector3 smoothPosition = Vector3.Lerp(this.transform.position, desiredPosition, smoothSpeed);
        //this.transform.position = smoothPosition;

        float a = p_Vignette.intensity.value;
        if (a > 0)
        {
            a -= 0.01f;
        }
        p_Vignette.intensity.value = a;
    }
    
    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;
            float z = originalPos.z + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, z);
            elapsed += Time.deltaTime;

            yield return null;

        }
        transform.localPosition = originalPos;
        Shaked = true;
    }
    public void shake() {
        StartCoroutine(CameraShake(0.05f, 0.5f));
    }

    

}