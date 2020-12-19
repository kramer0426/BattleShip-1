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
        public Image PlayerHpBar_;
        public Image enemyHpBar_;

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
        public void SetPlayerInfo()
        {

        }

        //
        public void SetEnemyInfo()
        {

        }

        //
        public void UpdatePlayerHp(int hp, int maxHp)
        {
            float percent = (float)hp / (float)maxHp;
            playerHpText_.text = "" + (int)(percent * 100.0f) + "%";
            PlayerHpBar_.fillAmount = percent;
        }

        //
        public void UpdateEnemyHp(int hp, int maxHp)
        {
            float percent = (float)hp / (float)maxHp;
            enemyHpText_.text = "" + (int)(percent * 100.0f) + "%";
            enemyHpBar_.fillAmount = percent;
        }

        //
        public void UpdatePlayerShell(int Shell)
        {
            //if (Shell <= 0)
            //{
            //    playerShellText_.text = "Shell : Reloading...";
            //}
            //else
            //{
            //    playerShellText_.text = "Shell : " + Shell;
            //}
            
        }
    }
}


