using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShipEnemy : ShipBase
    {
        //
        private void FixedUpdate()
        {
            if (shipState_ == ShipState.Battle)
            {

            }
        }

        //----------------------------------------------------------------------------------------
        // SetBattleShip
        //----------------------------------------------------------------------------------------
        public override void SetBattleShip(BattleShipEntity battleShip)
        {
            battleShipInfo_ = battleShip;
            shipState_ = ShipState.Start;
        }

        //----------------------------------------------------------------------------------------
        // Move
        //----------------------------------------------------------------------------------------
        public override void Move(Vector3 targetPos)
        {
            if (shipState_ == ShipState.Start)
            {
                LeanTween.move(this.gameObject, targetPos, 1.0f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    shipState_ = ShipState.Battle;
                });
            }
        }

        //----------------------------------------------------------------------------------------
        // Die
        //----------------------------------------------------------------------------------------
        public override void Die()
        {
        }
    }
}