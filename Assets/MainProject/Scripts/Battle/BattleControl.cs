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
        public GameObject playerStartPos_;
        public GameObject playerBattleReadyPos_;
        public GameObject playerBattleClearPos_;
        public GameObject enemyStartPos_;
        public GameObject enemyBattleReadyPos_;

        //
        public ShipBase playerShip_;
        public ShipBase enemyShip_;


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

            playerShip_.SetBattleShip(null);
            playerShip_.transform.position = playerStartPos_.transform.position;
            playerShip_.Move(playerBattleReadyPos_.transform.position);

            enemyShip_.SetBattleShip(null);
            enemyShip_.transform.position = enemyStartPos_.transform.position;
            enemyShip_.Move(enemyBattleReadyPos_.transform.position);
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