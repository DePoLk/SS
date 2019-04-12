using UnityEngine;
using System.Collections;
using Cinemachine;
//相机一直拍摄主角的后背
public class camteset : MonoBehaviour
{
    public CinemachineVirtualCameraBase cam1;
    public CinemachineVirtualCameraBase cam2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            change1to2();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            change2to1();
        }

    }

    public void change1to2() {
        cam1.VirtualCameraGameObject.SetActive(false);
        cam2.VirtualCameraGameObject.SetActive(true);
    }
    public void change2to1()
    {
        cam1.VirtualCameraGameObject.SetActive(true);
        cam2.VirtualCameraGameObject.SetActive(false);
    }

}
