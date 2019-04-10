using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBlendShape : MonoBehaviour {

    int blendShapeCount;
    SkinnedMeshRenderer SMR;
    Mesh SkinnedMesh;
    float blendOne = 0f;
    float blendTwo = 0f;
    float blendSpeed = 1f;
    bool blendOneFinished = false;

    private void Awake()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        SkinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    // Use this for initialization
    void Start () {
        blendShapeCount = SkinnedMesh.blendShapeCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (blendShapeCount > 2) {

        }
	}
}
