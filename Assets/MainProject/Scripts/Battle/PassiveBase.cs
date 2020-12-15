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

        public virtual float GetPassiveValue(PassiveValueType valueType) { return resultValue_; }

    }
}