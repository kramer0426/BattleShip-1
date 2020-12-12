using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class PassiveSkillDataExcel : ScriptableObject
{
    public List<PassiveSkillEntity> Sheet1; // Replace 'EntityType' to an actual type that is serializable.
}
