using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sinabro
{
    public class BattleUI : MonoBehaviour
    {
        public Text playerHpText_;
        public Text enemyHpText_;
        public Text stageText_;
        public Text playerShellText_;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //
        public void UpdateStageText()
        {
            int stage = 0;
            int chapter = 0;

            chapter = (DataMgr.Instance.myInfo_g.currentChapter_ % 10) + 1;
            stage = (DataMgr.Instance.myInfo_g.currentChapter_) / 10 + 1;

            if (stage == 0)
            {
                stageText_.text = "Stage 1 - " + chapter;
            }
            else
            {
                stageText_.text = "Stage " + stage + " - " + chapter;
            }

        }

        //
        public void UpdatePlayerHp(int hp)
        {
            playerHpText_.text = "Player HP : " + hp;
        }

        //
        public void UpdateEnemyHp(int hp)
        {
            enemyHpText_.text = "Enemy HP : " + hp;
        }

        //
        public void UpdatePlayerShell(int Shell)
        {
            if (Shell <= 0)
            {
                playerShellText_.text = "Shell : Reloading...";
            }
            else
            {
                playerShellText_.text = "Shell : " + Shell;
            }
            
        }

        //
        public void OnPlayerPlaneSupport()
        {
            BattleControl.Instance.CallPlayerPlaneSupport();
        }
    }
}


