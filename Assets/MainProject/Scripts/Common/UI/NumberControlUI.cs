using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class NumberControlUI : MonoBehaviour
    {
        public SpriteRenderer[] numbers;

        public GameObject numberRoot;

        private Vector3 numberScale = new Vector3(1.0f, 1.0f, 1.0f);
        private Vector3 numberPosition_ = new Vector3(0.0f, 0.0f, 0);

        private Vector3 numberPosition1_ = new Vector3(0, 0.5f, 0);
        private Vector3 numberPosition2_ = new Vector3(-0.1f, 0.5f, 0);
        private Vector3 numberPosition3_ = new Vector3(0.1f, 0.5f, 0);
        private Vector3 numberPosition4_ = new Vector3(-0.2f, 0.5f, 0);
        private Vector3 numberPosition5_ = new Vector3(0.2f, 0.5f, 0);
        private Vector3 numberPosition6_ = new Vector3(-0.3f, 0.5f, 0);
        private Vector3 numberPosition7_ = new Vector3(0.3f, 0.5f, 0);
        private Vector3 numberPosition8_ = new Vector3(-0.4f, 0.5f, 0);
        private Vector3 numberPosition9_ = new Vector3(0.4f, 0.5f, 0);
        private Vector3 numberPosition10_ = new Vector3(-0.5f, 0.5f, 0);
        private Vector3 numberPosition11_ = new Vector3(0.5f, 0.5f, 0);
        private Vector3 numberPosition12_ = new Vector3(-0.6f, 0.5f, 0);
        private Vector3 numberPosition13_ = new Vector3(0.6f, 0.5f, 0);
        private Vector3 numberPosition14_ = new Vector3(-0.7f, 0.5f, 0);
        private Vector3 numberPosition15_ = new Vector3(0.7f, 0.5f, 0);
        private Vector3 numberPosition16_ = new Vector3(-0.8f, 0.5f, 0);
        private Vector3 numberPosition17_ = new Vector3(0.8f, 0.5f, 0);

        //
        public void SetColor(Color numberColor)
        {
            for (int i = 0; i < 9; ++i)
                numbers[i].color = numberColor;
        }

        //
        public void SetDrawOrder(int addOrder)
        {
            for (int i = 0; i < 9; ++i)
                numbers[i].sortingOrder -= addOrder;
        }

        //
        public void SetNumner(int number)
        {
            for (int i = 0; i < 9; ++i)
                numbers[i].gameObject.SetActive(false);

            numberScale.x = numberScale.y = 1.0f;
            numberPosition_.y = 0.0f;
            numberRoot.transform.localScale = numberScale;
            numberRoot.transform.localPosition = numberPosition_;

            int positionCnt = 1;
            if (number < 10)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition1_;

                positionCnt = 1;
            }
            else if (number >= 10 && number < 100)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition2_;
                numbers[1].gameObject.transform.localPosition = numberPosition3_;

                positionCnt = 2;
            }
            else if (number >= 100 && number < 1000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition4_;
                numbers[1].gameObject.transform.localPosition = numberPosition1_;
                numbers[2].gameObject.transform.localPosition = numberPosition5_;

                positionCnt = 3;
            }
            else if (number >= 1000 && number < 10000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition6_;
                numbers[1].gameObject.transform.localPosition = numberPosition2_;
                numbers[2].gameObject.transform.localPosition = numberPosition3_;
                numbers[3].gameObject.transform.localPosition = numberPosition7_;

                positionCnt = 4;
            }
            else if (number >= 10000 && number < 100000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[4].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition8_;
                numbers[1].gameObject.transform.localPosition = numberPosition4_;
                numbers[2].gameObject.transform.localPosition = numberPosition1_;
                numbers[3].gameObject.transform.localPosition = numberPosition5_;
                numbers[4].gameObject.transform.localPosition = numberPosition9_;

                positionCnt = 5;
            }
            else if (number >= 100000 && number < 1000000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[4].gameObject.SetActive(true);
                numbers[5].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition10_;
                numbers[1].gameObject.transform.localPosition = numberPosition6_;
                numbers[2].gameObject.transform.localPosition = numberPosition2_;
                numbers[3].gameObject.transform.localPosition = numberPosition3_;
                numbers[4].gameObject.transform.localPosition = numberPosition7_;
                numbers[5].gameObject.transform.localPosition = numberPosition11_;

                positionCnt = 6;

                numberScale.x = numberScale.y = 0.9f;
                numberPosition_.y = 0.0f;
                numberRoot.transform.localScale = numberScale;
                numberRoot.transform.localPosition = numberPosition_;
            }
            else if (number >= 1000000 && number < 10000000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[4].gameObject.SetActive(true);
                numbers[5].gameObject.SetActive(true);
                numbers[6].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition12_;
                numbers[1].gameObject.transform.localPosition = numberPosition8_;
                numbers[2].gameObject.transform.localPosition = numberPosition4_;
                numbers[3].gameObject.transform.localPosition = numberPosition1_;
                numbers[4].gameObject.transform.localPosition = numberPosition5_;
                numbers[5].gameObject.transform.localPosition = numberPosition9_;
                numbers[6].gameObject.transform.localPosition = numberPosition13_;

                positionCnt = 7;

                numberScale.x = numberScale.y = 0.8f;
                numberPosition_.y = 0.05f;
                numberRoot.transform.localScale = numberScale;
                numberRoot.transform.localPosition = numberPosition_;
            }
            else if (number >= 10000000 && number < 100000000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[4].gameObject.SetActive(true);
                numbers[5].gameObject.SetActive(true);
                numbers[6].gameObject.SetActive(true);
                numbers[7].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition14_;
                numbers[1].gameObject.transform.localPosition = numberPosition10_;
                numbers[2].gameObject.transform.localPosition = numberPosition6_;
                numbers[3].gameObject.transform.localPosition = numberPosition2_;
                numbers[4].gameObject.transform.localPosition = numberPosition3_;
                numbers[5].gameObject.transform.localPosition = numberPosition7_;
                numbers[6].gameObject.transform.localPosition = numberPosition11_;
                numbers[7].gameObject.transform.localPosition = numberPosition15_;

                positionCnt = 8;

                numberScale.x = numberScale.y = 0.7f;
                numberPosition_.y = 0.1f;
                numberRoot.transform.localScale = numberScale;
                numberRoot.transform.localPosition = numberPosition_;
            }
            else if (number >= 100000000)
            {
                numbers[0].gameObject.SetActive(true);
                numbers[1].gameObject.SetActive(true);
                numbers[2].gameObject.SetActive(true);
                numbers[3].gameObject.SetActive(true);
                numbers[4].gameObject.SetActive(true);
                numbers[5].gameObject.SetActive(true);
                numbers[6].gameObject.SetActive(true);
                numbers[7].gameObject.SetActive(true);
                numbers[8].gameObject.SetActive(true);
                numbers[0].gameObject.transform.localPosition = numberPosition16_;
                numbers[1].gameObject.transform.localPosition = numberPosition12_;
                numbers[2].gameObject.transform.localPosition = numberPosition8_;
                numbers[3].gameObject.transform.localPosition = numberPosition4_;
                numbers[4].gameObject.transform.localPosition = numberPosition1_;
                numbers[5].gameObject.transform.localPosition = numberPosition5_;
                numbers[6].gameObject.transform.localPosition = numberPosition9_;
                numbers[7].gameObject.transform.localPosition = numberPosition13_;
                numbers[8].gameObject.transform.localPosition = numberPosition17_;

                positionCnt = 9;

                numberScale.x = numberScale.y = 0.6f;
                numberPosition_.y = 0.15f;
                numberRoot.transform.localScale = numberScale;
                numberRoot.transform.localPosition = numberPosition_;
            }

            string strNumber = number.ToString();
            string spriteName = "";

            for (int i = 0; i < positionCnt; ++i)
            {
                string temp = strNumber.Substring(i, 1);

                if (temp.CompareTo("0") == 0)
                {
                    spriteName = "0";
                }
                else if (temp.CompareTo("1") == 0)
                {
                    spriteName = "1";
                }
                else if (temp.CompareTo("2") == 0)
                {
                    spriteName = "2";
                }
                else if (temp.CompareTo("3") == 0)
                {
                    spriteName = "3";
                }
                else if (temp.CompareTo("4") == 0)
                {
                    spriteName = "4";
                }
                else if (temp.CompareTo("5") == 0)
                {
                    spriteName = "5";
                }
                else if (temp.CompareTo("6") == 0)
                {
                    spriteName = "6";
                }
                else if (temp.CompareTo("7") == 0)
                {
                    spriteName = "7";
                }
                else if (temp.CompareTo("8") == 0)
                {
                    spriteName = "8";
                }
                else if (temp.CompareTo("9") == 0)
                {
                    spriteName = "9";
                }

                string path = "CommonUI/" + spriteName;
                numbers[i].sprite = Resources.Load<Sprite>(path);

            }
        }
    }
}
