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

        //
        public ShipPlayer playerShip_;
        public ShipEnemy enemyShip_;


        //
        private bool bStart_;

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

            CreatePlayerShip();

            CreateEnemyShip();

            battleUI_.UpdateStageText();
            battleUI_.UpdatePlayerHp(playerShip_.hp_);
            battleUI_.UpdateEnemyHp(enemyShip_.hp_);
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
                }
            }

        }

        //---------------------------------------------------------------------------------
        // CreateEnemyShip
        //---------------------------------------------------------------------------------
        private void CreateEnemyShip()
        {
            // to do : select enemy 0 - 8 -> only one, 9 -> next ship, and range under 2 step ship

            BattleShipEnemyEntity shipInfo = excelDatas_.GetEnemyBattleShip(0);
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
                }
            }
        }

        //---------------------------------------------------------------------------------
        // DestroyEnemy
        //---------------------------------------------------------------------------------
        public void DestroyEnemy()
        {

        }

        //---------------------------------------------------------------------------------
        // DestroyPlayer
        //---------------------------------------------------------------------------------
        public void DestroyPlayer()
        {

        }


        //---------------------------------------------------------------------------------
        // HitToPlayer
        //---------------------------------------------------------------------------------
        public void HitToPlayer(int damage)
        {
            playerShip_.Damage(damage);
        }

        //---------------------------------------------------------------------------------
        // HitToEnemy
        //---------------------------------------------------------------------------------
        public void HitToEnemy(int damage)
        {
            enemyShip_.Damage(damage);
        }


    }
}