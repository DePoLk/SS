using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using DataValue;

public class DataSystem : MonoBehaviour
{

    //文本的每行內容
    public ArrayList DataSystemValueInfoAll;
    public ArrayList DataSystemValueInfoAll_Default;
    public ArrayList DataSystemValueInfoAll_Temp;
    StageEvent StE;
    public List<GameObject> UIEventGroup = new List<GameObject>();
    public List<GameObject> StageEventGroup = new List<GameObject>();

    [Header("Debug")]
    

    public int[] StageEventIndex;
    public bool IsAllUIDone = false;

    int StageEventIndexArrayNum = 0;
    [Header("PerElementIndex")]
    public int[] UIEventIndex;
    public int[] WolfIndex;
    public int[] ItemIndex;
    

    // Use this for initialization
    void Start()
    {
        StE = FindObjectOfType<StageEvent>();
        StageEventIndex = new int[DataValue.EventValue.StageEventMax];
        UIEventIndex = new int[DataValue.EventValue.StageEventMax];
        WolfIndex = new int[DataValue.EventValue.StageEventMax];
        ItemIndex = new int[DataValue.EventValue.StageEventMax];
        //刪除文件.
        // DeleteFile(Application.dataPath+"/Save", "Save.txt");

        //創建文件.
        //SaveFile(Application.dataPath + "/Save", "Save.txt",0);

        //得到文本中每一行的內容.
        LoadFileFunction();
        
        GetAllUIGroup();
        GetAllStageEventGroup();
        GetStageEventIndex();
        GetPerElementIndex();
        StageCheckSelfIsDone();

        DeBugOutPut();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(InfoAll[1]);
    }

    /**
   * path：文件創建目錄.
   * name：文件的名稱.
   * info：寫入的內容.
   */

    /*
     * 修改數據索引
     ---> 數據新增
         
         */

    void GetAllUIGroup() {
        for (int i = 0; i < DataValue.EventValue.UIEventMax; i++) {
            UIEventGroup.Add(GameObject.Find("UIEvent"+i));
        }
    }

    void GetAllStageEventGroup() {
        for (int i = 0; i < DataValue.EventValue.StageEventMax; i++)
        {
            StageEventGroup.Add(GameObject.Find("StageEvent" + i));
        }
    }

    public void StageCheckSelfIsDone()
    {
        for (int StageEventNum = 0; StageEventNum < DataValue.EventValue.StageEventMax; StageEventNum++) {

            if (DataSystemValueInfoAll_Default[StageEventIndex[StageEventNum] + 1].ToString().Equals("True"))
            {
                if (StageEventGroup[StageEventNum] != null)
                {
                    StageEventGroup[StageEventNum].GetComponent<StageEvent>().StageEventKillSelf();
                }
            }

        }
     
    }


    void SaveFileToDataSystemInfoAll(StreamWriter sw) {

        int StageEventNum = 0;

        for (int x = 0; x < DataSystemValueInfoAll_Temp.Count; x++) {
            if (DataSystemValueInfoAll_Temp[x].ToString().Equals("-> StageEvent: " + StageEventNum)) {
                if (DataSystemValueInfoAll_Temp[x + 1].ToString().Equals("False")) {
                    for (int SearchIndex = 0; SearchIndex < StageEventGroup[StageEventNum].transform.childCount; SearchIndex++) {
                        
                        if(DataSystemValueInfoAll_Temp[x + 2 + SearchIndex].ToString().Equals("U: " + true))
                        {
                            DataSystemValueInfoAll_Temp[x + 2 + SearchIndex] = ("U: " + false);
                        }
                        if (DataSystemValueInfoAll_Temp[x + 2 + SearchIndex].ToString().Equals("M: " + true))
                        {
                            DataSystemValueInfoAll_Temp[x + 2 + SearchIndex] = ("M: " + false);
                        }
                        if (DataSystemValueInfoAll_Temp[x + 2 + SearchIndex].ToString().Equals("I: " + true))
                        {
                            DataSystemValueInfoAll_Temp[x + 2 + SearchIndex] = ("I: " + false);
                        }//---> 數據新增 若需要新增儲存數值須新增此處

                        //Debug.Log(DataSystemValueInfoAll_Temp[x + 2 + UIEventIndex]);
                        //Debug.Log("x: " + x);
                        //Debug.Log(StageEventNum);
                    }
                }
                StageEventNum++;
            }
            //Debug.Log("Temp0: " + DataSystemValueInfoAll_Temp[x]);
        }


        for (int i = 0; i < DataSystemValueInfoAll_Temp.Count; i++) {
            DataSystemValueInfoAll[i] = DataSystemValueInfoAll_Temp[i];
            
           //Debug.Log("Temp: "+DataSystemValueInfoAll_Temp[i]);
        }

        for (int j = 0; j < DataSystemValueInfoAll.Count; j++) {
            sw.WriteLine(DataSystemValueInfoAll[j]);
            //Debug.Log("Ori: " + DataSystemValueInfoAll[j]);
        }

    }

    void GetStageEventIndex()
    {
        for (int i = 0; i < DataSystemValueInfoAll.Count; i++)
        {
            if (DataSystemValueInfoAll[i].ToString().Equals("-> StageEvent: " + StageEventIndexArrayNum))
            {
                StageEventIndex[StageEventIndexArrayNum] = i;
                //Debug.Log(StageEventIndex[StageEventIndexArrayNum]);
                StageEventIndexArrayNum++;
            }
        }
    }

    //---> 數據新增 新增範圍索引個數

    int UCR (int i){

        int value = 0;

        value = StageEventGroup[i].GetComponent<StageEvent>().GetEventUIEventCountEnd - StageEventGroup[i].GetComponent<StageEvent>().GetEventUIEventCountStart + 1;

        return value;

    }

    int MCR(int i) {
        int value = 0;

        value = StageEventGroup[i].GetComponent<StageEvent>().GetEventMonsterCountEnd - StageEventGroup[i].GetComponent<StageEvent>().GetEventMonsterCountStart + 1;

        return value;
    }

    int ICR(int i) {
        int value = 0;

        value = StageEventGroup[i].GetComponent<StageEvent>().GetEventItemCountEnd - StageEventGroup[i].GetComponent<StageEvent>().GetEventItemCountStart + 1;

        return value;
    }

    int StageEventChildCount(int i) {
        int value = 0;

        value = StageEventGroup[i].transform.childCount;

        return value;
    }

    //---> 數據新增 新增範圍索引個數

    private void GetPerElementIndex() {
        int StageEventIndexNum = 0;
        bool IsGetUIIndex = false;
        bool IsGetWolfIndex = false;
        bool IsGetItemIndex = false;

       //Debug.Log(StageEventChildCount(0));
       //Debug.Log(StageEventChildCount(1));


        for (int i = 0; i < DataSystemValueInfoAll.Count; i++) {
            if (DataSystemValueInfoAll[i].ToString().Equals("-> StageEvent: " + StageEventIndexNum)) {
                // Debug.Log("GetStage: " + StageEventIndexNum);
               // Debug.Log(i);
                //Debug.Log(DataSystemValueInfoAll[i]);
                for (int SearchIndex = 0; SearchIndex < StageEventChildCount(StageEventIndexNum); SearchIndex++) {
                    if (DataSystemValueInfoAll[i + 2 + SearchIndex].ToString().Equals("U: " + false) || DataSystemValueInfoAll[i + SearchIndex].ToString().Equals("U: " + true)) {
                        if (!IsGetUIIndex) {
                            UIEventIndex[StageEventIndexNum] = (i + 2 + SearchIndex);
                            IsGetUIIndex = true;
                        }
                    }
                    
                    if (DataSystemValueInfoAll[i +  2+ SearchIndex].ToString().Equals("M: " + false) || DataSystemValueInfoAll[i + SearchIndex].ToString().Equals("W: " + true)) {
                        if (!IsGetWolfIndex) {
                            WolfIndex[StageEventIndexNum] = (i + 2 +SearchIndex);
                            IsGetWolfIndex = true;            
                        }
                    }

                    if (DataSystemValueInfoAll[i + 2 + SearchIndex].ToString().Equals("I: " + false) || DataSystemValueInfoAll[i + SearchIndex].ToString().Equals("I: " + true))
                    {
                        if (!IsGetItemIndex)
                        {
                            ItemIndex[StageEventIndexNum] = (i + 2 + SearchIndex);
                            IsGetItemIndex = true;
                        }
                    }//---> 數據新增 若需要新增儲存數值須新增此處

                }// end SearchIndex for


                
                StageEventIndexNum++;
                IsGetUIIndex = false;
                IsGetWolfIndex = false;
                
            }

        }// end i for



    }

    void InitializeEventData(StreamWriter sw) {

        for (int i = 0; i < DataValue.EventValue.StageEventMax; i++) {
            DataSystemValueInfoAll.Add("-> StageEvent: " + i);
            DataSystemValueInfoAll.Add(false);

            for (int uc = 0; uc < UCR(i); uc++) {
                DataSystemValueInfoAll.Add("U: " + false);
                //Debug.Log("InUc");
            }
            for (int wc = 0; wc < MCR(i); wc++) {
                DataSystemValueInfoAll.Add("M: " + false);
            }
            for (int ic = 0; ic < ICR(i); ic++) {
                DataSystemValueInfoAll.Add("I: " + false);
            }//---> 數據新增 若需要新增儲存數值須新增此處

        }

        for (int i = 0; i < DataSystemValueInfoAll.Count; i++)
        {
            sw.WriteLine(DataSystemValueInfoAll[i]);//寫入存檔
        }

    }

   

    /*
        -> StageEvent: 0        DataSystemValueInfoAll[0]   StageEventIndex[0] = 0
        False                   DataSystemValueInfoAll[1]
        False                   DataSystemValueInfoAll[2]
        False                   DataSystemValueInfoAll[3]
        -> StageEvent: 1        DataSystemValueInfoAll[4]   StageEventIndex[1] = 4
        False                   DataSystemValueInfoAll[5]
        False                   DataSystemValueInfoAll[6]
        False                   DataSystemValueInfoAll[7]
             */

    void DeBugOutPut() {
        for (int i = 0; i < DataSystemValueInfoAll.Count; i++) {
            //Debug.Log("DataSystemValueInfoAll[" + i + "]: " +  DataSystemValueInfoAll[i]);
        }
        for (int j = 0; j < StageEventIndex.Length; j++) {
            //Debug.Log("StageEventIndex[" + j + "]: " + StageEventIndex[j]);
        }
    }
      


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
            //sw.WriteLine("MakeDoge");
            //InitializeEventData(sw);

            // Debug.Log("SavingStageEventNum: " + StageEventNum);
            SaveFileToDataSystemInfoAll(sw);
            

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
        DataSystemValueInfoAll = LoadFile(Application.dataPath + "/Save", "EventData.txt");
        DataSystemValueInfoAll_Default = LoadFile(Application.dataPath + "/Save", "EventData.txt");
        DataSystemValueInfoAll_Temp = LoadFile(Application.dataPath + "/Save", "EventData.txt");
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
    
}
