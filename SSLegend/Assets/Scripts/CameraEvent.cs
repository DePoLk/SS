using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraEvent : MonoBehaviour {

	private PlayerControl _Pctrl;
	private CameraControl _Cctrl;
    
    

	public enum MonsterType{None,Karohth,Wofka,Arubado,SolidGoddess}; 
	public MonsterType typeOfMon = MonsterType.Arubado;
	public Transform whichMon;
	private NormalMonster NMon;
    private Karohth kar;
    private bool hasLooked_Arubado = false;
    private bool hasLooked_Karohth = false;
    private bool hasLooked_SolidGoddess = false;
    private bool hasLooked_Wofka = false;
    void Start (){
		_Pctrl = FindObjectOfType<PlayerControl> ();
		_Cctrl = FindObjectOfType<CameraControl> ();
		NMon = FindObjectOfType<NormalMonster> ();
        
        kar = FindObjectOfType<Karohth>();
		whichMonster ();
	}
	void OnTriggerEnter(Collider a){
		if (a.gameObject.tag == "Player") {
            if (typeOfMon == MonsterType.Karohth) {
                if (hasLooked_Karohth == false)
                {
                    _Cctrl.target = whichMon;
                    //BGM.TurnInBattle();
                    kar.anim.SetInteger("firstattach", 1);
                    hasLooked_Karohth = true;

                }
            } else if (typeOfMon == MonsterType.Arubado) {
                if (hasLooked_Arubado == false)
                {
                   
                }
            }
            else if (typeOfMon == MonsterType.Wofka)
            {
                if (hasLooked_Wofka == false)
                {
                    
                    
                }
            }
            else if (typeOfMon == MonsterType.SolidGoddess)
            {
                if (hasLooked_SolidGoddess == false)
                {
                   

                }
            }
           


        }
	}
	void whichMonster(){
		Debug.Log (typeOfMon);
		if (typeOfMon == MonsterType.Arubado) {
			whichMon = GameObject.Find ("Arubado").transform;
		}
		else if(typeOfMon == MonsterType.Karohth){
			whichMon = GameObject.Find ("Karohth").transform;
		}else if(typeOfMon == MonsterType.Wofka){
			whichMon = GameObject.Find ("Wofka").transform;
		}else if(typeOfMon == MonsterType.SolidGoddess){
			whichMon = GameObject.Find ("SolidGoddess").transform;
		}

	}

}
