using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CottonFlyAway : MonoBehaviour
{
    PlayerControl PlayerCon;

    Hashtable moveSetting;
    Vector3[] Path = new Vector3[3];
    PlayerUI PUI;
    Transform OriPos;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine("Timer");//呼叫Timer
        //StopCoroutine("Timer");//停止Timer
        OriPos = this.gameObject.transform;
        PlayerCon = FindObjectOfType<PlayerControl>();
        PUI = FindObjectOfType<PlayerUI>();      
    }

    // Update is called once per frame
    void Update()
    {        
        /*if (PlayerCon.IsStriked || PlayerCon.IsDead)
        {
            //StartCoroutine("Timer",PlayerCon.Hp+1);
            PUI.CottonHPArray[PlayerCon.Hp+1].SetActive(false);
        }*/
    }//Player Lose Cotton


    void CottonMoveSetting(int CottonNum)
    {
        moveSetting = new Hashtable();
        moveSetting.Add("time", 3.0f);
        moveSetting.Add("easetype", iTween.EaseType.easeOutQuart);

        //設置三個移動path node為，此部分可依照角色特性微調

        Path[0] = new Vector3(PUI.CottonHPArray[CottonNum].transform.position.x, PUI.CottonHPArray[CottonNum].transform.position.y, PUI.CottonHPArray[CottonNum].transform.position.z);

        Path[1] = new Vector3(PUI.CottonHPArray[CottonNum].transform.position.x + 200, PUI.CottonHPArray[CottonNum].transform.position.y + 60, PUI.CottonHPArray[CottonNum].transform.position.z + 25f);

        Path[2] = new Vector3(PUI.CottonHPArray[CottonNum].transform.position.x + 500, PUI.CottonHPArray[CottonNum].transform.position.y - 400, PUI.CottonHPArray[CottonNum].transform.position.z + 50f);

        moveSetting.Add("path", Path);

        iTween.MoveTo(PUI.CottonHPArray[CottonNum], moveSetting);

    }



    IEnumerator Timer(int CottonNum)
    {
        /*Vector3 Temp;
        Temp = PUI.CottonHPArray[CottonNum].transform.position;

        CottonMoveSetting(PlayerCon.Hp);*/
        //yield return new WaitForSeconds(1f);
        while (PUI.CottonHPArray[CottonNum].GetComponent<Image>().color.a > 0) {
            yield return new WaitForSeconds(0.05f);
            PUI.CottonHPArray[CottonNum].GetComponent<Image>().color -= new Color(0,0,0,0.05f);
        }
        yield return new WaitForSeconds(3f);
        //PUI.LoseCottonHPUI(PlayerCon.Hp);
        /*PUI.CottonHPArray[CottonNum].transform.position = Temp;*/
        //PUI.CottonHPArray[CottonNum].GetComponent<Image>().color += new Color(0, 0, 0, 1f);  
        //StopCoroutine("Timer");
    }

}
