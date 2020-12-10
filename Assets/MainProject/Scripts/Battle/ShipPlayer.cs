using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShipPlayer : ShipBase
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

                    currentShellCnt_ = maxShellCnt_;
                    StartCoroutine(CoroutineFire());
                });
            }
        }

        //----------------------------------------------------------------------------------------
        // Damage
        //----------------------------------------------------------------------------------------
        public override void Damage(int damage) 
        {
            hp_ -= damage;

            if (hp_ <= 0)
            {
                shipState_ = ShipState.Die;
            }
        }

        //----------------------------------------------------------------------------------------
        // Die
        //----------------------------------------------------------------------------------------
        public override void Die() 
        {
        }

        //
        private void ReloadShell()
        {
            // set reload

            //
            StartCoroutine(CoroutineFire());
        }

        IEnumerator CoroutineFire()
        {
            if (shipState_ == ShipState.Clear || shipState_ == ShipState.Die)
                yield break;

            yield return new WaitForSeconds(fireTime_);


            int fireCnt = 4;

            while (true)
            {
                //
                if (fireCnt <= 0)
                {
                    yield return null;

                    continue;
                }

                //
                if (currentShellCnt_ <= 0)
                {
                    yield return null;

                    Invoke("ReloadShell", reloadTime_);

                    break; 
                }

                if (shipState_ == ShipState.Clear || shipState_ == ShipState.Die)
                {
                    yield return null;

                    break;
                }

                // to do : fire

                //
                fireCnt--;
                currentShellCnt_--;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}