using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingControl : MonoBehaviour {
    public GameObject Fade;
    public GameObject bearIcon;
    public GameObject bearIconEdge;
    float ScaleStatus = 1.0f;
    bool Switch = false;
    float alpha = 1;
    float alpha_loading = 0;
    public bool isLoading = false;
    int count=0;
    // Use this for initialization
    void Start () {
        Fade.SetActive(true);
        bearIcon.SetActive(false);
        bearIconEdge.SetActive(false);
        Time.timeScale=1;
	}
	
	// Update is called once per frame
	void Update () {
        if (isLoading)
        {
            LoadAnim();
        }
        else {
            
            alpha -= 0.03f;
            var color = Fade.GetComponent<Image>().color;
            color.a = alpha;
            Fade.GetComponent<Image>().color = color;
            bearIcon.SetActive(false);
            bearIconEdge.SetActive(false);
        }
        
    }
    public void BackToMenu() {
        StartCoroutine(DisplayLoading(0));
    }
    public void ChangeScene() {
        if (SceneManager.GetActiveScene().buildIndex == 0 && count==0) {
            count++;
            StartCoroutine(DisplayLoading(1));
        } else if (SceneManager.GetActiveScene().buildIndex == 1 && count == 0) {
            count++;
            StartCoroutine(DisplayLoading(2));
            
        }
        
    }


    IEnumerator DisplayLoading(int i){
       
        Debug.Log("before yield");
        AsyncOperation async = SceneManager.LoadSceneAsync(i);
        async.allowSceneActivation = false;
        isLoading = true;
        yield return new WaitForSecondsRealtime(8);
        Debug.Log("After yield");

        ScaleStatus = 1.0f;
        Switch = false;
        alpha_loading = 0;
        async.allowSceneActivation = true;


    }


    public void LoadAnim()
    {

        alpha_loading += 0.05f;
        var color = Fade.GetComponent<Image>().color;
        color.a = alpha_loading;
        Fade.GetComponent<Image>().color = color;
        if (alpha_loading >= 1) {
            bearIcon.SetActive(true);
            bearIconEdge.SetActive(true);
            if (Switch)
            {

                ScaleStatus += 0.015f;
                if (ScaleStatus >= 1.7f)
                {
                    Switch = false;
                    ScaleStatus = 1.7f;
                }
            }
            else
            {
                ScaleStatus -= 0.015f;
                if (ScaleStatus <= 1f)
                {
                    Switch = true;
                    ScaleStatus = 1f;
                }
            }
        }
        
        //Debug.Log(ScaleStatus);
        Vector3 v = new Vector3(ScaleStatus, ScaleStatus, ScaleStatus);

        bearIconEdge.transform.localScale = v;
       
    }
        

}
