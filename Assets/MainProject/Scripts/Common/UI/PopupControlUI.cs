using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sinabro
{
    public class PopupControlUI : MonoBehaviour
    {
        public GameObject popupUI;

        public GameObject confirmButton;
        public GameObject cancelButton;

        public Text titleText_;
        public Text msgText_;
        public Text confirmText_;
        public Text cancelText_;

        public event Action OnConfirmCallBack = delegate { };

        //
        private Vector3 leftPos = new Vector3(-200, -150, 0);
        private Vector3 rightPos = new Vector3(200, -150, 0);
        private Vector3 centerPos = new Vector3(0, -195, 0);

        //------------------------------------------------------------------------
        // ShowPopupMsg
        //------------------------------------------------------------------------
        public void ShowPopupMsg(string titleText, string msgText, string confirmText, string cancelText, bool bConfirmOnly)
        {
            popupUI.SetActive(true);

            titleText_.text = titleText;
            msgText_.text = msgText;
            confirmText_.text = confirmText;
            cancelText_.text = cancelText;

            if (bConfirmOnly)
            {
                confirmButton.transform.localPosition = centerPos;
                cancelButton.SetActive(false);
            }
            else
            {
                cancelButton.transform.localPosition = leftPos;
                confirmButton.transform.localPosition = rightPos;
                cancelButton.SetActive(true);
            }
        }

        //------------------------------------------------------------------------
        // OnConfirmButton
        //------------------------------------------------------------------------
        public void OnConfirmButton()
        {
            popupUI.SetActive(false);

            OnConfirmCallBack();
        }

        //------------------------------------------------------------------------
        // OnCancelButton
        //------------------------------------------------------------------------
        public void OnCancelButton()
        {
            popupUI.SetActive(false);
        }
    }
}