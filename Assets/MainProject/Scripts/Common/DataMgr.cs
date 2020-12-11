﻿using System.Collections;
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
        public int myGold_;
        public int myCash_;
        public int currentChapter_;

        //
        public DateTime saveDate;
        public string charName;

        

        public MyInfo()
        {
            myGold_ = DataMgr.DEFALUT_GOLD;
            myCash_ = 0;
            currentChapter_ = 0;
        }

        //
        public void ResetMyData()
        {
            myGold_ = DataMgr.DEFALUT_GOLD;
            myCash_ = 0;

            currentChapter_ = 0;

            //
            DataMgr.Instance.SaveData();
        }

        //
        public void AddMyGold(int addmoney)
        {
            myGold_ += addmoney;

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

        }

        // for local
        public const int TEXT_MAIN_TITLE = 1;

        public const int TEXT_CONFIRM = 100;
        public const int TEXT_CANCEL = 101;


        //
        public const int DEFALUT_GOLD = 1000;

        //
        public const float StartBattlePlayerMoveTime = 1.0f;
        public const float ClearBattlePlayerMoveTime = 3.0f;
        public const float LineBulletMoveTime = 1.0f;
        public const float CurveBulletMoveTime = 1.5f;
    }


}

