using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour {
    public AudioSource cam_audi;
    public AudioClip deathBGM;
    public AudioClip forestBGM;
    public AudioClip BossBGM;
    

	// Use this for initialization
	void Start () {

        cam_audi = GetComponent<AudioSource>();
       
    }
	
	// Update is called once per frame
	void Update () {
        if (cam_audi.volume < 0.4) {
            cam_audi.volume += 0.001f;
        }
        
	}
    public void ChangeToForest() {
        cam_audi.Stop();
        cam_audi.clip = forestBGM;
        cam_audi.Play();
        
    }
    public void ChangeToDeath() {
        cam_audi.Stop();
        cam_audi.clip = deathBGM;
        cam_audi.volume = 0f;
        cam_audi.Play();
    }

    public void ChangeToBoss() {
        cam_audi.Stop();
        cam_audi.clip = BossBGM;
        cam_audi.volume = 0.4f;
        cam_audi.Play();
    }

}
