using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


namespace Sinabro
{
    //
    // my info
    //
    [System.Serializable]
    public class MyInfo
    {
        // for save
        public int myGold;
        public int[] myStatLevels = new int[7];
        public int[] myStageLevels = new int[12];
        public bool[] isEdgesCleard = new bool[12];
        public bool[] isFinalsCleard = new bool[12];

        public List<int> myPurchaseSkillList = new List<int>();
        public int[] myEquipedSkillIds = new int[5];

        public List<int> myPurchaseActiveItemList = new List<int>();
        public int[] myEquipedActiveItemIds = new int[3];
        public List<int> myPurchasePassiveItemList = new List<int>();
        public int[] myEquipedPassiveItemIds = new int[4];

        public List<int> myPurchaseToolsList = new List<int>();
        public int[] myEquipedToolsIds = new int[4];

        //
        public DateTime saveDate;
        public string charName;

        // for play
        public int currentStageIndex;

        public MyInfo()
        {
            myGold = DataMgr.DEFALUT_GOLD;

            for (int i = 0; i < 7; ++i)
                myStatLevels[i] = 1;

            for (int i = 0; i < 12; ++i)
            {
                myStageLevels[i] = 0;
                isEdgesCleard[i] = false;
                isFinalsCleard[i] = false;
            }

            currentStageIndex = 0;

            myPurchaseSkillList.Add(1101);
            myPurchaseSkillList.Add(2101);
            myPurchaseSkillList.Add(3101);
            myPurchaseSkillList.Add(4101);
            myPurchaseSkillList.Add(5101);

            myEquipedSkillIds[0] = 1101;
            myEquipedSkillIds[1] = 2101;
            myEquipedSkillIds[2] = 3101;
            myEquipedSkillIds[3] = 4101;
            myEquipedSkillIds[4] = 5101;

            for (int i = 0; i < 3; ++i)
                myEquipedActiveItemIds[i] = 0;

            for (int i = 0; i < 4; ++i)
                myEquipedPassiveItemIds[i] = 0;

            for (int i = 0; i < 4; ++i)
                myEquipedToolsIds[i] = 0;
        }

        //
        public void ResetMyData()
        {
            //PlayerPrefs.DeleteAll();

            myGold = 0;

            for (int i = 0; i < 7; ++i)
                myStatLevels[i] = 1;

            for (int i = 0; i < 12; ++i)
            {
                myStageLevels[i] = 0;
                isEdgesCleard[i] = false;
                isFinalsCleard[i] = false;
            }


            currentStageIndex = 0;

            myPurchaseSkillList.Clear();
            myPurchaseSkillList.Add(1101);
            myPurchaseSkillList.Add(2101);
            myPurchaseSkillList.Add(3101);
            myPurchaseSkillList.Add(4101);
            myPurchaseSkillList.Add(5101);

            myEquipedSkillIds[0] = 1101;
            myEquipedSkillIds[1] = 2101;
            myEquipedSkillIds[2] = 3101;
            myEquipedSkillIds[3] = 4101;
            myEquipedSkillIds[4] = 5101;

            myPurchaseActiveItemList.Clear();
            myPurchasePassiveItemList.Clear();
            myPurchaseToolsList.Clear();

            for (int i = 0; i < 3; ++i)
                myEquipedActiveItemIds[i] = 0;

            for (int i = 0; i < 4; ++i)
                myEquipedPassiveItemIds[i] = 0;

            for (int i = 0; i < 4; ++i)
                myEquipedToolsIds[i] = 0;

            //
            DataMgr.Instance.SaveData();
        }

        //
        public void AddMyGold(int addmoney)
        {
            myGold += addmoney;

            DataMgr.Instance.SaveData();
        }
    }


    //
    // global data magager
    //
    public class DataMgr
    {
        public StringBuilder stringBuilder_ = new StringBuilder();

        private static DataMgr instance = null;

        public static DataMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataMgr();
                }
                return instance;
            }
        }

        private DataMgr()
        {

        }

        //
        public Dictionary<int, string> g_localizeTextTable = new Dictionary<int, string>();


        //
        public MyInfo myInfo_g = new MyInfo();


        //
        public int loadingImgIndex_g = 1;


        // for string
        public void ClearString()
        {
            stringBuilder_.Remove(0, stringBuilder_.Length);
        }

        //-----------------------------------------------------------------------------------------------------
        // localize table util
        //
        public string GetLocalText(int textId)
        {
            if (g_localizeTextTable.ContainsKey(textId))
            {
                return g_localizeTextTable[textId];
            }

            return "none";
        }


        //-----------------------------------------------
        // SaveData
        //-----------------------------------------------
        public void SaveData()
        {
            myInfo_g.saveDate = DateTime.Now;

            string destination = Application.persistentDataPath + "/test.dat";
            FileStream file;

            Debug.Log("Save~~!! : " + destination);

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, myInfo_g);
            file.Close();
        }

        //-----------------------------------------------
        // LoadData
        //-----------------------------------------------
        public void LoadData()
        {
            string destination = Application.persistentDataPath + "/test.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogError("File not found");
                SaveData();
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            MyInfo data = (MyInfo)bf.Deserialize(file);
            file.Close();

            myInfo_g = data;

            if (myInfo_g.isEdgesCleard == null)
            {
                myInfo_g.isEdgesCleard = new bool[12];
                for (int i = 0; i < 8; ++i)
                    myInfo_g.isEdgesCleard[i] = false;
            }
            if (myInfo_g.isFinalsCleard == null)
            {
                myInfo_g.isFinalsCleard = new bool[12];
                for (int i = 0; i < 8; ++i)
                    myInfo_g.isFinalsCleard[i] = false;
            }

            if (myInfo_g.myStageLevels.Length == 8)
            {
                Array.Resize(ref myInfo_g.myStageLevels, 12);
            }
            if (myInfo_g.isEdgesCleard.Length == 8)
            {
                Array.Resize(ref myInfo_g.isEdgesCleard, 12);
            }
            if (myInfo_g.isFinalsCleard.Length == 8)
            {
                Array.Resize(ref myInfo_g.isFinalsCleard, 12);
            }


        }

        // for local
        public const int TEXT_MAIN_TITLE = 1;

        public const int TEXT_CONFIRM = 100;
        public const int TEXT_CANCEL = 101;


        //
        public const int DEFALUT_GOLD = 0;
    }


}

