using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ShipSkinDataExcel : ScriptableObject
{
    public List<ShipSkinEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
