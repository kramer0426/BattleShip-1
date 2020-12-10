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
        }

        //----------------------------------------------------------------------------------------
        // Shoot
        //----------------------------------------------------------------------------------------
        public override void Shoot(bool bPlayer, int damage, float bulletMoveTime, Vector3 targetPos)
        {
            LeanTween.move(this.gameObject, targetPos, bulletMoveTime).setOnComplete(
            () =>
            {
                if (bPlayer)
                {
                    BattleControl.Instance.HitToEnemy(damage);
                }
                else
                {
                    BattleControl.Instance.HitToPlayer(damage);
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