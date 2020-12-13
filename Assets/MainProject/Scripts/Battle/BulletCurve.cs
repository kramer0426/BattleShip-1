using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BulletCurve : BulletBase
    {
        private Vector3 bezierPoint1 = new Vector3(0, 0, 0);
        private Vector3 bezierPoint2 = new Vector3(0, 0, 0);

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
        // SetBattleShip
        //----------------------------------------------------------------------------------------
        public override void Shoot(bool bPlayer, int damage, Vector3 targetPos)
        {
            bPlayer_ = bPlayer;

            bezierPoint1 = this.transform.position;
            bezierPoint1.y += 1.0f;

            if (bPlayer_)
            {
                bezierPoint1.x += (targetPos.x - this.transform.position.x) * 0.5f;
            }
            else
            {
                bezierPoint1.x -= (this.transform.position.x - targetPos.x) * 0.5f;
            }

            LTSpline ltSpline = new LTSpline(new Vector3[] { this.transform.position, this.transform.position, bezierPoint1, targetPos, targetPos });
            LeanTween.moveSpline(this.gameObject, ltSpline, DataMgr.CurveBulletMoveTime / BattleControl.Instance.battleTimeScale_).setOnComplete(
            () =>
            {
                if (bPlayer)
                {
                    BattleControl.Instance.HitToEnemy(damage, DamageType.Curve);
                }
                else
                {
                    BattleControl.Instance.HitToPlayer(damage, DamageType.Curve);
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