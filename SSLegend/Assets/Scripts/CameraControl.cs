using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cinemachine;



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
    //SHAKE--
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private CinemachineBrain CB;
    public CinemachineVirtualCamera[] VirtualCamera;
    private CinemachineBasicMultiChannelPerlin[] virtualCameraNoise = {null, null, null, null, null, null, null };



    void Start()
    {
        _Pctrl = FindObjectOfType<PlayerControl>();
        pp_Volume = gameObject.GetComponent<PostProcessVolume>();
        pp_Volume.profile.TryGetSettings(out p_Vignette);
        pp_Volume.profile.TryGetSettings(out p_ColorGrading);
        pp_Volume.profile.TryGetSettings(out p_depthOfField);
        cinemachineCtrl = FindObjectOfType<Cinemachine_Ctrl>();
        CB = GetComponent<CinemachineBrain>();
        for (int i = 0; i < VirtualCamera.Length; i++) {
            if (VirtualCamera[i] != null)
                virtualCameraNoise[i] = VirtualCamera[i].GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

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
           // StartCoroutine(CameraShake(ShakeDuration, ShakeAmplitude, ShakeFrequency));
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
            //StartCoroutine(CameraShake(ShakeDuration, ShakeAmplitude, ShakeFrequency));
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
    
    IEnumerator CameraShake(float Duration, float Amplitude,float Frequency)
    {

        if (CB.ActiveVirtualCamera.Name.Equals("CM main"))
        {
            if (VirtualCamera[0] != null && virtualCameraNoise[0] != null)
            {
                //Set  
                virtualCameraNoise[0] = VirtualCamera[0].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[0].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[0].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[0].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM track1"))
        {
            if (VirtualCamera[1] != null && virtualCameraNoise[1] != null)
            {
                //Set  
                virtualCameraNoise[1] = VirtualCamera[1].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[1].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[1].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[1].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM dollytrack2"))
        {
            if (VirtualCamera[2] != null && virtualCameraNoise[2] != null)
            {
                //Set  
                virtualCameraNoise[2] = VirtualCamera[2].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[2].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[2].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[2].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM dollytrack3"))
        {
            if (VirtualCamera[3] != null && virtualCameraNoise[3] != null)
            {
                //Set  
                virtualCameraNoise[3] = VirtualCamera[3].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine 3amera Noise parameters
                virtualCameraNoise[3].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[3].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[3].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM track4"))
        {
            if (VirtualCamera[4] != null && virtualCameraNoise[4] != null)
            {
                //Set  
                virtualCameraNoise[4] = VirtualCamera[4].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[4].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[4].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[4].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM track5"))
        {
            if (VirtualCamera[5] != null && virtualCameraNoise[5] != null)
            {
                //Set  
                virtualCameraNoise[5] = VirtualCamera[5].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[5].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[5].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[5].m_AmplitudeGain = 0f;


            }
        }

        if (CB.ActiveVirtualCamera.Name.Equals("CM dollytrack6"))
        {
            if (VirtualCamera[6] != null && virtualCameraNoise[6] != null)
            {
                //Set  
                virtualCameraNoise[6] = VirtualCamera[6].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise[6].m_AmplitudeGain = Amplitude;
                virtualCameraNoise[6].m_FrequencyGain = Frequency;
                yield return new WaitForSeconds(Duration);

                // If Camera Shake effect is over, reset variables
                virtualCameraNoise[6].m_AmplitudeGain = 0f;


            }
        }

        
    }
    public void shake() {
        StartCoroutine(CameraShake(ShakeDuration,ShakeAmplitude,ShakeFrequency));
    }

    

}