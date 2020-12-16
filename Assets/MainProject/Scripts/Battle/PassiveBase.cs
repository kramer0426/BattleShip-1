using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PassiveBase : MonoBehaviour
    {
        public bool                 bPlayer_;
        public float                resultValue_ = 0;
        public PassiveSkillEntity   passiveInfo_ = null;

        public virtual void SetPassive(bool bPlayer, PassiveSkillEntity passiveInfo) { }

        public virtual void ActivatePassive() { }

        public virtual float GetPassiveValue1() { return resultValue_; }
        public virtual float GetPassiveValue2() { return resultValue_; }
        public virtual float GetPassiveValue3() { return resultValue_; }

    }
}