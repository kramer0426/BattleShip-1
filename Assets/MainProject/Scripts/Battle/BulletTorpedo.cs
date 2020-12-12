using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BulletTorpedo : BulletBase
    {
        private Vector3 pathPoint1 = new Vector3(0, 0, 0);
        private Vector3 pathPoint2 = new Vector3(0, 0, 0);
        private Vector3 pathPoint3 = new Vector3(0, 0, 0);

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

            pathPoint1 = pathPoint2 = pathPoint3 = this.transform.position;
            pathPoint1.y -= 0.01f;
            pathPoint2.y += 0.01f;
            pathPoint3.y -= 0.01f;

            if (bPlayer_)
            {
                pathPoint1.x += (targetPos.x - this.transform.position.x) * 0.333f;
                pathPoint2.x += (targetPos.x - this.transform.position.x) * 0.5f;
                pathPoint3.x += (targetPos.x - this.transform.position.x) * 0.666f;
            }
            else
            {
                pathPoint1.x -= (this.transform.position.x - targetPos.x) * 0.333f;
                pathPoint2.x -= (this.transform.position.x - targetPos.x) * 0.5f;
                pathPoint3.x -= (this.transform.position.x - targetPos.x) * 0.666f;
            }

            LTSpline ltSpline = new LTSpline(new Vector3[] { this.transform.position, this.transform.position, pathPoint1, pathPoint2, pathPoint3, targetPos, targetPos });
            LeanTween.moveSpline(this.gameObject, ltSpline, DataMgr.CurveBulletMoveTime).setOnComplete(
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