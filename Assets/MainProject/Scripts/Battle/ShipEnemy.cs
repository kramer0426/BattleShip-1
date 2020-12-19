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
        public int maxHp_;
        public float[] shipAbility_ = new float[(int)ShipAbility.MAX];
        public PassiveSkillEntity passiveInfo_ = null;

        //
        private void FixedUpdate()
        {
            if (shipState_ == ShipState.Battle)
            {

            }
        }

        //
        private void Awake()
        {
            currentShellCnt_ = 0;
            maxHp_ = 0;
        }

        //----------------------------------------------------------------------------------------
        // SetBattleShip
        //----------------------------------------------------------------------------------------
        public void SetBattleShip(BattleShipEnemyEntity battleShip, Vector3 startPos)
        {
            // setting passive
            string[] values = null;
            values = battleShip.PassiveIds.Split(',');
            List<int> passiveIds = new List<int>();
            if (values != null)
            {
                for (int i = 0; i < values.Length; ++i)
                {
                    if (values[i].Length != 0) passiveIds.Add(int.Parse(values[i]));
                }
            }
            passiveInfo_ = BattleControl.Instance.excelDatas_.GetPassiveSkill(passiveIds[Random.Range(0, passiveIds.Count)]);


            //
            battleShipInfo_ = battleShip;
            shipState_ = ShipState.Start;
            transform.position = startPos;

            shipAbility_ = new float[(int)ShipAbility.MAX];
            shipAbility_[0] = battleShipInfo_.Hp;
            shipAbility_[1] = battleShipInfo_.BaseDamage;
            shipAbility_[2] = battleShipInfo_.Accuracy;
            shipAbility_[3] = battleShipInfo_.SideDefence;
            shipAbility_[4] = battleShipInfo_.TopDefence;
            shipAbility_[5] = 0;                                        // torpedo damage down
            shipAbility_[6] = battleShipInfo_.FireTime;
            shipAbility_[7] = battleShipInfo_.ReloadTime;
            shipAbility_[8] = battleShipInfo_.ShellCnt;
            shipAbility_[9] = battleShipInfo_.CriticalRate;
            shipAbility_[10] = battleShipInfo_.CriticalDamage;
            shipAbility_[11] = 0;                                       // avoid rate

            // add stage value
            int upgradeValue = BattleControl.Instance.excelDatas_.GetChapter(DataMgr.Instance.myInfo_g.currentChapter_).UpgradeValue;
            shipAbility_[(int)ShipAbility.Hp] += upgradeValue;
            shipAbility_[(int)ShipAbility.Ap] += upgradeValue;

            // add passive


            //
            maxHp_ = (int)shipAbility_[(int)ShipAbility.Hp];

            BattleControl.Instance.battleUI_.UpdateEnemyHp((int)shipAbility_[(int)ShipAbility.Hp], maxHp_);
        }

        //----------------------------------------------------------------------------------------
        // ActivatePassiveByPlayerShip
        //----------------------------------------------------------------------------------------
        public void ActivatePassiveByPlayerShip()
        {

        }

        //----------------------------------------------------------------------------------------
        // ActivatePassiveByEnemyPlane
        //----------------------------------------------------------------------------------------
        public void ActivatePassiveByPlayerPlane(PlaneBase targetPlane)
        {

        }

        //----------------------------------------------------------------------------------------
        // Move
        //----------------------------------------------------------------------------------------
        public void Move()
        {
            if (shipState_ == ShipState.Start)
            {
                LeanTween.move(this.gameObject, BattleControl.Instance.enemyBattleReadyPos_.transform.position, 1.0f / BattleControl.Instance.battleTimeScale_).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    shipState_ = ShipState.Battle;
                    currentShellCnt_ = (int)shipAbility_[(int)ShipAbility.ShellCnt];

                    if (BattleControl.Instance.bPlayerShipReady_)
                    {
                        if (BattleControl.Instance.playerShip_.passiveInfo_ != null)
                        {
                            if (BattleControl.Instance.playerShip_.passiveInfo_.Type == (int)PassiveType.EnemyMaxHpDown)
                            {
                                if (BattleControl.Instance.playerShip_.battleShipData_.shipInfo_.Country != battleShipInfo_.Country)
                                {
                                    shipAbility_[(int)ShipAbility.Hp] -= shipAbility_[(int)ShipAbility.Hp] * BattleControl.Instance.playerShip_.passiveInfo_.Value1 * 0.01f;

                                    BattleControl.Instance.battleUI_.UpdateEnemyHp((int)shipAbility_[(int)ShipAbility.Hp], maxHp_);
                                }
                            }
                        }
                    }

                    BattleControl.Instance.playerShip_.StartFire();
                    StartFire();
                });
            }
            else if (shipState_ == ShipState.Clear)
            {
                LeanTween.move(this.gameObject, BattleControl.Instance.playerBattleClearPos_.transform.position, 1.0f / BattleControl.Instance.battleTimeScale_).setEaseInOutQuad().setOnComplete(
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
            int accuracy = (int)BattleControl.Instance.playerShip_.shipAbility_[(int)ShipAbility.Accuracy];
            // player passive
            if (BattleControl.Instance.bPlayerShipReady_)
            {
                if (BattleControl.Instance.playerShip_.passiveInfo_ != null)
                {
                    if (BattleControl.Instance.playerShip_.passiveInfo_.Type == (int)PassiveType.CompleteAccuracy)
                    {
                        if ((int)BattleControl.Instance.playerShip_.passiveInfo_.Value1 >= Random.Range(0, 100))
                        {
                            accuracy = 100000000;   // make 100% accuracy
                        }
                    }
                }
            }

            if (accuracy - (int)shipAbility_[(int)ShipAbility.AvoidRate] >= Random.Range(0, 99))
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

                // critical
                if ((int)BattleControl.Instance.playerShip_.shipAbility_[(int)ShipAbility.CriticalRate] >= Random.Range(0, 99))
                {
                    Debug.Log("Critical to enemy");
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

            BattleControl.Instance.battleUI_.UpdateEnemyHp((int)shipAbility_[(int)ShipAbility.Hp], maxHp_);
            BattleControl.Instance.CreateDamageEffect(this.gameObject.transform.position, formulaDamage);

            if (shipAbility_[(int)ShipAbility.Hp] <= 0)
            {
                shipState_ = ShipState.Die;
                BattleControl.Instance.playerShip_.HoldFire();
                BattleControl.Instance.DestroyEnemy();

                int gainGold = battleShipInfo_.GainGold;
                if (BattleControl.Instance.bPlayerShipReady_)
                {
                    if (BattleControl.Instance.playerShip_.passiveInfo_ != null)
                    {
                        if (BattleControl.Instance.playerShip_.passiveInfo_.Type == (int)PassiveType.GainGoldUpIfEnemyDestroy)
                        {
                            gainGold += (int)((float)gainGold * BattleControl.Instance.playerShip_.passiveInfo_.Value1 * 0.01f);
                        }
                        else if (BattleControl.Instance.playerShip_.passiveInfo_.Type == (int)PassiveType.GainCashIfEnemyDestroy)
                        {
                            if ((int)BattleControl.Instance.playerShip_.passiveInfo_.Value1 >= Random.Range(0, 100))
                            {
                                DataMgr.Instance.myInfo_g.AddMyCash((int)BattleControl.Instance.playerShip_.passiveInfo_.Value2);
                            }
                        }
                    }
                }

                DataMgr.Instance.myInfo_g.AddMyGold(gainGold);

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

        //----------------------------------------------------------------------------------------
        // DestroyPassiveEffect
        //----------------------------------------------------------------------------------------
        public void DestroyPassiveEffect()
        {
            Invoke("Die", 0.3f / BattleControl.Instance.battleTimeScale_);
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

                    yield return new WaitForSeconds(shipAbility_[(int)ShipAbility.FireTime] / BattleControl.Instance.battleTimeScale_);
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

                yield return new WaitForSeconds(0.1f / BattleControl.Instance.battleTimeScale_);
            }
        }
    }
}