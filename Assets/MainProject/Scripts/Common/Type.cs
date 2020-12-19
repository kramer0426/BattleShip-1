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
    AvoidRate = 11,
    MAX = 12
}

public enum PassiveValueType
{
    AllDp = 0,
    AllAp = 1,
    CriticalRate = 2,
    CriticalDamage = 3,
    Accuracy = 4,
    AvoidRate = 5,
    ShellCnt = 6,
    ReloadTime = 7,
    AddMyDamage = 8,
    TorpedoAttack = 9,
    AvoidTorpedo = 10,
    MyHp = 11,
    FleetAp = 12,
    MAX = 13
}

public enum PassiveType
{
    DefenseUp = 0,
    AttackUp = 1,
    CriticalRateUp = 2,
    TorpedoDamageDown = 3,
    PlaneShootingDownRate = 4,
    ReloadDown = 5,
    AccuracyUp = 6,
    HpUp = 7,
    AllFleetHpRecoverIfEnemyDestroy = 8,
    AttackUpOnesIfEnemyDestroy = 9,
    AllDamageDownAndAttackDown = 10,
    HpRecoverByAttack = 11,
    AvoidRateUp = 12,
    MaxShellUp = 13,
    CriticalDamageUp = 14,
    AttackUpForBoss = 15,
    AttackUpIfOtherCountry = 16,
    PlaneSupport = 17,
    DefenseUpAndAvoidUp = 18,
    AttackUpAndAccuracyUp = 19,
    CriticalRateUpAndCriticalDamageUp = 20,
    MaxShellUpAndReloadDown = 21,
    AllFleetHpRecoverIfDestroyMe = 22,
    EnemyDestroyIfDestroyMe = 23,
    DamageDownIfSameCountry = 24,
    AttackUpIfMyHighDefenseThenEnemyDefense = 25,
    AvoidTorpedo = 26,
    AttackUpAddedMax10IfEnemyDestroy = 27,
    AttackUpAndMyDamageUp = 28,
    ContinueAttack = 29,
    TorpedoAttackUpAndAvoidTorpedo = 30,
    GainGoldUpIfEnemyDestroy = 31,
    GainCashIfEnemyDestroy = 32,
    CompleteAccuracy = 33,
    AllFleetHpUp = 34,
    AllFleetAttackUp = 35,
    AttackUpAndHpUp = 36,
    Resurrection = 37,
    AllFleetAvoidUp = 38,
    EnemyAttackReflection = 39,
    AttackDownAndHpDownAndAllFleetAttackUp = 40,
    EnemyMaxHpDown = 41,
    AllFleetChaneCountryToSameEnemy = 42,
    MAX = 43
}

public enum MenuState
{
    Ship = 0,
    Weapon = 1,
    Support = 2,
    Soldier = 3,
    Item = 4,
    Shop = 5
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