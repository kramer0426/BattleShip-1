using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PlaneBase : MonoBehaviour
    {
        public bool bPlayer_;

        public virtual void SetPlane() { }

        public virtual void Die() { }
    }
}