using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateToK : MonoBehaviour {

    LoadingControl Load;
    DataManger DM;
    int i=0;
	// Use this for initialization
	void Start () {
        Load = FindObjectOfType<LoadingControl>();
        DM = FindObjectOfType<DataManger>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player")&&i==0) {
            i += 1;
            DM.DeleteFile(Application.dataPath + "/Save", "Save.txt");
            DM.SaveFile(Application.dataPath + "/Save", "Save.txt");
            Load.ChangeScene();
        }
    }
}
