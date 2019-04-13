using UnityEngine;
using System.Collections;
using Cinemachine;
//相机一直拍摄主角的后背
public class CamTrigger : MonoBehaviour
{
    public enum TrackIdex {T_0,T_1,T_2,T_3,T_4,T_5,T_6};
    public TrackIdex trackIdex = TrackIdex.T_0;
    public CinemachineVirtualCameraBase[] cam;
    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            foreach (CinemachineVirtualCameraBase trackCam in cam)
            {
                trackCam.VirtualCameraGameObject.SetActive(false);

            }
            if (trackIdex == TrackIdex.T_0) {
               
                cam[0].VirtualCameraGameObject.SetActive(true);    
            }
            if (trackIdex == TrackIdex.T_1)
            {      
                cam[1].VirtualCameraGameObject.SetActive(true);
            }
            if (trackIdex == TrackIdex.T_2)
            {
                cam[2].VirtualCameraGameObject.SetActive(true);
            }
            if (trackIdex == TrackIdex.T_3)
            {
                cam[3].VirtualCameraGameObject.SetActive(true);
            }
            if (trackIdex == TrackIdex.T_4)
            {
                cam[4].VirtualCameraGameObject.SetActive(true);
            }
            if (trackIdex == TrackIdex.T_5)
            {
                cam[5].VirtualCameraGameObject.SetActive(true);
            }
            if (trackIdex == TrackIdex.T_6)
            {
                cam[6].VirtualCameraGameObject.SetActive(true);
            }

        }
    }
    public void OnTriggerExit(Collider other)
    {
        

    }

}
