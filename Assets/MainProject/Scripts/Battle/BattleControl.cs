using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class BattleControl : MonoBehaviour
    {
        //
        private static BattleControl instance = null;

        //
        public ExcelDatas   excelDatas_;
        public BattleUI     battleUI_;

        //
        public GameObject battleField_;
        public GameObject playerStartPos_;
        public GameObject playerBattleReadyPos_;
        public GameObject playerBattleClearPos_;
        public GameObject enemyStartPos_;
        public GameObject enemyBattleReadyPos_;
        public GameObject playerPlaneStartPos_;
        public GameObject enemyPlaneStartPos_;
        public GameObject damageNumberEffect_;

        public GameObject[] planePrefab_;

        //
        public ShipPlayer playerShip_;
        public ShipEnemy enemyShip_;


        //
        private bool bStart_;
        public bool bPlayerShipReady_;
        public bool bEnemyShipReady_;

        //
        public float battleTimeScale_ = 1.0f;

        //
        private Vector3 effectTargetPos_ = new Vector3(0, 0, 0);

        //-----------------------------------------------
        // Instance
        //-----------------------------------------------
        public static BattleControl Instance
        {
            get { return instance; }
        }

        //
        private void Awake()
        {
            //
            instance = this;

            //
            bStart_ = false;
            bPlayerShipReady_ = false;
            bEnemyShipReady_ = false;
        }

        private void Start()
        {
            Invoke("StartBattle", 0.1f);
            
        }

        //
        private void FixedUpdate()
        {
            if (bStart_)
            {

            }
        }

        //---------------------------------------------------------------------------------
        // StartBattle
        //---------------------------------------------------------------------------------
        public void StartBattle()
        {
            bStart_ = true;
            bPlayerShipReady_ = false;
            bEnemyShipReady_ = false;

            battleTimeScale_ = 1.0f;

            CreatePlayerShip();
            CreateEnemyShip();

            battleUI_.UpdateStageText();
        }

        //---------------------------------------------------------------------------------
        // CreatePlayerShip
        //---------------------------------------------------------------------------------
        private void CreatePlayerShip()
        {
            BattleShipEntity shipInfo = excelDatas_.GetBattleShip(0);
            if (shipInfo != null)
            {
                GameObject resAsset = Resources.Load<GameObject>("Ship/" + shipInfo.ResourceName);
                GameObject shipGO = (GameObject)Instantiate(resAsset, Vector3.zero, this.transform.rotation);
                playerShip_ = shipGO.GetComponent<ShipPlayer>();
                if (playerShip_ != null)
                {
                    shipGO.transform.SetParent(battleField_.transform);
                    playerShip_.SetBattleShip(shipInfo, playerStartPos_.transform.position);
                    playerShip_.Move();

                    bPlayerShipReady_ = true;
                }
            }

            battleUI_.UpdatePlayerHp((int)playerShip_.shipAbility_[(int)ShipAbility.Hp]);
            battleUI_.UpdatePlayerShell(playerShip_.currentShellCnt_);
        }

        //---------------------------------------------------------------------------------
        // CreateEnemyShip
        //---------------------------------------------------------------------------------
        private void CreateEnemyShip()
        {
            // to do : select enemy 0 - 8 -> only one, 9 -> next ship, and range under 2 step ship

            int createEnemyShipId = 0;
            if (DataMgr.Instance.myInfo_g.currentChapter_ < DataMgr.BOSS_CHAPTER)
            {
                createEnemyShipId = 0;
            }
            else if (DataMgr.Instance.myInfo_g.currentChapter_ == DataMgr.BOSS_CHAPTER)
            {
                createEnemyShipId = 1;
            }
            else
            {
                int stage = DataMgr.Instance.myInfo_g.currentChapter_ / DataMgr.MAX_CHAPTER;

                if (DataMgr.Instance.myInfo_g.currentChapter_ % DataMgr.MAX_CHAPTER == DataMgr.BOSS_CHAPTER)
                {
                    createEnemyShipId = stage + 1;
                }
                else
                {
                    int minEnemyLimite = stage - DataMgr.MIN_ENEMY_SHIP_LIMITE;
                    if (minEnemyLimite <= 0)
                        minEnemyLimite = 0;

                    createEnemyShipId = Random.Range(minEnemyLimite, stage + 1);
                }
            }


            BattleShipEnemyEntity shipInfo = excelDatas_.GetEnemyBattleShip(createEnemyShipId);
            if (shipInfo != null)
            {
                GameObject resAsset = Resources.Load<GameObject>("Ship/" + shipInfo.ResourceName);
                GameObject shipGO = (GameObject)Instantiate(resAsset, Vector3.zero, this.transform.rotation);
                enemyShip_ = shipGO.GetComponent<ShipEnemy>();
                if (enemyShip_ != null)
                {
                    shipGO.transform.SetParent(battleField_.transform);
                    enemyShip_.SetBattleShip(excelDatas_.GetEnemyBattleShip(0), enemyStartPos_.transform.position);
                    enemyShip_.Move();

                    bEnemyShipReady_ = true;
                }
            }

            battleUI_.UpdateEnemyHp((int)enemyShip_.shipAbility_[(int)ShipAbility.Hp]);
        }

        //---------------------------------------------------------------------------------
        // DestroyEnemy
        //---------------------------------------------------------------------------------
        public void DestroyEnemy()
        {
            bEnemyShipReady_ = false;

            DataMgr.Instance.myInfo_g.currentChapter_++;

            if (DataMgr.Instance.myInfo_g.currentChapter_ % DataMgr.MAX_CHAPTER == 0)
            {
                // go next stage : move right and Hp full up
                playerShip_.shipState_ = ShipState.Clear;
                playerShip_.Move();
            }
            else
            {
                Invoke("CreateEnemyShip", 1.0f / battleTimeScale_);
            }

            battleUI_.UpdateStageText();
        }

        //---------------------------------------------------------------------------------
        // DestroyPlayer
        //---------------------------------------------------------------------------------
        public void DestroyPlayer()
        {
            bPlayerShipReady_ = false;

            enemyShip_.shipState_ = ShipState.Clear;
            enemyShip_.Move();
        }

        //---------------------------------------------------------------------------------
        // ReStartBattle
        //---------------------------------------------------------------------------------
        public void ReStartBattle()
        {
            bPlayerShipReady_ = false;
            bEnemyShipReady_ = false;

            CreatePlayerShip();
            CreateEnemyShip();

            DataMgr.Instance.myInfo_g.currentChapter_ = DataMgr.Instance.myInfo_g.currentChapter_ / 10 * 10;
            DataMgr.Instance.SaveData();

            battleUI_.UpdateStageText();
        }

        //---------------------------------------------------------------------------------
        // GoNextStage
        //---------------------------------------------------------------------------------
        public void GoNextStage()
        {
            playerShip_.SetBattleShip(playerShip_.battleShipInfo_, playerStartPos_.transform.position);
            playerShip_.Move();

            bPlayerShipReady_ = true;

            battleUI_.UpdatePlayerHp((int)playerShip_.shipAbility_[(int)ShipAbility.Hp]);

            CreateEnemyShip();
        }

        //---------------------------------------------------------------------------------
        // HitToPlayer
        //---------------------------------------------------------------------------------
        public void HitToPlayer(int damage, DamageType type)
        {
            playerShip_.Damage(damage, type);
        }

        //---------------------------------------------------------------------------------
        // HitToEnemy
        //---------------------------------------------------------------------------------
        public void HitToEnemy(int damage, DamageType type)
        {
            enemyShip_.Damage(damage, type);
        }

        //---------------------------------------------------------------------------------
        // CallPlayerPlaneSupport
        //---------------------------------------------------------------------------------
        public void CallPlayerPlaneSupport()
        {
            StartCoroutine(CoroutineCallSupportPlane());
        }

        //
        public void CreateDamageEffect(Vector3 targetPos, int damge)
        {
            effectTargetPos_ = targetPos;
            effectTargetPos_.x += Random.Range(-0.1f, 0.1f);
            effectTargetPos_.y += Random.Range(0.1f, 0.2f);

            GameObject effectGO = (GameObject)Instantiate(damageNumberEffect_, effectTargetPos_, this.transform.rotation);
            DamageNumberEffect effect = effectGO.GetComponent<DamageNumberEffect>();
            if (effect != null)
            {
                effect.CreateFx(damge, false);
            }
        }

        //
        IEnumerator CoroutineCallSupportPlane()
        {
            int fireCnt = 5;
            int rndPlane = Random.Range(0, 2);

            while (true)
            {
                //
                if (fireCnt <= 0)
                {
                    yield return null;

                    break;
                }

                //
                if (BattleControl.Instance.bEnemyShipReady_)
                {
                    GameObject shipGO = (GameObject)Instantiate(planePrefab_[rndPlane], playerPlaneStartPos_.transform.position, this.transform.rotation);
                    PlaneBase plane = shipGO.GetComponent<PlaneBase>();
                    if (plane != null)
                    {
                        plane.SetPlane(true, 1);
                    }
                }
                else
                {
                    yield return null;

                    break;
                }



                //
                fireCnt--;

                yield return new WaitForSeconds(0.1f / BattleControl.Instance.battleTimeScale_);
            }
        }


    }
}