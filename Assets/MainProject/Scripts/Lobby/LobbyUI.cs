using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sinabro
{
    public class LobbyUI : MonoBehaviour
    {
        //
        public const int MAX_MAIN_MENU = 6;

        //
        public Text[] mainMenuTexts_;

        //
        public MenuState currentMenuState_;

        //
        private static LobbyUI instance = null;

        //-----------------------------------------------
        // Instance
        //-----------------------------------------------
        public static LobbyUI Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            //
            instance = this;
        }

        //
        void Start()
        {
            //
            int firstPlay = PlayerPrefs.GetInt("FirstPlay", 0);
            if (firstPlay == 0)
            {
                DataMgr.Instance.myInfo_g.myShipDataList_.Clear();
                MyShipData shipData = new MyShipData();
                shipData.shipId_ = 1;
                shipData.fleetIndex_ = 0;
                shipData.upgradeLevel_ = 0;
                shipData.shipInfo_ = DataMgr.Instance.GetBattleShip(shipData.shipId_);
                shipData.passiveId_ = 30;
                DataMgr.Instance.myInfo_g.myShipDataList_.Add(shipData);

                PlayerPrefs.SetInt("FirstPlay", 1);
            }
            else
            {
                DataMgr.Instance.LoadData();
            }

            //
            currentMenuState_ = MenuState.Ship;

            //
            SetLobbyUI();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F3))
            {
                DataMgr.Instance.myInfo_g.ResetMyData();

                currentMenuState_ = MenuState.Ship;

                SetLobbyUI();
            }
            else if (Input.GetKeyUp(KeyCode.F5))
            {
                currentMenuState_ = MenuState.Ship;

                DataMgr.Instance.myInfo_g.AddMyGold(100000);

                SetLobbyUI();
            }
        }

        //
        public void SetLobbyUI()
        {
            for (int i = 0; i < MAX_MAIN_MENU; ++i)
            {
                mainMenuTexts_[i].text = DataMgr.Instance.GetLocalExcelText(DataMgr.TEXT_MAIN_MENU_1 + i);
            }

            if (currentMenuState_ == MenuState.Ship)
            {

            }
        }

        //


        //
        public void OnMainMenuButton(int menuIndex)
        {
            if ((int)currentMenuState_ == menuIndex)
                return;

            currentMenuState_ = (MenuState)menuIndex;

            SetLobbyUI();
        }


    }
}