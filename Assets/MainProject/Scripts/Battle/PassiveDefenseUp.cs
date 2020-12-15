using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PassiveDefenseUp : PassiveBase
    {
        //-----------------------------------------------------------------------------------------
        // SetPassive
        //-----------------------------------------------------------------------------------------
        public override void SetPassive(bool bPlayer, PassiveSkillEntity passiveInfo) 
        {
            bPlayer_ = bPlayer;
            passiveInfo_ = passiveInfo;
        }

        //-----------------------------------------------------------------------------------------
        // ActivatePassive
        //-----------------------------------------------------------------------------------------
        public override void ActivatePassive() 
        {
        }

        //-----------------------------------------------------------------------------------------
        // GetPassiveValue
        //-----------------------------------------------------------------------------------------
        public override float GetPassiveValue(PassiveValueType valueType) 
        { 
            return resultValue_; 
        }
    }

}

