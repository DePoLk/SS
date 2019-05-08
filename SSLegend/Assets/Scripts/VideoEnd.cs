using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour {
    double time;
    double currentTime;
    private bool ChangeScene = false;
    // Use this for initialization
    void Start() {
        time = this.gameObject.GetComponent<VideoPlayer>().clip.length;
        Debug.Log(time);
    }

    // Update is called once per frame
    void Update() {
        currentTime = gameObject.GetComponent<VideoPlayer>().time;
        if (time - currentTime <= 0.1f && ChangeScene == false) {
            StartCoroutine("StartGame");
        }
    }
    IEnumerator StartGame() {
        ChangeScene = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
}
