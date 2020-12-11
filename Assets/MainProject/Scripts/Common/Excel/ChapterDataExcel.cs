using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ChapterDataExcel : ScriptableObject
{
    public List<ChapterEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
