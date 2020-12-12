using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BulletLine : BulletBase
    {
        //
        private void FixedUpdate()
        {
            if (bPlayer_)
            {
                if (BattleControl.Instance.enemyShip_.shipState_ == ShipState.Die)
                {
                    Destroy(this.gameObject);
                }
                
            }
            else
            {
                if (BattleControl.Instance.playerShip_.shipState_ == ShipState.Die)
                {
                    Destroy(this.gameObject);
                }

            }
        }

        //----------------------------------------------------------------------------------------
        // Shoot
        //----------------------------------------------------------------------------------------
        public override void Shoot(bool bPlayer, int damage, Vector3 targetPos)
        {
            bPlayer_ = bPlayer;

            LeanTween.move(this.gameObject, targetPos, DataMgr.LineBulletMoveTime).setOnComplete(
            () =>
            {
                if (bPlayer)
                {
                    BattleControl.Instance.HitToEnemy(damage, DamageType.Line);
                }
                else
                {
                    BattleControl.Instance.HitToPlayer(damage, DamageType.Line);
                }
                
                Destroy(this.gameObject);
            });
        }


        //----------------------------------------------------------------------------------------
        // Die
        //----------------------------------------------------------------------------------------
        public override void Die()
        {
        }
    }
}