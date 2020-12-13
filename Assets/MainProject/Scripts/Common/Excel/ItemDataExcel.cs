using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ItemDataExcel : ScriptableObject
{
    public List<ItemEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
