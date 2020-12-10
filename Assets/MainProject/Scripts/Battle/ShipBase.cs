using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShipBase : MonoBehaviour
    {
        //
        public ShipState            shipState_;
        public BattleShipEntity     battleShipInfo_ = null;

        //
        public int                  hp_;
        public int                  ap_;
        public int                  sideDp_;
        public int                  topDp_;
        public int                  torpedoDp_;
        public float                fireTime_;
        public float                reloadTime_;
        public int                  maxShellCnt_;
        public int                  currentShellCnt_;

        public virtual void SetBattleShip(BattleShipEntity battleShip) { }
        public virtual void Move(Vector3 targetPos) { }
        public virtual void Damage(int damage) { }
        public virtual void Die() { }
    }
}