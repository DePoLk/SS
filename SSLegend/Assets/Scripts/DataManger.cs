using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DataManger : MonoBehaviour {
    
    PlayerControl PlayerCon;
    GameObject Player_Obj;
    Exp_System ExpS;
    PlayerUI PUI;
    //文本的每行內容
    public ArrayList InfoAll;

    Scene ThisScene;

    // Use this for initialization
    void Start () {
        //刪除文件.
        // DeleteFile(Application.dataPath+"/Save", "Save.txt");

        //創建文件.
        //SaveFile(Application.dataPath + "/Save", "Save.txt",0);

        //得到文本中每一行的內容.
        LoadFileFunction();
        PlayerCon = FindObjectOfType<PlayerControl>();
        Player_Obj = GameObject.Find("Player");
        ExpS = FindObjectOfType<Exp_System>();
        PUI = FindObjectOfType<PlayerUI>();
        ThisScene = SceneManager.GetActiveScene();
    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(InfoAll[1]);
	}

    /**
   * path：文件創建目錄.
   * name：文件的名稱.
   * info：寫入的內容.
   */

   
    public void SaveFile(string path, string name)
    {
        //文件流信息.
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果文件不存在則創建
            //此處的順序即是Save.txt的順序
            sw = t.CreateText();


            if (ThisScene.name.Equals("MainMenu")) {
                sw.WriteLine(3);//0
                sw.WriteLine(3);
                sw.WriteLine(100);
                sw.WriteLine(10);
                sw.WriteLine(3);
                //寫入主角的位置
                sw.WriteLine(16.76f);
                sw.WriteLine(10.8f);
                sw.WriteLine(4.53f);
                sw.WriteLine("2-1");
                //寫入ExpSystem所需資料
            }
            else {
                sw.WriteLine(PlayerCon.Hp);//0
                sw.WriteLine(PlayerCon.MaxHp);
                sw.WriteLine(PlayerCon.ExpPoint);
                sw.WriteLine(PlayerCon.WaterElement);
                sw.WriteLine(PlayerCon.WindElement);
                //寫入主角的位置
                sw.WriteLine(Player_Obj.transform.position.x);
                sw.WriteLine(Player_Obj.transform.position.y);
                sw.WriteLine(Player_Obj.transform.position.z);
                //寫入ExpSystem所需資料
                sw.WriteLine(PlayerCon.ThisScene.name);//8
            }

            

            //--- 最後場景
            
        }
        else
        {
            //如果此文件存在則打開.
            sw = t.AppendText();
            sw.WriteLine(PlayerCon.ExpPoint);
           
        }
        //以行的形式寫入信息.
       // sw.WriteLine();

        //關閉流.
        sw.Close();

        //銷毀流.
        sw.Dispose();
    }

    public void LoadFileFunction() {
        InfoAll = LoadFile(Application.dataPath + "/Save", "Save.txt");
    }

    /**
      * path：讀取文件的路徑.
      * name：讀取文件的名稱.
      */
    ArrayList LoadFile(string path, string name)
    {
        //使用流的形式讀取.
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路徑與名稱未找到文件則直接回傳null.
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的讀取.
            //將每一行的內容存入數組鏈表容器中.
            arrlist.Add(line);
        }
        //關閉流.
        sr.Close();
        //銷毀流.
        sr.Dispose();
        //將數組鏈表容器返回.
        return arrlist;
    }

    /**
      * path：刪除文件的路徑.
      * name：刪除文件的名稱.
      */

    public void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
        }
    }

}
