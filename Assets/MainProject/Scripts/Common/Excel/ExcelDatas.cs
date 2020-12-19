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
    public string ShipClass;
    public int NameCode;
    public int BackgroundDescriptionCode;
    public int SignitureLocalCode;
    public int BasePrice;
    public int SpecialPrice; 
    public int PriceIncrement;
    public int BaseDamage;
    public int DamageIncrement;
    public int hp;
    public int Critical;
    public int SideDefence;
    public int TopDefence;
    public int Country;
    public int Accuracy;
    public float CriticalRate;
    public float CriticalDamage;
    public int GunCount;
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
    public int Country;
    public int Accuracy;
    public float CriticalRate;
    public float CriticalDamage;
    public float FireTime;
    public float ReloadTime;
    public int ShellCnt;
    public int maxDamage;
    public string ResourceName;
    public string PassiveIds;


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

//
// ShipSkinEntity
//
[System.Serializable]
public class ShipSkinEntity
{
    public int Id;
    public int BasttleShipID;
    public string RresourceName;
    public int PriceCash;
    public int PriceGold;
}

//
// PassiveSkillEntity
//
[System.Serializable]
public class PassiveSkillEntity
{
    public int Id;
    public int Rank;
    public int LocalNameId;
    public int LocalDescId;
    public int Type;
    public float Value1;
    public float Value2;
    public float Value3;
    public int RandomRate;
}

//
// ItemEntity
//
[System.Serializable]
public class ItemEntity
{
    public int Id;
    public int Rank;
    public int LocalNameId;
    public int LocalDescId;
    public int Type;
    public float Value1;
    public float Value2;
    public float RandomRate;
}

//
// LocalizeTextEntity
//
[System.Serializable]
public class LocalizeTextEntity
{
    public int Id;
    public string krString;
    public string enString;
    public string jpString;
    public string cnString;
    public string twString;
}

namespace Sinabro
{
    public class ExcelDatas : MonoBehaviour
    {
        [SerializeField] BattleShipEnemyDataExcel   battleShipEnemyExcel_;
        [SerializeField] ChapterDataExcel           chapterExcel_;
        [SerializeField] ShipSkinDataExcel          shipSkinExcel_;
        [SerializeField] PassiveSkillDataExcel      passiveSkillExcel_;
        [SerializeField] ItemDataExcel              itemExcel_;
        


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

        //
        public ChapterEntity GetChapter(int id)
        {
            for (int i = 0; i < chapterExcel_.Sheet1.Count; ++i)
            {
                if (chapterExcel_.Sheet1[i].ChapterID == id)
                {
                    return chapterExcel_.Sheet1[i];
                }
            }

            return null;
        }

        //
        public ShipSkinEntity GetShipSkin(int id)
        {
            for (int i = 0; i < shipSkinExcel_.Sheet1.Count; ++i)
            {
                if (shipSkinExcel_.Sheet1[i].Id == id)
                {
                    return shipSkinExcel_.Sheet1[i];
                }
            }

            return null;
        }

        //
        public PassiveSkillEntity GetPassiveSkill(int id)
        {
            for (int i = 0; i < passiveSkillExcel_.Sheet1.Count; ++i)
            {
                if (passiveSkillExcel_.Sheet1[i].Id == id)
                {
                    return passiveSkillExcel_.Sheet1[i];
                }
            }

            return null;
        }

        //
        public ItemEntity GetItem(int id)
        {
            for (int i = 0; i < itemExcel_.Sheet1.Count; ++i)
            {
                if (itemExcel_.Sheet1[i].Id == id)
                {
                    return itemExcel_.Sheet1[i];
                }
            }

            return null;
        }
    }
}