using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PassiveValue : PassiveBase
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
        // GetPassiveValue1
        //-----------------------------------------------------------------------------------------
        public override float GetPassiveValue1() 
        {
            return passiveInfo_.Value1; 
        }

        //-----------------------------------------------------------------------------------------
        // GetPassiveValue2
        //-----------------------------------------------------------------------------------------
        public override float GetPassiveValue2()
        {
            return passiveInfo_.Value2;
        }

        //-----------------------------------------------------------------------------------------
        // GetPassiveValue3
        //-----------------------------------------------------------------------------------------
        public override float GetPassiveValue3()
        {
            return passiveInfo_.Value3;
        }
    }

}

