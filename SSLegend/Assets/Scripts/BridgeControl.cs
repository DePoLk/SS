using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

public class BridgeControl : MonoBehaviour {

    public GameObject Bridge;

    [Header("音效")]
    public AudioSource AS;
    public AudioClip BridgeMoveSE;
    public AudioClip SwitchSE;
	// Use this for initialization
	void Start () {
        Bridge = GameObject.Find("Bridge");
	}
	
	// Update is called once per frame
	void Update () {
        if (Bridge.transform.localScale.z > 0 && Bridge.transform.localScale.z < 1)
        {

        }
        else if(Bridge.transform.localScale.z >= 1 || Bridge.transform.localScale.z <= 0){
            AS.Stop();
        }
    }

    IEnumerator PlaySound() {
        AS.Stop();
        AS.clip = SwitchSE;
        //AS.Play();
        yield return new WaitForSeconds(0.8f);
        AS.Stop();
        AS.clip = BridgeMoveSE;
        //.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player")) {
            this.transform.DOLocalMoveY(-0.3f,0.25f);
            Bridge.transform.DOScaleZ(1, 8f).SetEase(Ease.InOutSine);
            StartCoroutine("PlaySound");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player")) {
            //播放橋梁連結音效
            //AS.clip = BridgeMoveSE;         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            this.transform.DOLocalMoveY(0f, 0.25f);
            Bridge.transform.DOScaleZ(0, 8f).SetEase(Ease.InOutSine);
            StartCoroutine("PlaySound");
        }
    }
}
