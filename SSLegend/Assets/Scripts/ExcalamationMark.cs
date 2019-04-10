using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcalamationMark : MonoBehaviour {
    public enum MonType {Karohth,Wofga,Wofga_B };
    public MonType MarkStatus = MonType.Karohth;
    public Vector3 maxScale;
    private Vector3 minScale;
    private Vector3 currentScale;
    private float status;
    public float speed;
    public float duration;
    float i = 0.0f;
    float f = 0.0f;
    float Endtime;
    public float keepTime;
    public bool startMark = false;
    // Use this for initialization
    void Start () {
        minScale = new Vector3(0, 0, 0);
        if (MarkStatus == MonType.Karohth) {
            status = 0.009f;
        }
        if (MarkStatus == MonType.Wofga)
        {
            status = 0.2f;
        }
        if (MarkStatus == MonType.Wofga_B)
        {
           
        }
        maxScale = new Vector3(status, status, status);
    }
	
	// Update is called once per frame
	void Update () {
        if (startMark)
        {
            if (Endtime > keepTime)
            {
                EndScale();
            }
            else
            {
                StartToScale();
            }
        }
        else {
            Endtime = 0.0f;
            transform.localScale = new Vector3(status,0,0);
            i = 0.0f;
            f = 0.0f;
        }
        
       
	}

    void StartToScale() {
        currentScale = transform.localScale;
        //this.gameObject.transform.SetParent(null);

        float rate = 0.0f;
        
        if (i < 0.3) {
            rate = 1.0f / duration * speed * 2;
            maxScale.y += 0.0005f;
            maxScale.x += 0.00005f;
        } else if (i >= 0.3 && i<0.4) {
            rate = 1.0f / duration * speed;
            maxScale.y += 0.0005f;
            maxScale.x += 0.00005f;
        }
        else if(i >=0.4){
            rate = 1.0f / duration * speed;
           
            maxScale.y -= 0.0006f;
            maxScale.x -= 0.00005f;
            if (maxScale.x < status) {
                maxScale.x = status;
                
            }
            if (maxScale.y < status)
            {
                maxScale.y = status;
                Endtime += Time.deltaTime;

            }
        }
        //float rate = 1.0f / duration * speed;
       // Debug.Log(i);
        i += Time.deltaTime * rate;
       //
        

        transform.localScale = Vector3.Lerp(currentScale , maxScale , i );
     }

    void EndScale()
    {
        currentScale = transform.localScale;
        float rate = 0.0f;

        if (f < 0.3)
        {
            rate = 1.0f / duration * speed ;
            
        }
        else if (f >= 0.3)
        {
            rate = 1.0f / duration * speed ;

        }
       
         f += Time.deltaTime * rate;
         //Debug.Log(f);


         transform.localScale = Vector3.Lerp(currentScale, minScale, f);
        if (transform.localScale.x == 0 && transform.localScale.y == 0 && transform.localScale.z == 0)
        {
            startMark = false;
        }
    }
}
