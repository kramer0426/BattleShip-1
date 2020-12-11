using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//
// BattleShipEntity
//
[System.Serializable]
public class BattleShipEntity
{
    public int Id;
    public int Rank;
    public string NameCode;
    public string BackgroundDescriptionCode;
    public int BasePrice;
    public int SpecialPrice; 
    public int PriceIncrement;
    public int BaseDamage;
    public int DamageIncrement;
    public int hp;
    public int Critical;
    public int SideDefence;
    public int TopDefence;
    public int TorpedoDefence;
    public int Contry;
    public int Accuracy;
    public float FireTime;
    public float ReloadTime;
    public int ShellCnt;
    public int maxDamage;
    public string ResourceName;



}

//
// BattleShipEnemyEntity
//
[System.Serializable]
public class BattleShipEnemyEntity
{
    public int Id;
    public int GainGold;
    public int Hp;
    public int Critical;
    public int BaseDamage;
    public int Stage;
    public int SupportBomb;
    public int SupportTorpedo;
    public int SideDefence;
    public int TopDefence;
    public int TorpedoDefence;
    public int Contry;
    public int Accuracy;
    public float FireTime;
    public float ReloadTime;
    public int ShellCnt;
    public int maxDamage;
    public string ResourceName;


}

//
// ChapterEntity
//
[System.Serializable]
public class ChapterEntity
{
    public int ChapterID;
    public int UpgradeValue;


}

namespace Sinabro
{
    public class ExcelDatas : MonoBehaviour
    {
        [SerializeField] BattleShipDataExcel        battleShipExcel_;
        [SerializeField] BattleShipEnemyDataExcel   battleShipEnemyExcel_;
        
        //
        public BattleShipEntity GetBattleShip(int id)
        {
            for (int i = 0; i < battleShipExcel_.Sheet1.Count; ++i)
            {
                if (battleShipExcel_.Sheet1[i].Id == id)
                {
                    return battleShipExcel_.Sheet1[i];
                }
            }

            return null;
        }

        //
        public BattleShipEnemyEntity GetEnemyBattleShip(int id)
        {
            for (int i = 0; i < battleShipEnemyExcel_.Sheet1.Count; ++i)
            {
                if (battleShipEnemyExcel_.Sheet1[i].Id == id)
                {
                    return battleShipEnemyExcel_.Sheet1[i];
                }
            }

            return null;
        }
    }
}