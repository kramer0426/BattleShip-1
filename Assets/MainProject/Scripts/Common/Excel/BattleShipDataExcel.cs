using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class BattleShipDataExcel : ScriptableObject
{
    public List<BattleShipEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
