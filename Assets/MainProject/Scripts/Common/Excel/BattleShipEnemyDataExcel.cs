using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class BattleShipEnemyDataExcel : ScriptableObject
{
    public List<BattleShipEnemyEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
