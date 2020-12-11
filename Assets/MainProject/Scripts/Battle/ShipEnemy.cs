using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShipEnemy : MonoBehaviour
    {
        //
        public ShipState                shipState_;
        public BattleShipEnemyEntity    battleShipInfo_ = null;
        public ShakeControl             shakeControl_;
        public GameObject bulletObject_;
        public GameObject[] guns_;

        //
        private Vector3 bulletTargetPos_ = new Vector3(0, 0, 0);
        private Coroutine fireCoroutine = null;

        //
        public int hp_;
        public int ap_;
        public int sideDp_;
        public int topDp_;
        public int torpedoDp_;
        public float fireTime_;
        public float reloadTime_;
        public int maxShellCnt_;
        public int currentShellCnt_;

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
        public void SetBattleShip(BattleShipEnemyEntity battleShip, Vector3 startPos)
        {
            battleShipInfo_ = battleShip;
            shipState_ = ShipState.Start;
            transform.position = startPos;

            hp_ = battleShipInfo_.Hp;
            ap_ = battleShipInfo_.BaseDamage;
            sideDp_ = battleShipInfo_.SideDefence;
            topDp_ = battleShipInfo_.TopDefence;
            torpedoDp_ = battleShipInfo_.TorpedoDefence;
            fireTime_ = battleShipInfo_.FireTime;
            reloadTime_ = battleShipInfo_.ReloadTime;
            maxShellCnt_ = battleShipInfo_.ShellCnt;

            fireTime_ = 1.0f;
        }

        //----------------------------------------------------------------------------------------
        // Move
        //----------------------------------------------------------------------------------------
        public void Move()
        {
            if (shipState_ == ShipState.Start)
            {
                LeanTween.move(this.gameObject, BattleControl.Instance.enemyBattleReadyPos_.transform.position, 1.0f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    shipState_ = ShipState.Battle;
                    currentShellCnt_ = maxShellCnt_;

                    BattleControl.Instance.playerShip_.StartFire();
                    StartFire();
                });
            }
        }

        //----------------------------------------------------------------------------------------
        // StartFire
        //----------------------------------------------------------------------------------------
        public void StartFire()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = StartCoroutine(CoroutineFire());
        }

        //----------------------------------------------------------------------------------------
        // HoldFire
        //----------------------------------------------------------------------------------------
        public void HoldFire()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            CancelInvoke("ReloadShell");
        }

        //----------------------------------------------------------------------------------------
        // Damage
        //----------------------------------------------------------------------------------------
        public void Damage(int damage)
        {
            hp_ -= damage;
            if (hp_ <= 0)
                hp_ = 0;

            BattleControl.Instance.battleUI_.UpdateEnemyHp(hp_);

            if (hp_ <= 0)
            {
                shipState_ = ShipState.Die;
                BattleControl.Instance.playerShip_.HoldFire();
                BattleControl.Instance.DestroyEnemy();
                DataMgr.Instance.myInfo_g.AddMyGold(battleShipInfo_.GainGold);

                Invoke("Die", 0.3f);
            }
            else
            {
                StartCoroutine(shakeControl_.Shake(this.gameObject.transform.position, 0.2f, 0.02f));
            }
        }

        //----------------------------------------------------------------------------------------
        // Die
        //----------------------------------------------------------------------------------------
        public void Die()
        {
            Destroy(this.gameObject);
        }

        //
        private void ReloadShell()
        {
            // set reload
            currentShellCnt_ = maxShellCnt_;

            //
            StartCoroutine(CoroutineFire());
        }

        IEnumerator CoroutineFire()
        {
            yield return new WaitForSeconds(fireTime_);

            int fireCnt = guns_.Length;

            while (true)
            {
                //
                if (currentShellCnt_ <= 0)
                {
                    yield return null;

                    Invoke("ReloadShell", reloadTime_);

                    break;
                }

                //
                if (fireCnt <= 0)
                {
                    fireCnt = guns_.Length;

                    yield return new WaitForSeconds(fireTime_);
                }

                //
                if (shipState_ == ShipState.Clear || shipState_ == ShipState.Die)
                {
                    yield return null;

                    break;
                }

                // to do : fire
                GameObject bulletGO = (GameObject)Instantiate(bulletObject_, guns_[guns_.Length - fireCnt].transform.position, this.transform.rotation);
                BulletBase bullet = bulletGO.GetComponent<BulletBase>();
                if (bullet != null)
                {
                    bulletTargetPos_.x = BattleControl.Instance.playerShip_.transform.position.x + Random.Range(-0.1f, 0.1f);
                    bulletTargetPos_.y = guns_[guns_.Length - fireCnt].transform.position.y;

                    bullet.Shoot(false, ap_, bulletTargetPos_);
                }


                //
                fireCnt--;
                currentShellCnt_--;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}