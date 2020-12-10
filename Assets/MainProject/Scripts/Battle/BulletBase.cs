using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BulletBase : MonoBehaviour
    {
        public virtual void Shoot(bool bPlayer, int damage, float bulletMoveTime, Vector3 targetPos) { }

        public virtual void Die() { }
    }
}