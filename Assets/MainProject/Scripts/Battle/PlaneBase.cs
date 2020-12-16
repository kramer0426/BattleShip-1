using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PlaneBase : MonoBehaviour
    {
        public bool bPlayer_;

        public virtual void SetPlane(bool bPlayer, int damage) { }

        public virtual void Die() 
        {
            Destroy(this.gameObject);
        }

        public virtual void ActivatePassive()
        {
            if (bPlayer_)
            {
                BattleControl.Instance.enemyShip_.ActivatePassiveByPlayerPlane(this);
            }
            else
            {
                BattleControl.Instance.playerShip_.ActivatePassiveByEnemyPlane(this);
            }
        }
    }
}