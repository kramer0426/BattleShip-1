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


}

[System.Serializable]
public class ShipPartsEntity
{
    public int Id;
    public int ItemName;


}

namespace Sinabro
{
    public class ExcelLoader : MonoBehaviour
    {
        [SerializeField] BattleShipDataExcel battleShipExcel_;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {




        }

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
    }
}