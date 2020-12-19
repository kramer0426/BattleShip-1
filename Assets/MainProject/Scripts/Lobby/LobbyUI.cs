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
            currentMenuState_ = MenuState.Ship;

            //
            SetLobbyUI();
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