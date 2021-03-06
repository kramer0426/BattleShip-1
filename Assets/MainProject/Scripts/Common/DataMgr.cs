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
        public List<MyShipData> myShipDataList_ = new List<MyShipData>();


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
            DataMgr.Instance.myInfo_g.myShipDataList_.Clear();
            MyShipData shipData = new MyShipData();
            shipData.shipId_ = 1;
            shipData.fleetIndex_ = 0;
            shipData.upgradeLevel_ = 0;
            shipData.shipInfo_ = DataMgr.Instance.GetBattleShip(shipData.shipId_);
            shipData.passiveId_ = 30;
            DataMgr.Instance.myInfo_g.myShipDataList_.Add(shipData);

            //
            DataMgr.Instance.SaveData();
        }

        //
        public void AddMyGold(int addMoney)
        {
            myGold_ += addMoney;

            DataMgr.Instance.SaveData();
        }

        //
        public void AddMyCash(int addCash)
        {
            myCash_ += addCash;

            DataMgr.Instance.SaveData();
        }
    }

    [System.Serializable]
    public class MyShipData
    {
        //
        public int shipId_;
        public BattleShipEntity shipInfo_;
        public int fleetIndex_;

        //
        public int[] equipItemIds_ = new int[DataMgr.MAX_ITEM_SLOT];
        public int passiveId_;
        public float[] shipAbility_ = new float[(int)ShipAbility.MAX];

        //
        public int upgradeLevel_;

        // to do : passive data

        public MyShipData()
        {
            shipId_ = 0;
            fleetIndex_ = -1;
            upgradeLevel_ = 0;
            passiveId_ = 0;
            for (int i = 0; i < DataMgr.MAX_ITEM_SLOT; ++i)
                equipItemIds_[i] = 0;
        }

        //
        public void MakeShipAbility()
        {

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
        public LocalTextExcel g_localTextExcel;
        public BattleShipDataExcel g_battleShipExcel;

        //
        public MyInfo myInfo_g = new MyInfo();


        //
        public int loadingImgIndex_g = 1;
        public int currentLanguage_g = 1;


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

        public string GetLocalExcelText(int textId)
        {
            for (int i = 0; i < g_localTextExcel.Sheet1.Count; ++i)
            {
                if (g_localTextExcel.Sheet1[i].Id == textId)
                {
                    if (currentLanguage_g == (int)LanguageType.Korean)
                    {
                        return g_localTextExcel.Sheet1[i].krString;
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return "none";
        }

        //-----------------------------------------------------------------------------------------------------
        // GetBattleShip
        //
        public BattleShipEntity GetBattleShip(int id)
        {
            for (int i = 0; i < g_battleShipExcel.Sheet1.Count; ++i)
            {
                if (g_battleShipExcel.Sheet1[i].Id == id)
                {
                    return g_battleShipExcel.Sheet1[i];
                }
            }

            return null;
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

        public const int TEXT_MAIN_MENU_1 = 201;

        //
        public const int BOSS_CHAPTER = 9;
        public const int MAX_CHAPTER = 10;
        public const int MIN_ENEMY_SHIP_LIMITE = 2;
        public const int MAX_ITEM_SLOT = 5;
        public const int MAX_PASSIVE_DESTROY_ENEMY = 10;

        //
        public const int DEFALUT_GOLD = 0;

        //
        public const float StartBattlePlayerMoveTime = 1.0f;
        public const float ClearBattlePlayerMoveTime = 3.0f;
        public const float LineBulletMoveTime = 1.0f;
        public const float CurveBulletMoveTime = 1.5f;
        public const float PlaneBombMoveInTime = 3.0f;
        public const float PlaneBombMoveOutTime = 1.5f;
    }


}

