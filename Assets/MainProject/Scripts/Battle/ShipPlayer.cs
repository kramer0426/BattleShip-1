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
        public DamageType attackType_ = 0;

        //
        private Vector3 bulletTargetPos_ = new Vector3(0, 0, 0);
        private Coroutine fireCoroutine = null;
        private Coroutine planeCoroutine = null;

        //
        public int currentShellCnt_ = 0;
        public int maxHp_;
        public int passiveAp_;
        public int destroyEnemyCnt_;
        public float[] shipAbility_ = new float[(int)ShipAbility.MAX];
        public PassiveSkillEntity passiveInfo_ = null;
        private bool bUsedResurrection_ = false;

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

        //
        private void Awake()
        {
            currentShellCnt_ = 0;
            maxHp_ = 0;
            passiveAp_ = 0;
            destroyEnemyCnt_ = 0;
        }

        //----------------------------------------------------------------------------------------
        // SetBattleShip
        //----------------------------------------------------------------------------------------
        public void SetBattleShip(MyShipData battleShip, Vector3 startPos)
        {
            shipAbility_ = new float[(int)ShipAbility.MAX];
            battleShipData_ = battleShip;

            bUsedResurrection_ = false;

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
                else if (passiveInfo_.Type == (int)PassiveType.AttackUp || passiveInfo_.Type == (int)PassiveType.AttackUpAndMyDamageUp)
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
                else if (passiveInfo_.Type == (int)PassiveType.DefenseUpAndAvoidUp)
                {
                    shipAbility_[(int)ShipAbility.SideDp] += shipAbility_[(int)ShipAbility.SideDp] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.TopDp] += shipAbility_[(int)ShipAbility.TopDp] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.AvoidRate] += passiveInfo_.Value2;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUpAndAccuracyUp)
                {
                    shipAbility_[(int)ShipAbility.Ap] += shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.Accuracy] += shipAbility_[(int)ShipAbility.Accuracy] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.CriticalRateUpAndCriticalDamageUp)
                {
                    shipAbility_[(int)ShipAbility.CriticalRate] += shipAbility_[(int)ShipAbility.CriticalRate] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.CriticalDamage] += shipAbility_[(int)ShipAbility.CriticalDamage] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.MaxShellUpAndReloadDown)
                {
                    shipAbility_[(int)ShipAbility.ShellCnt] += shipAbility_[(int)ShipAbility.ShellCnt] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.ReloadTime] -= shipAbility_[(int)ShipAbility.ReloadTime] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.MaxShellUpAndReloadDown)
                {
                    shipAbility_[(int)ShipAbility.ShellCnt] += shipAbility_[(int)ShipAbility.ShellCnt] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.ReloadTime] -= shipAbility_[(int)ShipAbility.ReloadTime] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AllFleetHpUp)
                {
                    // to do : all fleet Hp Up
                }
                else if (passiveInfo_.Type == (int)PassiveType.AllFleetAttackUp)
                {
                    // to do : all fleet Attack Up
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUpAndHpUp)
                {
                    shipAbility_[(int)ShipAbility.Ap] += shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.Hp] += shipAbility_[(int)ShipAbility.Hp] * passiveInfo_.Value2 * 0.01f;
                }
                else if (passiveInfo_.Type == (int)PassiveType.AllFleetAvoidUp)
                {
                    // to do : all fleet avoid Up
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackDownAndHpDownAndAllFleetAttackUp)
                {
                    shipAbility_[(int)ShipAbility.Ap] -= shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f;
                    shipAbility_[(int)ShipAbility.Hp] -= shipAbility_[(int)ShipAbility.Hp] * passiveInfo_.Value2 * 0.01f;

                    // to do : all fleet attack Up
                }
                else if (passiveInfo_.Type == (int)PassiveType.AllFleetChaneCountryToSameEnemy)
                {
                    // to do : all fleet change county to same enemey
                }


                //
                if (shipAbility_[(int)ShipAbility.ReloadTime] < 0.1f)
                    shipAbility_[(int)ShipAbility.ReloadTime] = 0.1f;
            }

            //
            if ((int)shipAbility_[(int)ShipAbility.Hp] < 1)
                shipAbility_[(int)ShipAbility.Hp] = 1;

            if ((int)shipAbility_[(int)ShipAbility.Ap] < 1)
                shipAbility_[(int)ShipAbility.Ap] = 1;

            maxHp_ = (int)shipAbility_[(int)ShipAbility.Hp];
            if (passiveAp_ > 0)
                shipAbility_[(int)ShipAbility.Ap] = passiveAp_;
        }

        //----------------------------------------------------------------------------------------
        // ActivatePassiveByDestroyEnemyShip
        //----------------------------------------------------------------------------------------
        public void ActivatePassiveByDestroyEnemyShip()
        {
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.AllFleetHpRecoverIfEnemyDestroy)
                {
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 100))
                    {
                        shipAbility_[(int)ShipAbility.Hp] = maxHp_;
                    }
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUpOnesIfEnemyDestroy)
                {
                    if (passiveAp_ <= 0)
                    {
                        shipAbility_[(int)ShipAbility.Ap] += (shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f);
                        passiveAp_ = (int)shipAbility_[(int)ShipAbility.Ap];
                    }
                }
                else if (passiveInfo_.Type == (int)PassiveType.AttackUpAddedMax10IfEnemyDestroy)
                {
                    destroyEnemyCnt_++;
                    if (destroyEnemyCnt_ <= DataMgr.MAX_PASSIVE_DESTROY_ENEMY)
                    {
                        shipAbility_[(int)ShipAbility.Ap] += (shipAbility_[(int)ShipAbility.Ap] * passiveInfo_.Value1 * 0.01f);
                    }

                    passiveAp_ = (int)shipAbility_[(int)ShipAbility.Ap];
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
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 100))
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

            if (BattleControl.Instance.bEnemyShipReady_)
            {
                if ((int)BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.Accuracy] - (int)shipAbility_[(int)ShipAbility.AvoidRate] >= Random.Range(0, 100))
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

                    // passive
                    if (passiveInfo_.Type == (int)PassiveType.DamageDownIfSameCountry)
                    {
                        if (BattleControl.Instance.enemyShip_.battleShipInfo_.Country == battleShipData_.shipInfo_.Country)
                        {
                            formulaDamage -= (int)((float)formulaDamage * passiveInfo_.Value1 * 0.01f);
                        }
                    }
                    else if (passiveInfo_.Type == (int)PassiveType.AttackUpAndMyDamageUp)
                    {
                        formulaDamage += (int)((float)formulaDamage * passiveInfo_.Value1 * 0.01f);
                    }
                    else if (passiveInfo_.Type == (int)PassiveType.EnemyAttackReflection)
                    {
                        BattleControl.Instance.enemyShip_.Damage((int)((float)formulaDamage * passiveInfo_.Value1 * 0.01f), type);
                    }


                    // critical
                    if ((int)BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.CriticalRate] >= Random.Range(0, 100))
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

                // passive
                if (passiveInfo_.Type == (int)PassiveType.AvoidTorpedo)
                {
                    if ((int)passiveInfo_.Value1 >= Random.Range(0, 100))
                        formulaDamage = 0;
                }
                else if (passiveInfo_.Type == (int)PassiveType.TorpedoAttackUpAndAvoidTorpedo)
                {
                    if ((int)passiveInfo_.Value2 >= Random.Range(0, 100))
                        formulaDamage = 0;
                }
            }
            else
            {
                formulaDamage = 0;
            }

            //
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
            if (passiveInfo_ != null)
            {
                if (passiveInfo_.Type == (int)PassiveType.AllFleetHpRecoverIfDestroyMe)
                {
                    // to do : My fleect Hp recover
                }
                else if (passiveInfo_.Type == (int)PassiveType.EnemyDestroyIfDestroyMe)
                {
                    if (BattleControl.Instance.bEnemyShipReady_)
                        BattleControl.Instance.enemyShip_.DestroyPassiveEffect();
                }
                else if (passiveInfo_.Type == (int)PassiveType.Resurrection)
                {
                    if (bUsedResurrection_ == false)
                    {
                        bUsedResurrection_ = true;

                        shipAbility_[(int)ShipAbility.Hp] = maxHp_;

                        BattleControl.Instance.battleUI_.UpdatePlayerHp((int)shipAbility_[(int)ShipAbility.Hp]);
                        return;
                    }
                }
            }

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
                            if ((int)passiveInfo_.Value1 >= Random.Range(0, 100))
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
                            else if (passiveInfo_.Type == (int)PassiveType.AttackUpIfMyHighDefenseThenEnemyDefense)
                            {
                                if (shipAbility_[(int)ShipAbility.SideDp] > BattleControl.Instance.enemyShip_.shipAbility_[(int)ShipAbility.SideDp])
                                {
                                    ap += (int)((float)ap * passiveInfo_.Value1 * 0.01f);
                                }
                            }
                            else if (passiveInfo_.Type == (int)PassiveType.TorpedoAttackUpAndAvoidTorpedo)
                            {
                                if (attackType_ == DamageType.Torpedo)
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

                if (fireCnt <= 0)
                {
                    if (passiveInfo_ != null)
                    {
                        if (passiveInfo_.Type == (int)PassiveType.ContinueAttack)
                        {
                            if ((int)passiveInfo_.Value1 >= Random.Range(0, 100))
                            {
                                fireCnt = guns_.Length;
                            }
                        }
                    }
                }


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