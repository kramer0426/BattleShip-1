using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sinabro
{

    public class TableLoader : MonoBehaviour
    {
        [SerializeField] LocalTextExcel localTextExcel_;
        [SerializeField] BattleShipDataExcel battleShipDataExcel_;

        void Start()
        {
            LoadData();
        }

        //
        public void LoadData()
        {
            //
            DataMgr.Instance.g_localTextExcel = localTextExcel_;
            DataMgr.Instance.g_battleShipExcel = battleShipDataExcel_;

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

            // test code
            language = (int)LanguageType.Korean;

            // english = 0
            DataMgr.Instance.currentLanguage_g = language;

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


    }
}
