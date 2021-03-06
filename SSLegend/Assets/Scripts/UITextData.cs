﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class UITextData : MonoBehaviour
{

    PlayerControl PlayerCon;
    GameObject Player_Obj;
   

    //文本的每行內容
    public ArrayList InfoAll;

    

    // Use this for initialization
    void Start()
    {
        //刪除文件.
        // DeleteFile(Application.dataPath+"/Save", "Save.txt");

        //創建文件.
        //SaveFile(Application.dataPath + "/Save", "Save.txt",0);

        //得到文本中每一行的內容.
        LoadFileFunction();
        PlayerCon = FindObjectOfType<PlayerControl>();
        Player_Obj = GameObject.Find("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
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
            //如果文件不存在則創建.
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在則打開.
            sw = t.AppendText();        
        }
        //以行的形式寫入信息.
        // sw.WriteLine();

        //關閉流.
        sw.Close();

        //銷毀流.
        sw.Dispose();
    }

    public void LoadFileFunction()
    {
        InfoAll = LoadFile(Application.dataPath + "/Save", "UIData.txt");
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
           
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J))
            {
                DeleteFile(Application.dataPath + "/Save", "UIData.txt");
                SaveFile(Application.dataPath + "/Save", "UIData.txt");    
            }
        }
    }

}
