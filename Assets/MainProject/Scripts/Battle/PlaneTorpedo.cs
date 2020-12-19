using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class PlaneTorpedo : PlaneBase
    {
        private Vector3 pathPoint1 = new Vector3(0, 2.5f, 0);
        private Vector3 pathPoint2 = new Vector3(0, 2.5f, 0);
        private Vector3 outPathPoint1 = new Vector3(0, 2.5f, 0);
        private Vector3 outPathPoint2 = new Vector3(0, 2.5f, 0);
        private Vector3 startPos = new Vector3(0, 2.5f, 0);
        private Vector3 targetPos = new Vector3(0, 2.5f, 0);
        private Vector3 torpedoStartPos = new Vector3(0, 2.5f, 0);
        private Vector3 torpedoTargetPos = new Vector3(0, 2.5f, 0);

        //
        public GameObject bulletObject_;

        //
        private void FixedUpdate()
        {

        }

        //----------------------------------------------------------------------------------------
        // SetPlane
        //----------------------------------------------------------------------------------------
        public override void SetPlane(bool bPlayer, int damage)
        {
            bPlayer_ = bPlayer;

            Invoke("ActivatePassive", (DataMgr.PlaneBombMoveInTime - Random.Range(1.0f, 1.5f)) / BattleControl.Instance.battleTimeScale_);

            if (bPlayer_)
            {
                startPos = BattleControl.Instance.playerPlaneStartPos_.transform.position;
                targetPos.x += 0.5f;
                pathPoint1 = pathPoint2 = startPos;
                pathPoint1.x += (targetPos.x - startPos.x) * 0.33f;
                pathPoint2.x += (targetPos.x - startPos.x) * 0.66f;
                pathPoint2.y += 0.1f;

            }
            else
            {
                startPos = BattleControl.Instance.enemyPlaneStartPos_.transform.position;
                targetPos.x -= 0.5f;
                pathPoint1 = pathPoint2 = startPos;
                pathPoint1.x -= (startPos.x - targetPos.x) * 0.33f;
                pathPoint2.x -= (startPos.x - targetPos.x) * 0.66f;
                pathPoint2.y += 0.1f;
            }

            LTSpline ltSpline = new LTSpline(new Vector3[] { startPos, startPos, pathPoint1, pathPoint2, targetPos, targetPos });
            LeanTween.moveSpline(this.gameObject, ltSpline, DataMgr.PlaneBombMoveInTime / BattleControl.Instance.battleTimeScale_).setOnComplete(
            () =>
            {
                torpedoStartPos = this.gameObject.transform.position;
                torpedoStartPos.y -= 0.1f;

                //
                GameObject bulletGO = (GameObject)Instantiate(bulletObject_, torpedoStartPos, this.transform.rotation);
                BulletBase bullet = bulletGO.GetComponent<BulletBase>();
                if (bullet != null)
                {
                    if (bPlayer)
                    {
                        torpedoTargetPos = BattleControl.Instance.enemyBattleReadyPos_.transform.position;
                    }
                    else
                    {
                        torpedoTargetPos = BattleControl.Instance.playerBattleReadyPos_.transform.position;
                    }
                        
                    torpedoTargetPos.y = torpedoStartPos.y;

                    bullet.Shoot(bPlayer, damage, torpedoTargetPos);
                }

                if (bPlayer)
                {
                    outPathPoint1 = outPathPoint2 = targetPos;
                    outPathPoint1.y = BattleControl.Instance.enemyPlaneStartPos_.transform.position.y;
                    outPathPoint2.y = BattleControl.Instance.enemyPlaneStartPos_.transform.position.y;
                    outPathPoint1.x += (BattleControl.Instance.enemyPlaneStartPos_.transform.position.x - targetPos.x) * 0.2f;
                    outPathPoint2.x += (BattleControl.Instance.enemyPlaneStartPos_.transform.position.x - targetPos.x) * 0.8f;

                    LTSpline outLtSpline = new LTSpline(new Vector3[] { targetPos, targetPos, outPathPoint1, outPathPoint2, BattleControl.Instance.enemyPlaneStartPos_.transform.position, BattleControl.Instance.enemyPlaneStartPos_.transform.position });
                    LeanTween.moveSpline(this.gameObject, outLtSpline, DataMgr.PlaneBombMoveInTime / BattleControl.Instance.battleTimeScale_).setOnComplete(
                    () =>
                    {
                        Destroy(this.gameObject);
                    });
                }
                else
                {
                    outPathPoint1 = outPathPoint2 = targetPos;
                    outPathPoint1.y = BattleControl.Instance.playerPlaneStartPos_.transform.position.y;
                    outPathPoint2.y = BattleControl.Instance.playerPlaneStartPos_.transform.position.y;
                    outPathPoint1.x += (BattleControl.Instance.playerPlaneStartPos_.transform.position.x - targetPos.x) * 0.2f;
                    outPathPoint2.x += (BattleControl.Instance.playerPlaneStartPos_.transform.position.x - targetPos.x) * 0.8f;

                    LTSpline outLtSpline = new LTSpline(new Vector3[] { targetPos, targetPos, outPathPoint1, outPathPoint2, BattleControl.Instance.playerPlaneStartPos_.transform.position, BattleControl.Instance.playerPlaneStartPos_.transform.position });
                    LeanTween.moveSpline(this.gameObject, outLtSpline, DataMgr.PlaneBombMoveInTime / BattleControl.Instance.battleTimeScale_).setOnComplete(
                    () =>
                    {
                        Destroy(this.gameObject);
                    });
                }

            });


        }
    }

}