using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BulletBase : MonoBehaviour
    {
        public bool bPlayer_;

        public virtual void Shoot(bool bPlayer, int damage, Vector3 targetPos) { }

        public virtual void Die() { }
    }
}