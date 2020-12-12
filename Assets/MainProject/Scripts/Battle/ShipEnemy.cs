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
        public GameObject               bulletObject_;
        public GameObject[]             guns_;

        //
        private Vector3 bulletTargetPos_ = new Vector3(0, 0, 0);
        private Coroutine fireCoroutine = null;

        //
        public int currentShellCnt_ = 0;
        public float[] shipAbility_ = new float[(int)ShipAbility.MAX];

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

            shipAbility_ = new float[(int)ShipAbility.MAX];

            shipAbility_[0] = battleShipInfo_.Hp;
            shipAbility_[1] = battleShipInfo_.BaseDamage;
            shipAbility_[2] = battleShipInfo_.Accuracy;
            shipAbility_[3] = battleShipInfo_.SideDefence;
            shipAbility_[4] = battleShipInfo_.TopDefence;
            shipAbility_[5] = battleShipInfo_.TorpedoDefence;
            shipAbility_[6] = battleShipInfo_.FireTime;
            shipAbility_[7] = battleShipInfo_.ReloadTime;
            shipAbility_[8] = battleShipInfo_.ShellCnt;

            // add stage value
            for (int i = 0; i < 6; ++i)
                shipAbility_[i] += BattleControl.Instance.excelDatas_.GetChapter(DataMgr.Instance.myInfo_g.currentChapter_).UpgradeValue;

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
                    currentShellCnt_ = (int)shipAbility_[(int)ShipAbility.ShellCnt];

                    BattleControl.Instance.playerShip_.StartFire();
                    StartFire();
                });
            }
            else if (shipState_ == ShipState.Clear)
            {
                LeanTween.move(this.gameObject, BattleControl.Instance.playerBattleClearPos_.transform.position, 1.0f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    Destroy(this.gameObject);
                    BattleControl.Instance.ReStartBattle();
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
        public void Damage(int damage, DamageType type)
        {
            if (shipState_ != ShipState.Battle)
                return;

            // make damage
            int formulaDamage = 0;

            if ((int)BattleControl.Instance.playerShip_.shipAbility_[(int)ShipAbility.Accuracy] >= Random.Range(0, 99))
            {
                if (type == DamageType.Line)
                {
                    formulaDamage = damage - (int)shipAbility_[(int)ShipAbility.SideDp];
                }
                else if (type == DamageType.Curve)
                {
                    formulaDamage = damage - (int)shipAbility_[(int)ShipAbility.TopDp];
                }
                else if (type == DamageType.Torpedo)
                {
                    formulaDamage = damage - (int)shipAbility_[(int)ShipAbility.TorpedoDp];
                }

                // limite min damage
                if (formulaDamage <= 0)
                    formulaDamage = 1;
            }
            else
            {
                // miss
            }



            shipAbility_[(int)ShipAbility.Hp] -= formulaDamage;
            if (shipAbility_[(int)ShipAbility.Hp] <= 0)
                shipAbility_[(int)ShipAbility.Hp] = 0;

            BattleControl.Instance.battleUI_.UpdateEnemyHp((int)shipAbility_[(int)ShipAbility.Hp]);
            BattleControl.Instance.CreateDamageEffect(this.gameObject.transform.position, formulaDamage);

            if (shipAbility_[(int)ShipAbility.Hp] <= 0)
            {
                shipState_ = ShipState.Die;
                BattleControl.Instance.playerShip_.HoldFire();
                BattleControl.Instance.DestroyEnemy();
                DataMgr.Instance.myInfo_g.AddMyGold(battleShipInfo_.GainGold);

                Invoke("Die", 0.3f);
            }
            else
            {
                if (formulaDamage > 0)
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
            currentShellCnt_ = (int)shipAbility_[(int)ShipAbility.ShellCnt];

            //
            StartFire();
        }

        IEnumerator CoroutineFire()
        {
            yield return new WaitForSeconds(shipAbility_[(int)ShipAbility.FireTime]);

            int fireCnt = guns_.Length;

            while (true)
            {
                //
                if (currentShellCnt_ <= 0)
                {
                    yield return null;

                    Invoke("ReloadShell", shipAbility_[(int)ShipAbility.ReloadTime]);

                    break;
                }

                //
                if (fireCnt <= 0)
                {
                    fireCnt = guns_.Length;

                    yield return new WaitForSeconds(shipAbility_[(int)ShipAbility.FireTime]);
                }

                //
                if (shipState_ == ShipState.Clear || shipState_ == ShipState.Die)
                {
                    yield return null;

                    break;
                }

                // to do : fire
                if (BattleControl.Instance.bPlayerShipReady_)
                {
                    GameObject bulletGO = (GameObject)Instantiate(bulletObject_, guns_[guns_.Length - fireCnt].transform.position, this.transform.rotation);
                    BulletBase bullet = bulletGO.GetComponent<BulletBase>();
                    if (bullet != null)
                    {
                        bulletTargetPos_.x = BattleControl.Instance.playerShip_.transform.position.x + Random.Range(-0.1f, 0.1f);
                        bulletTargetPos_.y = guns_[guns_.Length - fireCnt].transform.position.y;

                        bullet.Shoot(false, (int)shipAbility_[(int)ShipAbility.Ap], bulletTargetPos_);
                    }
                }
                else
                {
                    yield return null;

                    break;
                }



                //
                fireCnt--;
                currentShellCnt_--;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}