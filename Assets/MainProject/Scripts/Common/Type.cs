using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    English = 0,
    Korean = 1,
    Japanese = 2,
    Cn = 3,
    Tw = 4,
    MAX = 5
}

public enum ShipState
{
    Start = 0,
    Battle = 1,
    Clear = 2,
    Die = 3
}

public enum DamageType
{
    Line = 0,
    Curve = 1,
    Torpedo = 2
}

public enum ShipAbility
{
    Hp = 0,
    Ap = 1,
    Accuracy = 2,
    SideDp = 3,
    TopDp = 4,
    TorpedoDp = 5,
    FireTime = 6,
    ReloadTime = 7,
    ShellCnt = 8,
    CriticalRate = 9,
    CriticalDamage = 10,
    MAX = 11
}



//
public static class Utils
{
    public static string GetLanguageName(LanguageType type)
    {
        switch (type)
        {
            case LanguageType.English:
                return "English";

            case LanguageType.Korean:
                return "Korean";

            case LanguageType.Japanese:
                return "Japan";

            case LanguageType.Cn:
                return "China";

            case LanguageType.Tw:
                return "Taiwan";

            default:
                break;
        }

        return "English";
    }
}