using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sinabro
{

    public class TableLoader : MonoBehaviour
    {

        void Start()
        {
            LoadData();
        }

        //
        public void LoadData()
        {
            //
            int language = PlayerPrefs.GetInt("Lanuage", -1);
            if (language == -1)
            {
                //set system language
                SystemLanguage systemLanguage = Application.systemLanguage;
                if (SystemLanguage.English == systemLanguage)
                {
                    language = (int)LanguageType.English;
                }
                else if (SystemLanguage.Korean == systemLanguage)
                {
                    language = (int)LanguageType.Korean;
                }
                else if (SystemLanguage.Japanese == systemLanguage)
                {
                    language = (int)LanguageType.Japanese;
                }
                else if (SystemLanguage.ChineseSimplified == systemLanguage)
                {
                    language = (int)LanguageType.Cn;
                }
                else if (SystemLanguage.ChineseTraditional == systemLanguage)
                {
                    language = (int)LanguageType.Tw;
                }
                else
                {
                    language = (int)LanguageType.English;
                }

                PlayerPrefs.SetInt("Lanuage", language);

            }

            LoadLocalTable(language);


            //----------------------------------------------------------------------------------------------
            // sound table Load
            //----------------------------------------------------------------------------------------------
            SoundMgr.Instance.g_soundTable.Clear();
            SoundTable soundTable;
            soundTable = GetComponent<SoundTable>();
            soundTable.LoadData("Table/SoundTable");
            int dataCnt = soundTable.GetDataSize();
            SoundTable.DataEntity soundDatas = soundTable.GetData();

            for (int i = 0; i < dataCnt; ++i)
            {
                SoundInfo soundInfo = new SoundInfo();
                soundInfo.id = soundDatas.Datas[i].Id;
                soundInfo.type = soundDatas.Datas[i].Type;
                soundInfo.resourceName = soundDatas.Datas[i].ResourceName;

                SoundMgr.Instance.g_soundTable.Add(soundInfo.id, soundInfo);
            }
        }


        //
        public void ChangeLocalize(int nationCode)
        {
            PlayerPrefs.SetInt("Lanuage", nationCode);

            // english = 0
            LoadLocalTable(nationCode);
        }

        //
        private void LoadLocalTable(int nationCode)
        {
            //
            int dataCnt = 0;

            DataMgr.Instance.g_localizeTextTable.Clear();
            LocalizeTextTable localizeText;
            localizeText = GetComponent<LocalizeTextTable>();
            localizeText.LoadData("Table/LocalizeTextTable");
            dataCnt = localizeText.GetDataSize();
            LocalizeTextTable.DataEntity localizeTextDatas = localizeText.GetData();

            int textId;
            string strText;
            for (int i = 0; i < dataCnt; ++i)
            {
                textId = localizeTextDatas.Texts[i].Id;
                if (nationCode == (int)LanguageType.Korean)
                {
                    strText = localizeTextDatas.Texts[i].KrString;
                    DataMgr.Instance.g_localizeTextTable.Add(textId, strText);
                }
                else if (nationCode == (int)LanguageType.English)
                {
                    strText = localizeTextDatas.Texts[i].EngString;
                    DataMgr.Instance.g_localizeTextTable.Add(textId, strText);
                }
                else if (nationCode == (int)LanguageType.Japanese)
                {
                    strText = localizeTextDatas.Texts[i].JpString;
                    DataMgr.Instance.g_localizeTextTable.Add(textId, strText);
                }
                else if (nationCode == (int)LanguageType.Cn)
                {
                    strText = localizeTextDatas.Texts[i].CnString;
                    DataMgr.Instance.g_localizeTextTable.Add(textId, strText);
                }
                else if (nationCode == (int)LanguageType.Tw)
                {
                    strText = localizeTextDatas.Texts[i].TwString;
                    DataMgr.Instance.g_localizeTextTable.Add(textId, strText);
                }
            }
        }
    }
}
