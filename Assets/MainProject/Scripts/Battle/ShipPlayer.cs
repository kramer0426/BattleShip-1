using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShipPlayer : MonoBehaviour
    {
        //
        public ShipState shipState_;
        public MyShipData battleShipData_ = null;
        public ShakeControl shakeControl_;
        public GameObject bulletObject_;
        public GameObject[] guns_;

        //
        private Vector3 bulletTargetPos_ = new Vector3(0, 0, 0);
        private Coroutine fireCoroutine = null;
        private Coroutine planeCoroutine = null;

        //
        public int currentShellCnt_ = 0;
        public int maxHp_;
        public int maxAp_;
        public float[] shipAbility_ = new float[(int)ShipAbility.MAX];
        public PassiveSkillEntity passiveInfo_ = null;

        //
        private void FixedUpdate()
        {
            //if (shipState_ == ShipState.Battle)
            //{
            //    playTime += Time.deltaTime * speed;
            //    shipPos.y += Mathf.Sin(playTime) * 0.001f;
            //    transform.position = shipPos;
            //}
        }

        //----------------------------------------------------------------------------------------
        // SetBattleShip
        //----------------------------------------------------------------------------------------
        public void SetBattleShip(MyShipData battleShip, Vector3 startPos)
        {
            shipAbility_ = new float[(int)ShipAbility.MAX];
            battleShipData_ = battleShip;

            //
            passiveInfo_ = BattleControl.Instance.excelDatas_.GetPassiveSkill(battleShipData_.passiveId_);

            //
            shipState_ = ShipState.Start;
            transform.position = startPos;

            // to do : add upgrade value
            shipAbility_[0] = battleShip.shipInfo_.hp;
            shipAbility_[1] = battleShip.shipInfo_.BaseDamage;
            shipAbility_[2] = battleShip.shipInfo_.Accuracy;
            shipAbility_[3] = battleShip.shipInfo_.SideDefence;
            shipAbility_[4] = battleShip.shipInfo_.TopDefence;
            shipAbility_[5] = 0;                                        // torpedo damage down
            shipAbility_[6] = battleShip.shipInfo_.FireTime;
            shipAbility_[7] = battleShip.shipInfo_.ReloadTime;
            shipAbility_[8] = battleShip.shipInfo_.ShellCnt;
            shipAbility_[9] = battleShip.shipInfo_.CriticalRate;
            shipAbility_[10] = battleShip.shipInfo_.CriticalDamage;
            shipAbility_[11] = 0;                                       // avoid rate

            // add passive value
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.DefenseUp)
                {
                    shipAbility_[(int)ShipAbility.SideDp] += shipAbility_[(int)ShipAbility.SideDp] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.TopDp] += shipAbility_[(int)ShipAbility.TopDp] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUp)
                {
                    shipAbility_[(int)ShipAbility.Ap] += shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.CriticalRateUp)
                {
                    shipAbility_[(int)ShipAbility.CriticalRate] += shipAbility_[(int)ShipAbility.CriticalRate] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.TorpedoDamageDown)
                {
                    shipAbility_[(int)ShipAbility.TorpedoDp] += passiveInfo_.Value1;
                }
                else if (passiveInfo_.Type == (int)PassiveType.ReloadDown)
                {
                    shipAbility_[(int)ShipAbility.ReloadTime] -= shipAbility_[(int)ShipAbility.ReloadTime] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AccuracyUp)
                {
                    shipAbility_[(int)ShipAbility.Accuracy] += shipAbility_[(int)ShipAbility.Accuracy] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.HpUp)
                {
                    shipAbility_[(int)ShipAbility.Hp] += shipAbility_[(int)ShipAbility.Hp] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AllDamageDownAndAttackDown)
                {
                    shipAbility_[(int)ShipAbility.SideDp] += shipAbility_[(int)ShipAbility.SideDp] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.TopDp] += shipAbility_[(int)ShipAbility.TopDp] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.TorpedoDp] += passiveInfo_.Value1;

                    shipAbility_[(int)ShipAbility.Ap] -= shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AvoidRateUp)
                {
                    shipAbility_[(int)ShipAbility.AvoidRate] += passiveInfo_.Value1;
                }
                else if (passiveInfo_.Type == (int)PassiveType.MaxShellUp)
                {
                    shipAbility_[(int)ShipAbility.ShellCnt] += shipAbility_[(int)ShipAbility.ShellCnt] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.CriticalDamageUp)
                {
                    shipAbility_[(int)ShipAbility.CriticalDamage] += shipAbility_[(int)ShipAbility.CriticalDamage] * passiveInfo_.Value1 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.PlaneSupport)
                {
                    StartPlane();
                }

                //
                if (shipAbility_[(int)ShipAbility.ReloadTime] < 0.1f)
                    shipAbility_[(int)ShipAbility.ReloadTime] = 0.1f;
            }

            //
            maxHp_ = (int)shipAbility_[(int)ShipAbility.Hp];
            maxAp_ = (int)shipAbility_[(int)ShipAbility.Ap];
        }

        //----------------------------------------------------------------------------------------
        // ActivatePassiveByEnemyShip
        //----------------------------------------------------------------------------------------
        public void ActivatePassiveByEnemyShip()
        {
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.AllFleetHpRecoverIfEnemyDestroy)
                {
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 99))
                    {
                        shipAbility_[(int)ShipAbility.Hp] = maxHp_;
                    }
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUpOnesIfEnemyDestroy)
                {
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 99))
                    {
                        shipAbility_[(int)ShipAbility.Ap] = maxAp_ + (maxAp_ * passiveInfo_.Value1 * 0.01f);
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------
        // ActivatePassiveByEnemyPlane
        //----------------------------------------------------------------------------------------
        public void ActivatePassiveByEnemyPlane(PlaneBase targetPlane)
        {
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.PlaneShootingDownRate)
                {
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 99))
                    {
                        targetPlane.Die();
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------
        // Move
        //----------------------------------------------------------------------------------------
        public void Move()
        {
            if (shipState_ == ShipState.Start)
            {
                LeanTween.move(this.gameObject, BattleControl.Instance.playerBattleReadyPos_.transform.position, 1.0f / BattleControl.Instance.battleTimeScale_).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    shipState_ = ShipState.Battle;

                    currentShellCnt_ = (int)shipAbility_[(int)ShipAbility.ShellCnt];
                });
            }
            else if (shipState_ == ShipState.Clear)
            {
                HoldPlane();

                LeanTween.move(this.gameObject, BattleControl.Instance.playerBattleClearPos_.transform.position, 3.0f / BattleControl.Instance.battleTimeScale_).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    BattleControl.Instance.GoNextStage();
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
        // StartPlane
        //----------------------------------------------------------------------------------------
        public void StartPlane()
        {
            if (planeCoroutine != null)
            {
                StopCoroutine(planeCoroutine);
            }

            planeCoroutine = StartCoroutine(CoroutineCallPlane());
        }

        //----------------------------------------------------------------------------------------
        // HoldPlane
        //----------------------------------------------------------------------------------------
        public void HoldPlane()
        {
            if (planeCoroutine != null)
            {
                StopCoroutine(planeCoroutine);
            }
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

            if ((int)BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.Accuracy] - (int)shipAbility_[(int)ShipAbility.AvoidRate] >= Random.Range(0, 99))
            {
                // default formula
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
                    formulaDamage = damage - (int)((float)damage * shipAbility_[(int)ShipAbility.TorpedoDp] * 0.01f);
                }

                // critical
                if ((int)BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.CriticalRate] >= Random.Range(0, 99))
                {
                    Debug.Log("Critical to player");
                    formulaDamage += (int)((float)formulaDamage * BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.CriticalDamage] * 0.01f);
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

            BattleControl.Instance.battleUI_.UpdatePlayerHp((int)shipAbility_[(int)ShipAbility.Hp]);
            BattleControl.Instance.CreateDamageEffect(this.gameObject.transform.position, formulaDamage);

            if (shipAbility_[(int)ShipAbility.Hp] <= 0)
            {
                shipState_ = ShipState.Die;
                BattleControl.Instance.DestroyPlayer();
                HoldPlane();
                Invoke("Die", 0.3f / BattleControl.Instance.battleTimeScale_);
            }
            else
            {
                if (formulaDamage > 0)
                    StartCoroutine(shakeControl_.Shake(this.gameObject.transform.position, 0.2f / BattleControl.Instance.battleTimeScale_, 0.02f));
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
            BattleControl.Instance.battleUI_.UpdatePlayerShell(currentShellCnt_);

            //
            StartFire();
        }

        IEnumerator CoroutineFire()
        {
            yield return new WaitForSeconds(shipAbility_[(int)ShipAbility.FireTime] / BattleControl.Instance.battleTimeScale_);

            int fireCnt = guns_.Length;

            while (true)
            {
                //
                if (currentShellCnt_ <= 0)
                {
                    yield return null;

                    Invoke("ReloadShell", shipAbility_[(int)ShipAbility.ReloadTime] / BattleControl.Instance.battleTimeScale_);

                    break;
                }

                //
                if (fireCnt <= 0)
                {
                    fireCnt = guns_.Length;

                    if (passiveInfo_ != null)
                    {
                        if (passiveInfo_.Type == (int)PassiveType.HpRecoverByAttack)
                        {
                            if ((int)passiveInfo_.Value1 >= Random.Range(0, 99))
                            {
                                shipAbility_[(int)ShipAbility.Hp] += shipAbility_[(int)ShipAbility.Hp] * passiveInfo_.Value2 * 0.01f;

                                if (maxHp_ < shipAbility_[(int)ShipAbility.Hp])
                                    shipAbility_[(int)ShipAbility.Hp] = maxHp_;

                                BattleControl.Instance.battleUI_.UpdatePlayerHp((int)shipAbility_[(int)ShipAbility.Hp]);
                            }
                        }
                    }

                    yield return new WaitForSeconds(shipAbility_[(int)ShipAbility.FireTime] / BattleControl.Instance.battleTimeScale_);
                }

                //
                if (shipState_ == ShipState.Clear || shipState_ == ShipState.Die)
                {
                    yield return null;

                    break;
                }

                // to do : fire
                if (BattleControl.Instance.bEnemyShipReady_)
                {
                    GameObject bulletGO = (GameObject)Instantiate(bulletObject_, guns_[guns_.Length - fireCnt].transform.position, this.transform.rotation);
                    BulletBase bullet = bulletGO.GetComponent<BulletBase>();
                    if (bullet != null)
                    {
                        bulletTargetPos_.x = BattleControl.Instance.enemyShip_.transform.position.x + Random.Range(-0.1f, 0.1f);
                        bulletTargetPos_.y = guns_[guns_.Length - fireCnt].transform.position.y;


                        int ap = (int)shipAbility_[(int)ShipAbility.Ap];

                        if (passiveInfo_ != null)
                        {
                            if (passiveInfo_.Type == (int)PassiveType.AttackUpForBoss)
                            {
                                if (DataMgr.Instance.myInfo_g.currentChapter_ % DataMgr.MAX_CHAPTER == DataMgr.BOSS_CHAPTER)
                                {
                                    ap += (int)((float)ap * passiveInfo_.Value1 * 0.01f);
                                }
                            }
                            else if (passiveInfo_.Type == (int)PassiveType.AttackUpIfOtherCountry)
                            {
                                if (battleShipData_.shipInfo_.Country != BattleControl.Instance.enemyShip_.battleShipInfo_.Country)
                                {
                                    ap += (int)((float)ap * passiveInfo_.Value1 * 0.01f);
                                }
                            }
                        }

                        bullet.Shoot(true, ap, bulletTargetPos_);
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

                BattleControl.Instance.battleUI_.UpdatePlayerShell(currentShellCnt_);

                yield return new WaitForSeconds(0.1f / BattleControl.Instance.battleTimeScale_);
            }
        }

        //
        IEnumerator CoroutineCallPlane()
        {
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.PlaneSupport)
                {
                    yield return new WaitForSeconds(passiveInfo_.Value1 / BattleControl.Instance.battleTimeScale_);
                }
            }
            else
            {
                yield return null;

            }

            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.PlaneSupport)
                {
                    BattleControl.Instance.CallPlayerPlaneSupport(true, (int)shipAbility_[(int)ShipAbility.Ap]);
                }
            }
            
        }
    }
}